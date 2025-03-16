using System;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerScripts
{
    public class PlayerRotation : MonoBehaviour
    {
        [Header("Settings")] [SerializeField] [Tooltip("Handle rotation speed")]
        private float rotationSpeed = 10f;

        [SerializeField] [Tooltip("Value of rotation upper body of player in pan")]
        private float rotationXAngle;

        [SerializeField] [Tooltip("Value of rotation upper body of player in tilt")]
        private float rotationZAngle;

        [Header("References")] [SerializeField]
        private Camera playerCamera;

        [SerializeField] private Transform lowerBodyParent;
        [SerializeField] private Transform lowerBody;
        [SerializeField] private Transform upperBodyParent;
        [SerializeField] private Transform upperBody;
        [SerializeField] private Transform weaponParent;
        [SerializeField] private Transform lScapula;
        [SerializeField] private Transform rScapula;

        private Vector2 moveInput;
        private Vector2 lookInput;
        private float currentXRotation;
        private float currentZRotation;

        private void Start()
        {
            upperBody.parent = upperBodyParent;
            lowerBody.parent = lowerBodyParent;
            lScapula.parent = weaponParent;
            rScapula.parent = weaponParent;

            InputManager.Instance.InputActions.Player.Move.performed += OnMovePerformed;
            InputManager.Instance.InputActions.Player.Move.canceled += OnMoveCanceled;
            InputManager.Instance.InputActions.Player.Look.performed += OnLookPerformed;
            InputManager.Instance.InputActions.Player.Look.canceled += OnLookCanceled;
        }
        private void OnMovePerformed(InputAction.CallbackContext obj) => moveInput = obj.ReadValue<Vector2>();
        private void OnMoveCanceled(InputAction.CallbackContext obj) => moveInput = Vector2.zero;
        private void OnLookPerformed(InputAction.CallbackContext obj) => lookInput = obj.ReadValue<Vector2>();
        private void OnLookCanceled(InputAction.CallbackContext obj) => lookInput = Vector2.zero;


        private void FixedUpdate()
        {
            HandleLowerBodyRotation();
            HandleUpperBodyRotation();
            HandleWeaponRotation();
        }

        private void HandleLowerBodyRotation()
        {
            if (moveInput.sqrMagnitude > 0.01f)
            {
                Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

                lowerBodyParent.localRotation = Quaternion.Slerp(lowerBodyParent.localRotation,
                    Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0),
                    Time.deltaTime * rotationSpeed);
            }
        }

        private void HandleUpperBodyRotation()
        {
            Vector2 mouseDelta = lookInput;
            mouseDelta *= rotationSpeed * Time.deltaTime;
            currentXRotation -= mouseDelta.x;
            currentZRotation += mouseDelta.y;

            currentXRotation = Mathf.Clamp(currentXRotation, -rotationXAngle, rotationXAngle);
            currentZRotation = Mathf.Clamp(currentZRotation, -rotationZAngle, rotationZAngle);

            upperBodyParent.transform.localRotation = Quaternion.Euler(currentXRotation, 0f, currentZRotation);
        }

        private void HandleWeaponRotation()
        {
            Quaternion targetRotation = Quaternion.Euler(currentXRotation, 0f, 0f);
            weaponParent.localRotation = Quaternion.Slerp(weaponParent.localRotation, targetRotation,
                Time.deltaTime * rotationSpeed);
        }

        private void OnDestroy()
        {
            InputManager.Instance.InputActions.Player.Move.performed += OnMovePerformed;
            InputManager.Instance.InputActions.Player.Move.canceled += OnMoveCanceled;
            InputManager.Instance.InputActions.Player.Look.performed += OnLookPerformed;
            InputManager.Instance.InputActions.Player.Look.canceled += OnLookCanceled;
        }
    }
}