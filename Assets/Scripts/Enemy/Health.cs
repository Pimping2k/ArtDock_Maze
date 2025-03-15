using System;
using Interfaces;
using UnityEngine;

namespace Enemy
{
    public class Health : MonoBehaviour, IDeadable
    {
        [SerializeField] private bool isDead;
        private Animator _animator;
        public bool IsDead
        {
            get => isDead;
            set => isDead = value;
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            
        }

        private void OnEnable()
        {
            _animator.enabled = true;
            isDead = false;
        }

        public void Die()
        {
            _animator.enabled = false;
        }
    }
}