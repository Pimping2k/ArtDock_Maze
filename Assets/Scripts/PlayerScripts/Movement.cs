using System;
using UnityEngine;

namespace PlayerScripts
{
    public class Movement : MonoBehaviour
    {
        public float walkSpeed = 6f;
        public float runSpeed = 12f;

        public float lookSpeed = 2f;
        public float lookXLimit = 45f;

        Vector3 moveDirection = Vector3.zero;
        float rotationX = 0;

        CharacterController characterController;
        public bool canMove = true;
        bool isRunning;
        Vector2 moveInput;

        private InputSystem_Actions _inputSystemActions;

        private void Start()
        {
            characterController = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            _inputSystemActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            _inputSystemActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        }

        private void Update()
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * moveInput.y : 0;
            float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * moveInput.x : 0;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        }
    }
}