using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool instance;

    void Awake(){
        if (instance != null && instance != this){
            Destroy(gameObject);
        } else {
            instance = this;
        }
    }
}
