using UnityEngine;

namespace ArcaneWords.Scripts.Game.GameRoot
{
    [RequireComponent(typeof(AudioSource))]
    public class GameAudioHandler : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound(AudioClip clip)
        {
            _audioSource.PlayOneShot(clip);
        }

        private void OnDisable()
        {
            _audioSource.Stop();
        }
    }
}