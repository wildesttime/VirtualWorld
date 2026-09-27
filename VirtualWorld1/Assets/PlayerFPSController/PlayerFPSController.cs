using UnityEngine;

public class PlayerFPSController : MonoBehaviour
{
    [Header("Скорость передвижения")]
    public float walkingSpeed = 5f;

    [Header("Чувствительность обзора")]
    public float lookSensitivity = 2f;

    [Header("Прыжок")]
    public float jumpPower = 5f;

    [Header("Физика")]
    public float gravityForce = -9.81f;

    [Header("Ссылки")]
    public Transform playerCamera;

    private CharacterController charController;
    private Vector3 verticalVelocity;
    private float cameraPitch = 0f;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
        LockCursor();
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        RotateView();
        MovePlayer();
    }

    private void RotateView()
    {
        float mouseInputX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseInputY = Input.GetAxis("Mouse Y") * lookSensitivity;

        transform.Rotate(0f, mouseInputX, 0f);

        cameraPitch -= mouseInputY;
        cameraPitch = Mathf.Clamp(cameraPitch, -80f, 80f);
        playerCamera.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    private void MovePlayer()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * inputX + transform.forward * inputZ;
        charController.Move(moveDirection * walkingSpeed * Time.deltaTime);

        ApplyGravityAndJump();
    }

    private void ApplyGravityAndJump()
    {
        if (charController.isGrounded)
        {
            if (verticalVelocity.y < 0f)
            {
                verticalVelocity.y = -2f;
            }

            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity.y = jumpPower;
            }
        }

        verticalVelocity.y += gravityForce * Time.deltaTime;
        charController.Move(verticalVelocity * Time.deltaTime);
    }
}