using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

public class YouWin : MonoBehaviour {
    [SerializeField] private GameObject youWinScreen;

    void Start(){
        youWinScreen.SetActive(false);
    }

    void OnEnable(){
        LevelManager.OnGameWin += ActivateWinScreen;
    }
    void OnDisable()
    {
        LevelManager.OnGameWin -= ActivateWinScreen;
    }

    private void ActivateWinScreen(){
        Debug.Log("Activate Win Screen");
        youWinScreen.SetActive(true);
    }
}