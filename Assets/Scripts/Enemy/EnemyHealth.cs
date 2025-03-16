using Interfaces;
using UnityEngine;

namespace Enemy
{
    public class EnemyHealth : MonoBehaviour, IDeadable
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
            _animator = GetComponentInChildren<Animator>();
            _collider = GetComponent<Collider>();
        }

        private void Start()
        {
            enemyPool = GameManager.Instance.EnemyPoolInstance.GetComponent<CustomObjectPool>();
        }

        private void OnEnable()
        {
            HandleComponentsState(true);
            isDead = false;
        }

        public void Die()
        {
            HandleComponentsState(false);
            Invoke(nameof(DestroyEnemy), 5f);
        }

        private void HandleComponentsState(bool state)
        {
            _collider.enabled = state;
            _animator.enabled = state;
        }

        private void DestroyEnemy()
        {
            enemyPool.ReturnToPool(gameObject);
        }
    }
}