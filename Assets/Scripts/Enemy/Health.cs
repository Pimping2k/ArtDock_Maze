using System;
using Interfaces;
using UI;
using UnityEngine;

namespace Enemy
{
    public class Health : MonoBehaviour, IDeadable
    {
        [SerializeField] private bool isDead;

        private Animator _animator;
        private CustomObjectPool enemyPool;
        private Collider _collider;

        public bool IsDead
        {
            get => isDead;
            set => isDead = value;
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider>();
        }

        private void Start()
        {
            enemyPool = GameManager.Instance.EnemyPoolInstance.GetComponent<CustomObjectPool>();
        }

        private void OnEnable()
        {
            _animator.enabled = true;
            _collider.enabled = true;
            isDead = false;
        }

        public void Die()
        {
            _collider.enabled = false;
            _animator.enabled = false;
            Invoke(nameof(DestroyEnemy), 5f);
        }

        private void DestroyEnemy()
        {
            enemyPool.ReturnToPool(gameObject.transform.parent.gameObject);

            KillEnemyUIController.InvokeOnStateChanged(false);
        }
    }
}