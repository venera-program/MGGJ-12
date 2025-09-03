using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using MGGJ25.Shared;

public class VolumeControl : MonoBehaviour {
    [SerializeField] private Slider SFXSlider;
    [SerializeField][Range(0,1)] private float defaultSFXVolume = .5f;
    [SerializeField] private Slider musicSlider;
    [SerializeField][Range(0,1)] private float defaultMusicVolume = .5f;
    void Start(){
        SFXSlider.onValueChanged.AddListener(ChangeSFXVolume);
        SFXSlider.value = defaultSFXVolume;
        musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
        musicSlider.value = defaultMusicVolume;
    }

    
    private void ChangeSFXVolume(float value){
        AudioManager.Instance.SetSFXVolume(value);
    }

    private void ChangeMusicVolume(float value){
        AudioManager.Instance.SetMusicVolume(value);
    }

}