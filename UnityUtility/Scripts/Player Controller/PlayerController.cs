using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Objects")]
    [Header("----------------------------------------------------------------------------------------------------------")]
    public CharacterController controller;
    public Rigidbody rb;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public Collider collider;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    public Camera playerCamera;

    [Header("Camera Options")]
    [Header("----------------------------------------------------------------------------------------------------------")]
    public float mouseSensitivity;
    public float sprintingFOV;
    [Range(0f, 0.5f)] public float cameraSmoothness;

    [Header("Movement Options")]
    [Header("----------------------------------------------------------------------------------------------------------")]
    public float walkSpeed;
    public float sprintSpeed;
    [Range(0f, 0.5f)] public float smoothness;
    public float jumpHeight;

    [Header("Other Options")]
    [Header("----------------------------------------------------------------------------------------------------------")]
    public MovementType movementType;
    public float gravity = -9.81f;

    [Header("Required Options")]
    [Header("----------------------------------------------------------------------------------------------------------")]
    public LayerMask everythingMask;
    public LayerMask groundMask;

    [Header("Player Stats")]
    [Header("----------------------------------------------------------------------------------------------------------")]
    public bool sprinting;
    public bool isGrounded;
    public float yVelocity;

    private float cameraPitch = 0.0f;
    private Vector2 currentDirection = Vector2.zero;
    private Vector2 currentDirectionVelocity = Vector2.zero;
    private Vector2 currentMouseDelta = Vector2.zero;
    private Vector2 currentMouseDeltaVelocity = Vector2.zero;

    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        playerCamera.depthTextureMode = DepthTextureMode.Depth;

        if (movementType == MovementType.Physics) controller.enabled = false;
        else if (movementType == MovementType.CharacterController) collider.enabled = false;
    }

    void Update() {
        MouseLook();
        if (movementType == MovementType.Physics) PhysicsMovement();
        else Movement();
        if (Input.GetKey(KeyCode.LeftShift) && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))) sprinting = true;
        else sprinting = false;
        if (sprinting) playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, sprintingFOV, 3 * Time.deltaTime);
        else playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, 90, 5 * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void MouseLook() {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, cameraSmoothness);

        cameraPitch -= currentMouseDelta.y;
        cameraPitch = Mathf.Clamp(cameraPitch, -90, 90);

        playerCamera.transform.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    void Movement() {
        Vector2 targetDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDirection.Normalize();

        currentDirection = Vector2.SmoothDamp(currentDirection, targetDirection, ref currentDirectionVelocity, smoothness);

        if (controller.isGrounded) { yVelocity = 0; }
        yVelocity += gravity * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) rb.AddForce(Vector3.up * jumpHeight * 100);

        Vector3 velocity = (transform.forward * currentDirection.y + transform.right * currentDirection.x) * (!sprinting ? walkSpeed : sprintSpeed) + Vector3.up * yVelocity;
        controller.Move(velocity * Time.deltaTime);
    }

    public void PhysicsMovement() {
        float speed = (sprinting ? sprintSpeed : walkSpeed);
        isGrounded = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), 0.4f, groundMask);
        float x = Input.GetAxisRaw("Horizontal") * speed;
        float z = Input.GetAxisRaw("Vertical") * speed;

        Vector3 movePosition = transform.right * x + transform.forward * z;
        Vector3 newMovePosition = new Vector3(movePosition.x, rb.velocity.y, movePosition.z);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) rb.AddForce(Vector3.up * jumpHeight * 100 * rb.mass);

        rb.velocity = newMovePosition; 
    }
}

public enum MovementType
{
    Physics,
    CharacterController,
}