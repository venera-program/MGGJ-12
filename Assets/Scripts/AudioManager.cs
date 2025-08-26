using UnityEngine;
using System.Collections.Generic;

namespace MGGJ25.Shared
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [SerializeField] private AudioSource musicAudioSource;
        [SerializeField] private AudioSource sfxAudioSource;
        [SerializeField] private GameObject audioSourcePrefab; // Prefab with an AudioSource component
        private List<AudioSource> sfxAudioSources = new List<AudioSource>();
        private Queue<AudioSource> availableSfxSources = new Queue<AudioSource>();

        [Header("Pool Settings")]
        [SerializeField] private int initialPoolSize = 3; // Number of SFX sources to preallocate

        [Header("Music Clips")]
        [SerializeField] private AudioClip levelMusicClip;

        [Header("SFX Clips")]
        [SerializeField] private AudioClip playerbulletSound;
        [SerializeField] private AudioClip playerdiesSound;
        [SerializeField] private AudioClip enemybulletSound;
        [SerializeField] private AudioClip enemydiesSound;

        protected void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.Log("Destroying Duplicate AudioManager.", gameObject);
                Destroy(this);
                return;
            }

            InitializeAudioSourcePool();
        }

        private void InitializeAudioSourcePool()
        {
            for (int i = 0; i < initialPoolSize; i++)
            {
                CreateNewAudioSource();
            }
        }

        private void CreateNewAudioSource()
        {
            GameObject audioSourceObj = Instantiate(audioSourcePrefab, AudioManager.Instance.transform);
            audioSourceObj.SetActive(true);
            AudioSource audioSource = audioSourceObj.GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
            sfxAudioSources.Add(audioSource);
            availableSfxSources.Enqueue(audioSource);
        }

        private AudioSource GetAvailableAudioSource()
        {
            if (availableSfxSources.Count == 0)
            {
                CreateNewAudioSource();
            }

            AudioSource source = availableSfxSources.Dequeue();
            return source;
        }

        private void ReturnAudioSourceToPool(AudioSource source)
        {
            source.Stop();
            source.clip = null;
            availableSfxSources.Enqueue(source);
        }

        public void PlaySfx(AudioClip clip)
        {
            foreach (var source in sfxAudioSources)
            {
                if (source.clip == clip) return;
            }

            AudioSource audioSource = GetAvailableAudioSource();
            audioSource.clip = clip;
            audioSource.Play();
            StartCoroutine(ReturnToPoolAfterPlayback(audioSource));
        }

        private System.Collections.IEnumerator ReturnToPoolAfterPlayback(AudioSource source)
        {
            yield return new WaitUntil(() => !source.isPlaying);
            ReturnAudioSourceToPool(source);
        }

        public void StopAllSfx()
        {
            foreach (var source in sfxAudioSources)
            {
                source.Stop();
            }
        }

        public void PlayMusic(AudioClip clip)
        {
            if (musicAudioSource.clip == clip) return;

            musicAudioSource.clip = clip;
            musicAudioSource.Play();
        }

        public void StopMusic()
        {
            musicAudioSource.Stop();
        }

        #region Music Sounds
        public void PlayLevel_Music() => PlayMusic(levelMusicClip);
        #endregion

        #region SFX Sounds
        public void PlayPlayerBullet_SFX() => PlaySfx(playerbulletSound);
        public void PlayPlayerDies_SFX() => PlaySfx(playerdiesSound);
        public void PlayEnemyBullet_SFX() => PlaySfx(enemybulletSound);
        public void PlayEnemyDies_SFX() => PlaySfx(enemydiesSound);
        #endregion
    }
}
