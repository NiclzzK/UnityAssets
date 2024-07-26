using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform firstPersonCamera;
    public float cameraSensitivity = 2.0f;
    public bool invertYAxis = true;
    public float normalFOV = 75.0f;
    public float sprintFOV = 90.0f;
    public float fovTransitionSpeed = 5.0f;

    [Header("Movement Settings")]
    public float walkSpeed = 6.0f;
    public float sprintSpeed = 8.0f;
    public float crouchSpeed = 2.5f;
    public float gravity = -9.81f;

    [Header("Slide Settings")]
    public float maxClimbAngle = 40.0f;
    public float slideForce = 5.0f;
    public float playerInputInfluence = 0.5f;

    [Header("Jump Settings")]
    public float jumpHeight = 1.0f;
    public float maxJumpTime = 0.5f;
    public float airControlFactor = 0.5f;
    public float coyoteTime = 0.2f;
    public float jumpCooldown = 0.2f;

    [Header("Crouch Settings")]
    public float crouchHeight = 1.0f;
    public float crouchTransitionSpeed = 5.0f;

    private Camera mainCamera;
    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;
    private float defaultHeight;
    private float defaultCenterY;
    private float pitch = 0.0f;
    private float lastGroundedTime;
    private float lastJumpTime;
    private bool isJumping;
    private float jumpTimer;
    private bool isCrouching;

    void Start()
    {
        mainCamera = Camera.main;
        mainCamera.transform.position = firstPersonCamera.position;
        mainCamera.transform.rotation = firstPersonCamera.rotation;
        mainCamera.fieldOfView = normalFOV;
        characterController = GetComponent<CharacterController>();
        defaultHeight = characterController.height;
        defaultCenterY = characterController.center.y;
        characterController.slopeLimit = maxClimbAngle;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMovement();
        HandleCamera();
        AdjustFOV();
    }

    void HandleMovement()
    {
        isGrounded = characterController.isGrounded;
        if (isGrounded)
        {
            lastGroundedTime = Time.time;
            if (velocity.y < 0)
            {
                velocity.y = -2f;
            }
        }

        float currentSpeed = walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) currentSpeed = sprintSpeed;

        HandleCrouching(ref currentSpeed);

        Vector3 move = transform.forward * Input.GetAxis("Vertical") * currentSpeed + transform.right * Input.GetAxis("Horizontal") * currentSpeed;
        if (!isGrounded) move *= airControlFactor;

        HandleSliding(ref move, currentSpeed);

        characterController.Move(move * Time.deltaTime);

        HandleJumping();

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    void HandleCrouching(ref float currentSpeed)
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            currentSpeed = crouchSpeed;
            if (!isCrouching)
            {
                isCrouching = true;
                characterController.height = crouchHeight;
                characterController.center = new Vector3(characterController.center.x, crouchHeight / 2, characterController.center.z);
            }
        }
        else
        {
            if (isCrouching)
            {
                isCrouching = false;
                characterController.height = defaultHeight;
                characterController.center = new Vector3(characterController.center.x, defaultCenterY, characterController.center.z);
            }
        }
    }

    void HandleJumping()
    {
        if (Input.GetButtonDown("Jump") && (isGrounded || Time.time - lastGroundedTime <= coyoteTime) && Time.time - lastJumpTime >= jumpCooldown)
        {
            isJumping = true;
            jumpTimer = 0f;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            lastJumpTime = Time.time;
        }

        if (Input.GetButton("Jump") && isJumping)
        {
            jumpTimer += Time.deltaTime;
            if (jumpTimer < maxJumpTime)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
    }

    void HandleCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * cameraSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity * (invertYAxis ? 1 : -1);

        transform.Rotate(Vector3.up * mouseX);
        pitch = Mathf.Clamp(pitch - mouseY, -90f, 90f);
        firstPersonCamera.localRotation = Quaternion.Euler(pitch, 0, 0);
    }

    void AdjustFOV()
    {
        float targetFOV = Input.GetKey(KeyCode.LeftShift) ? sprintFOV : normalFOV;
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime * fovTransitionSpeed);
    }

    void HandleSliding(ref Vector3 move, float currentSpeed)
    {
        if (isGrounded && OnSteepSlope(out Vector3 slopeNormal))
        {
            float slopeAngle = Vector3.Angle(Vector3.up, slopeNormal);
            if (slopeAngle > maxClimbAngle)
            {
                Vector3 slideDirection = Vector3.ProjectOnPlane(Vector3.down, slopeNormal).normalized;
                Vector3 playerInput = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
                move = slideDirection * slideForce + playerInput * currentSpeed * playerInputInfluence;
                move = Vector3.ProjectOnPlane(move, slopeNormal);
            }
        }
    }

    bool OnSteepSlope(out Vector3 slopeNormal)
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, characterController.height / 2 + 0.5f))
        {
            slopeNormal = hit.normal;
            float slopeAngle = Vector3.Angle(Vector3.up, slopeNormal);
            return slopeAngle > characterController.slopeLimit;
        }
        slopeNormal = Vector3.up;
        return false;
    }
}
