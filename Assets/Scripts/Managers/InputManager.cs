using System;
using UnityEngine;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;

        private InputSystem_Actions _inputActions;

        public InputSystem_Actions InputActions
        {
            get => _inputActions;
            set => _inputActions = value;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            _inputActions = new InputSystem_Actions();
        }

        private void Start()
        {
            _inputActions.Enable();
        }
    }
}