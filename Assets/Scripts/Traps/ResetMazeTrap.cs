using System;
using Containers;
using Interfaces;
using PlayerScripts;
using UnityEngine;

namespace Traps
{
    public class ResetMazeTrap : MonoBehaviour
    {
        [SerializeField] private bool regenerateMaze;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TagsContainer.PLAYER))
            {
                if (other.TryGetComponent<Health>(out var playerHealth))
                {
                    if (regenerateMaze)
                        MazeGenerator.InvokeRegenerateMaze();
                    
                    playerHealth.Die();
                }
            }
        }
    }
}