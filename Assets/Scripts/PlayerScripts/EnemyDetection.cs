using Containers;
using Managers;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerScripts
{
    public class EnemyDetection : MonoBehaviour
    {
        [SerializeField] private float enemyDestroyTime;

        private GameObject enemyInstance;

        private void Start()
        {
            InputManager.Instance.InputActions.Player.Shoot.performed += OnShootPerformed;
        }

        private void OnShootPerformed(InputAction.CallbackContext obj)
        {
            KillEnemy();
            Invoke(nameof(DestroyEnemy), enemyDestroyTime);
            KillEnemyUIController.InvokeOnStateChanged(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TagsContainer.ENEMY))
            {
                enemyInstance = other.gameObject;
                KillEnemyUIController.InvokeOnStateChanged(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(TagsContainer.ENEMY))
            {
                enemyInstance = null;
                KillEnemyUIController.InvokeOnStateChanged(false);
            }
        }

        private void KillEnemy()
        {
            if (enemyInstance != null)
                enemyInstance.GetComponent<Animator>().enabled = false;
        }

        private void DestroyEnemy()
        {
            var enemyPool = GameManager.Instance.EnemyPoolInstance.GetComponent<CustomObjectPool>();
            enemyPool.ReturnToPool(enemyInstance);
        }
    }
}