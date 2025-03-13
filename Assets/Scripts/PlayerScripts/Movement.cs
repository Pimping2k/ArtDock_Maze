using System;
using UnityEngine;

namespace PlayerScripts
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        [SerializeField] private float walkSpeed = 6f;
        [SerializeField] private float runSpeed = 12f;
        [SerializeField] private float jumpPower = 0f;
        [SerializeField] private float gravity = 10f;

        [SerializeField] private float lookSpeed = 2f;
        [SerializeField] private float lookXLimit = 45f;
        [SerializeField] private bool canMove = true;

        Vector3 moveDirection = Vector3.zero;
        float rotationX = 0;

        CharacterController characterController;

        bool isRunning;
        bool jumpPressed;

        Vector2 moveInput;

        private InputSystem_Actions _inputSystemActions;

        private void Awake()
        {
            _inputSystemActions = new InputSystem_Actions();
        }

        private void Start()
        {
            characterController = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            _inputSystemActions.Player.Enable();

            _inputSystemActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            _inputSystemActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

            _inputSystemActions.Player.Jump.performed += ctx => jumpPressed = true;
            _inputSystemActions.Player.Jump.canceled += ctx => jumpPressed = false;
        }

        private void Update()
        {
            #region HandleMovement

            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * moveInput.y : 0;
            float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * moveInput.x : 0;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            _animator.SetFloat("VelocityX", moveInput.x);
            _animator.SetFloat("VelocityY", moveInput.y);
            
            #endregion

            #region HandleJump

            if (jumpPressed && canMove && characterController.isGrounded)
            {
                moveDirection.y = jumpPower;
            }
            else
            {
                moveDirection.y = movementDirectionY;
            }

            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }

            #endregion

            characterController.Move(moveDirection * Time.deltaTime);
        }
    }
}