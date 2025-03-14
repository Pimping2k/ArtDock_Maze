using System;
using UnityEngine;

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
            _inputSystemActions = new InputSystem_Actions();
            _inputSystemActions.Enable();
            _inputSystemActions.Player.Pause.performed += ctx => Pause();
        }

        private void Pause()
        {
            isPaused = !isPaused;
            
            pauseUI.SetActive(isPaused);
            Time.timeScale = isPaused ? 0f : 1f;
            Cursor.lockState = isPaused? CursorLockMode.None : CursorLockMode.Locked ;
            Cursor.visible = isPaused;
        }

        private void OnDestroy()
        {
            _inputSystemActions.Disable();
        }
    }
}