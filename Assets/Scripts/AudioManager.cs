using UnityEngine;
using System.Collections.Generic;

namespace MGGJ25.Shared
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [SerializeField] private AudioSource musicAudioSource;
        [SerializeField] private AudioSource sfxAudioSource;
        [SerializeField] private AudioSource sfxLoopAudioSource;
        [SerializeField] private GameObject audioSourcePrefab; // Prefab with an AudioSource component
        private List<AudioSource> sfxAudioSources = new List<AudioSource>();
        private Queue<AudioSource> availableSfxSources = new Queue<AudioSource>();

        [Header("Pool Settings")]
        [SerializeField] private int initialPoolSize = 3; // Number of SFX sources to preallocate

        [Header("Music Clips")]
        [SerializeField] private AudioClip level1MusicClip;
        [SerializeField] private AudioClip level2MusicClip;
        [SerializeField] private AudioClip level3MusicClip;


        [Header("SFX Clips")]
        [SerializeField] private AudioClip playerbulletSound;
        [SerializeField] private AudioClip playerdiesSound;
        [SerializeField] private AudioClip playerspecialSound;
        [SerializeField] private AudioClip playergrazeSound;
        [SerializeField] private AudioClip playergrazefullSound;
        [SerializeField] private AudioClip playeremptySound;
        [SerializeField] private AudioClip enemybulletSound;
        [SerializeField] private AudioClip enemydiesSound;
        [SerializeField] private AudioClip uiselectSound;
        [SerializeField] private AudioClip uiconfirmSound;

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

            if(clip != playerbulletSound && clip != playerspecialSound){
                AudioSource audioSource = GetAvailableAudioSource();
                audioSource.clip = clip;
                audioSource.Play();
                StartCoroutine(ReturnToPoolAfterPlayback(audioSource));
            } else {
                sfxLoopAudioSource.clip = clip;
                sfxLoopAudioSource.Play();
            }
            
        }

        public void StopSfx(AudioClip clip){

            if(clip == playerbulletSound || clip == playerspecialSound){
                sfxLoopAudioSource.Stop();
                return;
            }

            foreach (var source in sfxAudioSources){
                if(source.clip == clip){
                    source.Stop();
                }
            }
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

        public void SetMusicVolume(float volume){
            musicAudioSource.volume = volume;
        }

        public void SetSFXVolume(float volume){
            foreach (AudioSource source in sfxAudioSources){
                source.volume = volume;
            }

            sfxLoopAudioSource.volume = volume;
        }

        #region Music Sounds
        public void PlayLevel1_Music() => PlayMusic(level1MusicClip);
        public void PlayLevel2_Music() => PlayMusic(level2MusicClip);
        public void PlayLevel3_Music() => PlayMusic(level3MusicClip);
        #endregion

        #region SFX Sounds
        public void PlayPlayerBullet_SFX() => PlaySfx(playerbulletSound);
        public void PlayPlayerDies_SFX() => PlaySfx(playerdiesSound);
        public void PlayPlayerSpecial_SFX() => PlaySfx(playerspecialSound);
        public void PlayPlayerGraze_SFX() => PlaySfx(playergrazeSound);
        public void PlayPlayerGrazeFull_SFX() => PlaySfx(playergrazefullSound);
        public void PlayPlayerEmpty_SFX() => PlaySfx(playeremptySound);     
        public void PlayEnemyBullet_SFX() => PlaySfx(enemybulletSound);
        public void PlayEnemyDies_SFX() => PlaySfx(enemydiesSound);
        public void PlayUISelect_SFX() => PlaySfx(uiselectSound);
        public void PlayUIConfirm_SFX() => PlaySfx(uiconfirmSound);

        #endregion

        #region Stop SFX Sounds
        public void StopPlayerBullet_SFX() => StopSfx(playerbulletSound);
        public void StopPlayerSpecial_SFX() => StopSfx(playerspecialSound);
        #endregion
    }
}
