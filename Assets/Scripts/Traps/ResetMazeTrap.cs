using Containers;
using Interfaces;
using UnityEngine;

namespace Traps
{
    public class ResetMazeTrap : MonoBehaviour, ITrappable
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
            MazeGenerator.InvokeRegenerateMaze(false);
            GameManager.Instance.RespawnPlayer();
        }
    }
}