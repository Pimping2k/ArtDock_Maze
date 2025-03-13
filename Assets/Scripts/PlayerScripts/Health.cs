using UnityEngine;

namespace PlayerScripts
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;
        
        public void Die()
        {
            //Make screen fade
            transform.position = spawnPoint.position;
        }
    }
}