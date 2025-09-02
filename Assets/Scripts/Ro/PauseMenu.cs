using UnityEngine;
using System;
using System.Collections;

public class PauseMenu : MonoBehaviour {
    public static PauseMenu instance;
    
    public void Awake(){
        if(instance != null && instance != this){
            Destroy(gameObject);
        } else {
            instance = this;
        }
    }

    public void OnEnable(){
        Time.timeScale = 0f;
        PlayerControllerScript.instance.DisablePlayerControls();
    }

    public void OnDisable(){
        Time.timeScale = 1f;
        PlayerControllerScript.instance.EnablePlayerControls();
    }

    public void RestartLevel(){
        
    }
}