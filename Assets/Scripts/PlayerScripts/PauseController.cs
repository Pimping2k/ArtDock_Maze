using System;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerScripts
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField] private GameObject pauseUI;

        private InputSystem_Actions _inputSystemActions;

        private bool isPaused;

        private void Start()
        {
            pauseUI.SetActive(false);
            InputManager.Instance.InputActions.PlayerUI.Pause.performed += OnPausePerformed;
        }

        private void OnPausePerformed(InputAction.CallbackContext obj) => Pause();

        private void Pause()
        {
            var playerInput = InputManager.Instance.InputActions.Player;
            isPaused = !isPaused;

            pauseUI.SetActive(isPaused);

            if (isPaused)
                playerInput.Disable();
            else
                playerInput.Enable();
            
            Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isPaused;
        }

        public void ForceUnpause()
        {
            isPaused = false;
            pauseUI.SetActive(false);
    
            InputManager.Instance.InputActions.Player.Enable();
    
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        private void OnDestroy()
        {
            InputManager.Instance.InputActions.PlayerUI.Pause.performed -= OnPausePerformed;
        }
    }
}