using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour{
   public static Score instance;
   public float score = 0f;

   void Awake(){
        if(instance != this && instance != null){
            Destroy(gameObject);
        } else {
            instance = this;
        }
   }
    public void UpdateScore(float additionalAmount){
        score += additionalAmount;
        Debug.Log($"New Score: {score}");
    }
}
