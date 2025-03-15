using Interfaces;
using UnityEngine;

namespace Enemy
{
    public class Health : MonoBehaviour, IDeadable
    {
        [SerializeField] private bool isDead;

        public bool IsDead
        {
            get => isDead;
            set => isDead = value;
        }

        public void Die()
        {
            GetComponent<Animator>().enabled = false;
        }
    }
}