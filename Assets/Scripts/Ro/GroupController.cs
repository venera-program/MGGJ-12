using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class GroupController : MonoBehaviour{

    private Group _currGroup;
    private bool startSpawning = false;
    void Awake(){

    }

    void Update(){
        if(startSpawning){
            StartSpawning(_currGroup);
        }
    }

    public void StartGroup(Group currGroup){
        _currGroup = currGroup;
        startSpawning = true;
    }

    private void StartSpawning(Group currGroup){
        switch(currGroup.pattern){
            case GroupType.Ring:
                SpawnRing(currGroup);
                break;
            case GroupType.Spread:
                SpawnSpread(currGroup);
                break;
            case GroupType.Stack:
                SpawnStack(currGroup);
                break;
            default:
                break;
        }
    }

    private void SpawnRing(Group ring){
        for (int i = 0 ; i < ring.projectileCount; i++){
            float angle = HelperFunctions.CalculateProjectileAngle(i, ring, transform.position);
            float xPos = Mathf.Cos(angle);
            float yPos = Mathf.Sin(angle);
            Vector2 projectilePosition = new Vector2((xPos * ring.radius) + transform.position.x, 
                                (yPos * ring.radius) + transform.position.y);
            GameObject projectile = Instantiate(ring.projectile, projectilePosition, Quaternion.identity, transform );
            Projectile proj = projectile.GetComponent<Projectile>();
            proj.ConstructProjectile(ring.speed, angle * Mathf.Rad2Deg);
        }
    }

    private void SpawnSpread(Group spread){

    }

    private void SpawnStack(Group stack){

    }

   
}


public enum MovementAngle {
    Fixed, 
    TowardsPlayer,
    Random
}

public enum GroupType {
    Ring,
    Spread,
    Stack
}