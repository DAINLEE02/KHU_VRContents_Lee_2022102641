using UnityEngine;
using UnityEngine.InputSystem;

public class EX_InputSystem_PC_DummyController_V1 : MonoBehaviour
{
    [Header("Dependencies")]
    public Transform playerCamera;
    public Transform controllerModel;
    public GameObject hitCursor;
    private LineRenderer laserLine;

    [Header("Positioning")]
    public Vector3 screenOffset = new Vector3(0.3f, -0.25f, 0.6f);

    [Header("Interaction")]
    public float interactRange = 10f;
    public LayerMask interactLayer;

    // 인터랙션 상태 관리를 위한 변수
    private IRayInteractable currentTarget;

    void Start()
    {
        if (playerCamera == null) playerCamera = Camera.main.transform;

        laserLine = controllerModel.GetComponent<LineRenderer>();
        if (laserLine != null)
        {
            laserLine.positionCount = 2;
            laserLine.enabled = false;
        }

        if (hitCursor != null) hitCursor.SetActive(false);
    }

    void LateUpdate()
    {
        // 컨트롤러 모델 위치 업데이트
        controllerModel.position = playerCamera.position + playerCamera.TransformDirection(screenOffset);
        controllerModel.rotation = playerCamera.rotation;

        HandleInteraction();
    }

    void HandleInteraction()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            // 마우스 왼쪽 버튼을 누르고 있는 동안 레이 활성화
            if (mouse.leftButton.isPressed)
            {
                UpdateLaser(mouse);
            }

            // 버튼을 떼는 순간 처리
            if (mouse.leftButton.wasReleasedThisFrame)
            {
                DisableLaser();
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

        if (Physics.Raycast(ray, out hit, interactRange, interactLayer))
        {
            // 1. 시각적 처리 (라인 및 커서)
            laserLine.SetPosition(1, hit.point);
            UpdateCursor(hit);

            // 2. 인터페이스 인터랙션 처리
            IRayInteractable interactable = hit.collider.GetComponent<IRayInteractable>();

            if (interactable != null)
            {
                // 새로운 타겟에 진입한 경우
                if (currentTarget != interactable)
                {
                    currentTarget?.OnRayExit(); // 이전 타겟 퇴장
                    currentTarget = interactable;
                    currentTarget.OnRayEnter(); // 새 타겟 진입
                }

                // 매 프레임 실행
                currentTarget.OnRayStay();

                // 클릭 순간 (Input System의 wasPressedThisFrame 사용)
                if (mouse.leftButton.wasPressedThisFrame)
                {
                    currentTarget.OnRayClick();
                }
            }
            else
            {
                // 레이는 맞았으나 인터랙터블이 아닌 경우
                ClearCurrentTarget();
            }
        }
        else
        {
            // 허공을 쏠 때
            laserLine.SetPosition(1, controllerModel.position + (controllerModel.forward * interactRange));
            if (hitCursor != null) hitCursor.SetActive(false);
            ClearCurrentTarget();
        }
    }

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