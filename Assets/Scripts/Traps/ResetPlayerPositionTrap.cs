using System;
using Containers;
using Interfaces;
using PlayerScripts;
using UnityEngine;

namespace Traps
{
    public class ResetPlayerPositionTrap : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TagsContainer.PLAYER))
            {
                if (other.TryGetComponent<Health>(out var playerHealth))
                {
                    playerHealth.Die();
                }
            }
        }
    }
}