using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour {

    public float score;  
    private Health health;
    void Awake(){
        health = GetComponent<Health>();
        health.healthChange.AddListener(OnDeath);
    }
    public void OnDeath(float currHealth, float maxHealth){
        if (currHealth <= 0){
            Score.instance.UpdateScore(score);
            Destroy(gameObject);
        }
    }


}