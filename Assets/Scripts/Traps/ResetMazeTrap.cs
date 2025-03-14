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
                if (regenerateMaze)
                    MazeGenerator.InvokeRegenerateMaze();
                
                other.transform.position = GameManager.Instance.SpawnPoint.position;
            }
        }
    }
}