using System;
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
        [SerializeField] private GameObject sword;
        [SerializeField] private GameObject gun;

        private GameObject playerInstance;
        private GameObject enemyInstance;
        private Animator _animator;

        private Health enemyHealth;

        private void Start()
        {
            InputManager.Instance.InputActions.Player.Shoot.performed += OnShootPerformed;
            playerInstance = transform.parent.gameObject;
            _animator = GetComponent<Animator>();
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
            if (other.CompareTag(TagsContainer.ENEMY) && enemyInstance == null)
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
                enemyInstance = null;
                enemyHealth = null;
            }
        }

        private void PerformKillAnimation()
        {
            sword.gameObject.SetActive(true);
            gun.gameObject.SetActive(false);

            StartCoroutine(MoveToEnemy(enemyInstance.transform.position));
            InputManager.Instance.InputActions.Player.Disable();
        }

        private IEnumerator MoveToEnemy(Vector3 target)
        {
            while (Vector3.Distance(playerInstance.transform.position, target) > 0.4f)
            {
                playerInstance.transform.position = Vector3.MoveTowards(playerInstance.transform.position, target, .1f);
                Vector3 direction = enemyInstance.transform.position - playerInstance.transform.position;
                playerInstance.transform.forward = direction;
                playerInstance.transform.rotation = Quaternion.LookRotation(direction);
                yield return null;
            }

            _animator.SetTrigger(AnimatorTagsContainer.FINISHING);
            enemyHealth.IsDead = true;
        }

        private void KillEnemy()
        {
            if (enemyInstance != null)
            {
                enemyHealth.Die();
            }
        }
        
        private void GetGun()
        {
            sword.gameObject.SetActive(false);
            gun.gameObject.SetActive(true);
        }

        private void TurnInput()
        {
            InputManager.Instance.InputActions.Player.Enable();
            playerInstance.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        private void OnDestroy()
        {
            InputManager.Instance.InputActions.Player.Shoot.performed -= OnShootPerformed;
        }
    }
}