using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MGGJ25.Shared;

public class VolumeControl : MonoBehaviour 
{
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private AudioMixer mixer;

    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    
    void Start()
    {
        float value;

        if (mixer.GetFloat("MusicVolume", out value))
            musicVolumeSlider.value = Mathf.Pow(10f, value / 20f);

        if (mixer.GetFloat("SFXVolume", out value))
            sfxVolumeSlider.value = Mathf.Pow(10f, value / 20f);

    }

    public void ChangeMusicVolume(Slider sliderValue)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(sliderValue.value, 0.0001f, 1f)) * 20);
    }

    public void ChangeSFXVolume(Slider sliderValue)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(sliderValue.value, 0.0001f, 1f)) * 20);
        AudioManager.Instance.PlayPlayerGrazeFull_SFX();
    }

}