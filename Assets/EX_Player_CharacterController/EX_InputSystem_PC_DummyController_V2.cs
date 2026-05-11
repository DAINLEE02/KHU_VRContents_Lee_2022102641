using UnityEngine;
using UnityEngine.InputSystem;

public class EX_InputSystem_PC_DummyController_V2 : MonoBehaviour
{
    [Header("Dependencies")]
    public Transform playerCamera;
    Transform controllerModel;
    public GameObject hitCursor;
    private LineRenderer laserLine;

    [Header("Positioning")]
    public Vector3 screenOffset = new Vector3(0.3f, -0.25f, 0.6f); // Right
    //public Vector3 screenOffset = new Vector3(-0.3f, -0.25f, 0.6f);  // Left

    [Header("Interaction (Grab)")]
    public float grabRange = 10f;
    public LayerMask grabLayer;

    [Header("Grab Settings")]
    public Transform grabPoint; // 물체가 붙을 위치 (없으면 controllerModel 사용)
    private Rigidbody grabbedObject; // 현재 잡고 있는 물체
    private bool isGrabbing = false;

    // --- 오프셋 유지를 위한 변수 ---
    private Vector3 positionOffset;
    private Quaternion rotationOffset;

    private IRayInteractable currentTarget;

    void Start()
    {
        if (playerCamera == null) playerCamera = Camera.main.transform;
        controllerModel = transform;

        laserLine = controllerModel.GetComponent<LineRenderer>();
        if (laserLine != null)
        {
            laserLine.positionCount = 2;
            laserLine.enabled = false;
        }

        if (hitCursor != null) hitCursor.SetActive(false);

        // GrabPoint가 설정되지 않았다면 컨트롤러 앞쪽 적당한 곳으로 자동 생성
        if (grabPoint == null)
        {
            GameObject gp = new GameObject("GrabPoint");
            gp.transform.SetParent(controllerModel);
            gp.transform.localPosition = new Vector3(0, 0, 1.0f); // 컨트롤러 앞 1m
            grabPoint = gp.transform;
        }
    }

    void LateUpdate()
    {
        // 컨트롤러 모델 위치 업데이트
        controllerModel.position = playerCamera.position + playerCamera.TransformDirection(screenOffset);
        controllerModel.rotation = playerCamera.rotation;

        HandleInteraction();

        // 잡고 있는 물체가 있다면 위치 업데이트
        if (isGrabbing && grabbedObject != null)
        {
            UpdateGrabbedObject();
        }
    }

    void HandleInteraction()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            // 그랩 여부와 상관없이 왼쪽 버튼을 누르고 있으면 레이저 업데이트
            if (mouse.leftButton.isPressed)
            {
                UpdateLaser(mouse);
            }

            // 버튼을 뗄 때 처리
            if (mouse.leftButton.wasReleasedThisFrame)
            {
                if (isGrabbing) ReleaseObject(); // 잡고 있었다면 놓기
                DisableLaser();                  // 레이저 끄기
            }
        }
    }

    void UpdateLaser(Mouse mouse)
    {
        if (laserLine == null) return;
        laserLine.enabled = true;
        laserLine.SetPosition(0, controllerModel.position);

        Ray ray = new Ray(controllerModel.position, controllerModel.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, grabRange, grabLayer))
        {
            laserLine.SetPosition(1, hit.point);
            UpdateCursor(hit);

            // 1. 인터페이스 처리 (스크립트가 있는 경우만 실행)
            IRayInteractable interactable = hit.collider.GetComponent<IRayInteractable>();
            if (interactable != null)
            {
                if (currentTarget != interactable)
                {
                    currentTarget?.OnRayExit();
                    currentTarget = interactable;
                    currentTarget.OnRayEnter();
                }
                currentTarget.OnRayStay();
                if (mouse.leftButton.wasPressedThisFrame) currentTarget.OnRayClick();
            }
            else
            {
                ClearCurrentTarget();
            }

            // 2. 그랩 처리 (핵심: 인터페이스 유무와 상관없이 Rigidbody만 있으면 실행)
            if (mouse.leftButton.wasPressedThisFrame)
            {
                Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    GrabObject(rb); // 이제 스크립트 없는 상자도 잡힙니다!
                }
            }
        }
        else
        {
            laserLine.SetPosition(1, controllerModel.position + (controllerModel.forward * grabRange));
            if (hitCursor != null) hitCursor.SetActive(false);
            ClearCurrentTarget();
        }
    }

    // --- 그랩 관련 핵심 메소드 ---

    private void GrabObject(Rigidbody rb)
    {
        print("try grab");
        isGrabbing = true;
        grabbedObject = rb;

        // 1. 물리 엔진 영향 일시 정지
        grabbedObject.useGravity = false;
        grabbedObject.isKinematic = true;

        // 2. 잡는 순간의 상대적 위치와 회전 계산 (핵심)
        // 컨트롤러 기준에서 물체가 어디에 있는지 역계산하여 저장합니다.
        positionOffset = controllerModel.InverseTransformPoint(grabbedObject.position);
        rotationOffset = Quaternion.Inverse(controllerModel.rotation) * grabbedObject.rotation;

        //if (laserLine != null) laserLine.enabled = false;
        //if (hitCursor != null) hitCursor.SetActive(false);
    }

    private void UpdateGrabbedObject()
    {
        // 저장된 오프셋을 컨트롤러의 현재 위치/회전에 적용
        grabbedObject.position = controllerModel.TransformPoint(positionOffset);
        grabbedObject.rotation = controllerModel.rotation * rotationOffset;
    }

    private void ReleaseObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.useGravity = true;
            grabbedObject.isKinematic = false;
            grabbedObject = null;
        }
        isGrabbing = false;
    }

    // -------------------------

    private void UpdateCursor(RaycastHit hit)
    {
        if (hitCursor != null)
        {
            hitCursor.SetActive(true);
            hitCursor.transform.position = hit.point + (hit.normal * 0.01f);
            hitCursor.transform.rotation = Quaternion.LookRotation(hit.normal);
        }
    }

    private void DisableLaser()
    {
        //if (!isGrabbing) // 잡는 중이 아닐 때만 레이저 비활성화
        //{
        //    if (laserLine != null) laserLine.enabled = false;
        //    if (hitCursor != null) hitCursor.SetActive(false);
        //    ClearCurrentTarget();
        //}
        if (laserLine != null) laserLine.enabled = false;
        if (hitCursor != null) hitCursor.SetActive(false);
        ClearCurrentTarget();
    }

    private void ClearCurrentTarget()
    {
        if (currentTarget != null)
        {
            currentTarget.OnRayExit();
            currentTarget = null;
        }
    }
}