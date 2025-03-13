using System;
using Containers;
using Interfaces;
using PlayerScripts;
using UnityEngine;

namespace Traps
{
    public class ResetMazeTrap : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TagsContainer.PLAYER))
            {
                if (other.TryGetComponent<Health>(out var playerHealth))
                {
                    MazeGenerator.InvokeRegenerateMaze();
                    playerHealth.Die();
                }
            }
        }
    }
}