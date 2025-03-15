using System.Collections;
using Containers;
using Enemy;
using Managers;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerScripts
{
    public class EnemyDetection : MonoBehaviour
    {
        [SerializeField] private float enemyDestroyTime;
        [SerializeField] private GameObject sword;
        [SerializeField] private GameObject gun;

        private GameObject playerInstance;
        private GameObject enemyInstance;
        private Animator _animator;
        private CustomObjectPool enemyPool;

        private Health enemyHealth;
        
        private void Start()
        {
            InputManager.Instance.InputActions.Player.Shoot.performed += OnShootPerformed;
            playerInstance = transform.parent.gameObject;
            _animator = GetComponent<Animator>();
            enemyPool = GameManager.Instance.EnemyPoolInstance.GetComponent<CustomObjectPool>();
        }

        private void OnShootPerformed(InputAction.CallbackContext obj)
        {
            if (enemyInstance != null && !enemyHealth.IsDead)
            {
                PerformKillAnimation();
            }

            KillEnemyUIController.InvokeOnStateChanged(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TagsContainer.ENEMY))
            {
                enemyInstance = other.gameObject;
                enemyHealth = enemyInstance.GetComponent<Health>();
                KillEnemyUIController.InvokeOnStateChanged(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(TagsContainer.ENEMY))
            {
                KillEnemyUIController.InvokeOnStateChanged(false);
            }
        }

        private void KillEnemy()
        {
            if (enemyInstance != null)
            {
                enemyHealth.Die();
                Invoke(nameof(DestroyEnemy), enemyDestroyTime);
            }
        }

        private void PerformKillAnimation()
        {
            sword.gameObject.SetActive(true);
            gun.gameObject.SetActive(false);

            StartCoroutine(MoveToEnemy(enemyInstance.transform.position));
            InputManager.Instance.InputActions.Player.Disable();
        }

        private IEnumerator MoveToEnemy(Vector3 targetPos)
        {
            while (Vector3.Distance(playerInstance.transform.position, targetPos) > 0.4f)
            {
                playerInstance.transform.LookAt(targetPos);
                playerInstance.transform.position = Vector3.MoveTowards(playerInstance.transform.position,targetPos,.1f);
                
                yield return null;
            }

            _animator.SetTrigger(AnimatorTagsContainer.FINISHING);
            enemyHealth.IsDead = true;
            playerInstance.transform.rotation = Quaternion.Euler(0f,0f,0f);
        }

        private void GetGun()
        {
            sword.gameObject.SetActive(false);
            gun.gameObject.SetActive(true);
        }

        private void TurnInput()
        {
            InputManager.Instance.InputActions.Player.Enable();
        }

        private void DestroyEnemy()
        {
            if (enemyInstance != null)
            {
                enemyPool.ReturnToPool(enemyInstance.transform.parent.gameObject);
                enemyInstance = null;
                enemyHealth = null;
            }

            KillEnemyUIController.InvokeOnStateChanged(false);
        }
    }
}