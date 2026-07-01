using UnityEngine;

using RPGTest.Architecture;

namespace RPGTest.Core.Managers 
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Audio Sources")]
        public AudioSource bgmSource;
        public AudioSource sfxSource;

        private void Awake()
        {
            if (!ServiceLocator.Current.Contains<AudioManager>())
            {
                ServiceLocator.Current.Register<AudioManager>(this);
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PlayBGM(AudioClip clip)
        {
            if (clip == null) return;

            if (bgmSource.clip == clip) return;

            bgmSource.clip = clip;
            bgmSource.Play();
        }

        public void PlaySFX(AudioClip clip)
        {
            if (clip != null)
            {
                sfxSource.PlayOneShot(clip);
            }
        }
    }
}