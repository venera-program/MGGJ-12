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
            float positionAngle = HelperFunctions.CalculateProjectilePositionAngle(i, ring);
            float xPos = Mathf.Cos(positionAngle);
            float yPos = Mathf.Sin(positionAngle);
            Vector2 projectilePosition = new Vector2((xPos * ring.radius) + transform.position.x + ring.offset.x, 
                                (yPos * ring.radius) + transform.position.y + ring.offset.y);
            GameObject projectile = Instantiate(ring.projectile, projectilePosition, Quaternion.identity, transform );
            Projectile proj = projectile.GetComponent<Projectile>();
            float movementAngle = HelperFunctions.CaluclateProjectileMovementAngle(i, ring, projectilePosition);
            proj.ConstructProjectile(ring.speed, movementAngle);
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
}

[Serializable]
public enum PositionAngle{
    FixedPosition,
    RandomPosition,
}


public enum GroupType {
    Ring,
    Spread,
    Stack
}