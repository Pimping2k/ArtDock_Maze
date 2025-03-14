using System;
using UnityEngine;

namespace PlayerScripts
{
    public class Health : MonoBehaviour
    {
        private Transform spawnPoint;

        public void Die()
        {
            //Make screen fade
            transform.position = GameManager.Instance.SpawnPoint.position;
        }
    }
}