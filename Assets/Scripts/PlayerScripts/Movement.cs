using UnityEngine;

namespace PlayerScripts
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        [SerializeField,Range(1,100)] private float walkSpeed;
        [SerializeField,Range(1,100)] private float runSpeed;
        [SerializeField,Range(1,20)] private float jumpPower;
        [SerializeField] private float groundCheckDistance = 0.2f;

        [SerializeField] private bool canMove = true;
        
        private Vector3 moveDirection;
        private Vector2 moveInput;
        private bool isRunning;
        private bool jumpPressed;
        private bool isGrounded;

        private Rigidbody rb;
        private InputSystem_Actions _inputSystemActions;

        private void Awake()
        {
            _inputSystemActions = new InputSystem_Actions();
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true; // Отключаем физический поворот, чтобы не падал
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            var startWalkSpeed = walkSpeed;
            
            _inputSystemActions.Player.Enable();
            _inputSystemActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            _inputSystemActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
            _inputSystemActions.Player.Jump.performed += ctx => jumpPressed = true;
            _inputSystemActions.Player.Sprint.performed += ctx => walkSpeed = runSpeed;
            _inputSystemActions.Player.Sprint.canceled += ctx => walkSpeed = startWalkSpeed;
        }

        private void FixedUpdate()
        {
            HandleMovement();
            HandleJump();
        }

        private void HandleMovement()
        {
            if (!canMove) return;

            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

            float curSpeedX = (isRunning ? runSpeed : walkSpeed) * moveInput.y;
            float curSpeedY = (isRunning ? runSpeed : walkSpeed) * moveInput.x;

            Vector3 moveVelocity = (forward * curSpeedX) + (right * curSpeedY);
            rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);

            _animator.SetFloat("VelocityX", moveInput.x);
            _animator.SetFloat("VelocityY", moveInput.y);
        }

        private void HandleJump()
        {
            isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
            
            if (jumpPressed && isGrounded)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpPower, rb.linearVelocity.z);
            }

            jumpPressed = false;
        }
    }
}
