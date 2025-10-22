using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    void Awake(){
        if(instance != this && instance != null){
            Destroy(gameObject);
        } else {
            instance = this;
        }
    }
    public void StartTick(){
        Enemy_Spawner.Instance.TurnOnTick();
    }

    public void StopTick(){
        Enemy_Spawner.Instance.TurnOffTick();
    }

    public void StartEnemy(){
        EnemyManager.instance.StartEnemyMovement();
        EnemyManager.instance.StartEnemyProjectileSpawning();
    }

    public void StopEnemy(){
        EnemyManager.instance.StopEnemyMovement();
        EnemyManager.instance.StopEnemyProjectileSpawning();
    }

    public void StartPlayerAnimation(){
        PlayerControllerScript.instance.StartAnimations();
    }
    public void StopPlayerAnimation(){
        PlayerControllerScript.instance.StopAnimations();
    }
    
    public void StartGrazeDetection(){
        Graze.instance.StartGrazeCount();
    }
    public void StopGrazeDetection(){
        Graze.instance.StopGrazeCount();
    }
    public void StartSkillTimer(){
        Graze.instance.StartSkillTimer();
    }
    public void StopSkillTimer(){
        Graze.instance.PauseSkillTimer();
    }
    public void StartProjectileMovement(){
        ProjectilePool.instance.UnpauseProjectilesMovement();
    }
    public void StopProjectileMovement(){
        ProjectilePool.instance.StopProjectilesMovement();
    }
}