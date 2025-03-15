using System;
using Containers;
using Interfaces;
using UnityEngine;

namespace Traps
{
    public class ResetPlayerPositionTrap : MonoBehaviour, ITrappable
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TagsContainer.PLAYER))
            {
                Activate();
            }
        }

        public void Activate()
        {
            GameManager.Instance.InvokeRespawnPlayer();
        }
    }
}