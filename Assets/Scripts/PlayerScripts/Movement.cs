using Containers;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerScripts
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        [SerializeField, Range(1, 100)] private float walkSpeed;
        [SerializeField, Range(1, 20)] private float jumpPower;

        [SerializeField] private bool canMove = true;

        private Vector3 moveDirection;
        private Vector2 moveInput;
        private bool isRunning;
        private bool jumpPressed;
        private bool isGrounded;

        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            InputManager.Instance.InputActions.Player.Move.performed += OnMovePerformed;
            InputManager.Instance.InputActions.Player.Move.canceled += OnMoveCanceled;
        }

        private void OnMovePerformed(InputAction.CallbackContext obj) => moveInput = obj.ReadValue<Vector2>();
        private void OnMoveCanceled(InputAction.CallbackContext obj) => moveInput = Vector2.zero;


        private void FixedUpdate()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            if (!canMove) return;

            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

            float curSpeedX =walkSpeed * moveInput.y;
            float curSpeedY =walkSpeed * moveInput.x;

            Vector3 moveVelocity = (forward * curSpeedX) + (right * curSpeedY);
            rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);

            _animator.SetFloat(AnimatorTagsContainer.VELOCITYX, moveInput.x,0.1f,Time.fixedDeltaTime);
            _animator.SetFloat(AnimatorTagsContainer.VELOCITYY, moveInput.y,0.1f,Time.fixedDeltaTime);
        }

        private void OnDestroy()
        {
            InputManager.Instance.InputActions.Player.Move.performed -= OnMovePerformed;
            InputManager.Instance.InputActions.Player.Move.canceled -= OnMoveCanceled;
        }
    }
}