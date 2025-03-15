using System;
using UnityEngine;

namespace UI
{
    public class KillEnemyUIController : MonoBehaviour
    {
        [SerializeField] private GameObject UI;

        private static event Action<bool> StateChanged;
        public static void InvokeOnStateChanged(bool state) => StateChanged.Invoke(state);

        private void Awake()
        {
            StateChanged += OnStateChanged;
        }

        private void Start()
        {
            UI.SetActive(false);
        }

        private void OnStateChanged(bool state)
        {
            UI.SetActive(state);
        }

        private void OnDestroy()
        {
            StateChanged -= OnStateChanged;
        }
    }
}