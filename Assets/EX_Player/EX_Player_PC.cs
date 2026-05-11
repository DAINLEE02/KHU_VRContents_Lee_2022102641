using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems; //마우스 클릭이 발생했을 때 그 클릭이 UI(버튼, 인벤토리 창 등) 위에서 일어난 것인지 확인하기 위해 필요

[RequireComponent(typeof(CharacterController))]
public class EX_Player_PC : MonoBehaviour
{
    CharacterController Character;

    [Header("Camera")]
    public Transform CameraPivot;
    public float mouseSensitivity = 0.1f;
    float yaw;
    float pitch;

    [Header("Move/Run/Jump/Climb")]
    public float walkSpeed = 1.5f;
    public float runSpeed = 3.5f;
    public float friction = 0.9f;
    public float climbSpeed = 1.2f;
    public float jumpHeight = 0.5f;
    public float gravity = -9.81f;

    // State Variables
    Vector3 Velocity;
    Vector2 MoveInput;
    Vector2 LookInput;

    bool isSprinting;
    bool jumpPressed;

    enum PlayerState { Ground, Air, Climb }
    PlayerState state = PlayerState.Ground;

    enum ClimbType { None, Ladder, Cliff }
    ClimbType climbType = ClimbType.None;

    void Start()
    {
        Character = GetComponent<CharacterController>();

        if (CameraPivot == null)
            CameraPivot = transform.Find("CameraRig/CameraPivot");

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 1. 입력 읽기 (InputActions 대신 직접 호출)
        GatherInput();

        // 2. 커서 제어
        HandleCursor();

        // 3. 로직 처리
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Look();
        }

        StateMachine();
        Character.Move(Velocity * Time.deltaTime);

        // 점프 입력은 한 프레임만 처리되도록 초기화 (GatherInput에서 다시 체크함)
        jumpPressed = false;
    }

    void HandleCursor()
    {
        var keyboard = Keyboard.current;
        var mouse = Mouse.current;
        if (keyboard == null || mouse == null) return;

        if (keyboard.escapeKey.wasPressedThisFrame || mouse.middleButton.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (mouse.leftButton.wasPressedThisFrame)
        {
            // 마우스 클릭이 발생했을 때, 그 클릭이 UI(버튼, 인벤토리 창 등) 위에서 일어난 것인지 확인
            bool isOverUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
            // 만약 마우스가 UI 오브젝트 위에 있는 것이 아니라면 잠금/숨김
            if (!isOverUI)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    /// <summary>
    /// InputActions를 거치지 않고 키보드/마우스에서 직접 값을 가져옴
    /// </summary>
    void GatherInput()
    {
        // 키보드 객체가 있는지 확인
        var keyboard = Keyboard.current;
        var mouse = Mouse.current;

        if (keyboard == null || mouse == null) return;

        // 이동 입력 (WASD)
        Vector2 move = Vector2.zero;
        if (keyboard.wKey.isPressed) move.y += 1f;
        if (keyboard.sKey.isPressed) move.y -= 1f;
        if (keyboard.aKey.isPressed) move.x -= 1f;
        if (keyboard.dKey.isPressed) move.x += 1f;
        if (keyboard.upArrowKey.isPressed) move.y += 1f;
        if (keyboard.downArrowKey.isPressed) move.y -= 1f;
        if (keyboard.leftArrowKey.isPressed) move.x -= 1f;
        if (keyboard.rightArrowKey.isPressed) move.x += 1f;
        MoveInput = move;

        // 시선 입력 (Mouse Delta)
        LookInput = mouse.delta.ReadValue();

        // 점프 입력 (Space) - 이번 프레임에 눌렸는지 확인
        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            jumpPressed = true;
        }

        // 스프린트 입력 (Left Shift)
        isSprinting = keyboard.leftShiftKey.isPressed || keyboard.rightShiftKey.isPressed;
    }

    // -------- Camera --------

    void Look()
    {
        // LookInput은 GatherInput에서 마우스 델타값을 받아옴
        yaw += LookInput.x * mouseSensitivity;
        pitch -= LookInput.y * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, -90f, 60f); // Up, Down

        transform.rotation = Quaternion.Euler(0, yaw, 0);
        CameraPivot.localRotation = Quaternion.Euler(pitch, 0, 0);
    }

    // -------- State Machine & Movement (로직은 동일) --------

    void StateMachine()
    {
        switch (state)
        {
            case PlayerState.Ground:
                GroundMove();
                break;

            case PlayerState.Air:
                AirMove();
                break;

            case PlayerState.Climb:
                ClimbMove();
                break;
        }
    }

    // -------- Ground --------
    void GroundMove()
    {
        Vector3 inputDir = transform.forward * MoveInput.y + transform.right * MoveInput.x;

        if (inputDir.sqrMagnitude > 1)
        {
            inputDir.Normalize();
        }

        float speed = isSprinting ? runSpeed : walkSpeed;

        if (inputDir.magnitude > 0.1f)
        {
            Velocity.x = inputDir.x * speed;
            Velocity.z = inputDir.z * speed;
        }
        else
        {
            Velocity.x *= friction;
            Velocity.z *= friction;
        }

        Velocity.y = -2f;

        if (jumpPressed)
        {
            Velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            state = PlayerState.Air;
            return;
        }

        if (!Character.isGrounded)
        {
            state = PlayerState.Air;
        }
    }

    void AirMove()
    {
        Vector3 inputDir = transform.forward * MoveInput.y + transform.right * MoveInput.x;
        Velocity.x = inputDir.x * walkSpeed;
        Velocity.z = inputDir.z * walkSpeed;
        Velocity.y += gravity * Time.deltaTime;

        if (Character.isGrounded)
        {
            state = PlayerState.Ground;
        }
    }

    void ClimbMove()
    {
        Velocity = Vector3.zero;

        if (climbType == ClimbType.Ladder)
        {
            Velocity.y = MoveInput.y * climbSpeed;
        }
        else if (climbType == ClimbType.Cliff)
        {
            Velocity = transform.right * MoveInput.x * climbSpeed + Vector3.up * MoveInput.y * climbSpeed;
            Velocity += -transform.forward * 0.1f;
        }

        if (jumpPressed)
        {
            Vector3 jumpDir = -transform.forward + Vector3.up;
            float jumpForce = Mathf.Sqrt(jumpHeight * -2f * gravity);
            Velocity = jumpDir.normalized * jumpForce;
            state = PlayerState.Air;
        }
    }

    // -------- Trigger (동일) --------
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ClimbableLadder"))
        {
            climbType = ClimbType.Ladder;
            state = PlayerState.Climb;
            Velocity = Vector3.zero;
        }

        if (other.CompareTag("ClimbableCliff"))
        {
            climbType = ClimbType.Cliff;
            state = PlayerState.Climb;
            Velocity = Vector3.zero;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ClimbableLadder") ||
            other.CompareTag("ClimbableCliff"))
        {
            state = PlayerState.Air;
        }
    }
}