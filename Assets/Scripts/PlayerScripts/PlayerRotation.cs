using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerScripts
{
    public class PlayerRotation : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Camera playerCamera;

        [SerializeField] private Transform lowerBodyParent;
        [SerializeField] private Transform lowerBody;
        [SerializeField] private Transform upperBodyParent;
        [SerializeField] private Transform upperBody;
        [SerializeField] private Transform weaponParent;
        [SerializeField] private Transform lScapula;
        [SerializeField] private Transform rScapula;

        [Header("Settings")] [SerializeField] [Tooltip("Handle rotation speed")]
        private float rotationSpeed = 10f;

        [SerializeField] [Tooltip("Minimum value of rotation upper body of player")]
        private float minAngle;

        [SerializeField] [Tooltip("Maximum value of rotation upper body of player")]
        private float maxAngle;

        private Vector2 moveInput;
        private InputSystem_Actions _inputSystemActions;
        private Vector2 lookInput;
        private float currentXRotation;

        private void Awake()
        {
            _inputSystemActions = new InputSystem_Actions();
        }

        private void Start()
        {
            _inputSystemActions.Player.Enable();

            upperBody.parent = upperBodyParent;
            lowerBody.parent = lowerBodyParent;
            lScapula.parent = weaponParent;
            rScapula.parent = weaponParent;

            _inputSystemActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            _inputSystemActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
            _inputSystemActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            HandleLowerBodyRotation();
            HandleUpperBodyRotation();
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

            currentXRotation = Mathf.Clamp(currentXRotation, minAngle, maxAngle);
            upperBodyParent.transform.localRotation = Quaternion.Euler(currentXRotation, 0f, 0f);

            Quaternion targetRotation = Quaternion.Euler(currentXRotation,0f, 0f );
            weaponParent.localRotation = Quaternion.Slerp(weaponParent.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}