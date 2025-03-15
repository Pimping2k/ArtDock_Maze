using UnityEngine;

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
        
        [Header("References")] 
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Transform lowerBodyParent;
        [SerializeField] private Transform lowerBody;
        [SerializeField] private Transform upperBodyParent;
        [SerializeField] private Transform upperBody;
        [SerializeField] private Transform weaponParent;
        [SerializeField] private Transform lScapula;
        [SerializeField] private Transform rScapula;
        
        private Vector2 moveInput;
        private InputSystem_Actions _inputSystemActions;
        private Vector2 lookInput;
        private float currentXRotation;
        private float currentZRotation;

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
            currentZRotation -= mouseDelta.y;
            
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
    }
}