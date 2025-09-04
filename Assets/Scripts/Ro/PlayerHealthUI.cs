using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerHealthUI : MonoBehaviour {
    [SerializeField] private List<GameObject> Hearts = new List<GameObject>();
    private Health playerHealth;
    
    public void OnEnable() {
        playerHealth = PlayerControllerScript.instance.GetComponent<Health>();
        playerHealth.healthChange.AddListener(UpdateHealthUI);
        UpdateHealthUI(playerHealth.GetCurrHealth(), playerHealth.GetMaxHealth());
    }

    public void OnDisable(){
        playerHealth.healthChange.RemoveListener(UpdateHealthUI);
    }

    private void UpdateHealthUI(float currHealth, float maxHealth){
        float difference = maxHealth - currHealth;
            for(int i = 0 ; i < Hearts.Count; i++){
                if(i <= difference - 1){
                    Hearts[i].SetActive(false);
                } else {
                    Hearts[i].SetActive(true);
                }
                
            }
    }
}