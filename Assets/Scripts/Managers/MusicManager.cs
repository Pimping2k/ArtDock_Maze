using UnityEngine;

namespace Managers
{
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager Instance;
        
        [SerializeField] private AudioSource _audioSource;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            _audioSource.Play();
        }

        public void MuteMusic() => _audioSource.mute = true;
        public void UnMuteMusic() => _audioSource.mute = false;
    }
}