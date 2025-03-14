using Containers;
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
                    MazeGenerator.InvokeRegenerateMaze(true);

                GameManager.Instance.InvokeRespawnPlayer();
            }
        }
    }
}