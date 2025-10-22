using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {
    public static EnemyManager instance;

    void Awake(){
        if(instance != this && instance != null){
            Destroy(gameObject);
        } else {
            instance = this;
        }
    }

    public void StartEnemyMovement(){
        EnemyMotor[] enemyMotors = FindObjectsByType<EnemyMotor>(FindObjectsSortMode.None);
        foreach(EnemyMotor A in enemyMotors){
            A.StartMoving();
        }
        EnemyAnimation[] enemyAnims = FindObjectsByType<EnemyAnimation>(FindObjectsSortMode.None);
        foreach(EnemyAnimation A in enemyAnims){
            A._animator.enabled = true;
        }
    }

    public void StopEnemyMovement(){
        EnemyMotor[] list = FindObjectsByType<EnemyMotor>(FindObjectsSortMode.None);
        foreach(EnemyMotor A in list){
            A.StopMoving();
        }
        EnemyAnimation[] enemyAnims = FindObjectsByType<EnemyAnimation>(FindObjectsSortMode.None);
        foreach(EnemyAnimation A in enemyAnims){
            A._animator.enabled = false;
        }
    }
    public void StartEnemyProjectileSpawning(){
        GroupController[] list = FindObjectsByType<GroupController>(FindObjectsSortMode.None);
        foreach(GroupController A in list){
            A.UnpauseSpawning();
        }
    }
    public void StopEnemyProjectileSpawning(){
        GroupController[] list = FindObjectsByType<GroupController>(FindObjectsSortMode.None);
        foreach(GroupController A in list){
            A.StopSpawning();
        }
    }
}