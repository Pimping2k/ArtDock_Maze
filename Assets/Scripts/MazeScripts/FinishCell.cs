using Containers;
using UnityEngine;

namespace MazeScripts
{
    public class FinishCell : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TagsContainer.PLAYER))
            {
                GameManager.Instance.FinishGame();
            }
        }
    }
}