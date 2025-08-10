using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class GroupController : MonoBehaviour{

    private Group[] groups;
    private float[] spawnedThisSecond;
    private float spawnTiming;
    public float timer; 
    private bool startSpawning = false;
    void Update(){

        if(startSpawning){
            timer  += Time.deltaTime;
            for(int i = 0 ; i < groups.Length ; i++){
                // Floors timer so that modulo actually works with the time given 
                // And then checks to see if the wave has spawned already since time.deltatime only increases the timer with
                // fractions of a second, necessatiting a check. 
                
                float timePastDelay = timer - groups[i].delay;
                float timePastDelayInt = Mathf.Floor(resetFloat);
                bool isTimeToGenerate = false;
                if(spawnedThisSecond[i] == timePastDelayInt){
                    continue;
                } else {
                    isTimeToGenerate =  timePastDelayInt % groups[i].spawnInterval == 0;
                }

                if(isTimeToGenerate){ 
                    spawnedThisSecond[i] = timePastDelayInt;
                    StartSpawning(groups[i]);
                }
            }
        }
    }

    public void StartGroup(Group[] currGroup){
        groups = currGroup;
        spawnedThisSecond = new float[groups.Length];
        startSpawning = true;
    }

    private void StartSpawning(Group currGroup){
        switch(currGroup.pattern){
            case GroupType.Ring:
                SpawnRing(currGroup);
                break;
            case GroupType.Spread:
                SpawnRing(currGroup);
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

    private void SpawnStack(Group stack){
        for (int i = 0; i < stack.projectileCount ; i++){
            float positionAngle = HelperFunctions.CalculateProjectilePositionAngle(i, stack);
            float xPos = Mathf.Cos(positionAngle);
            float yPos = Mathf.Sin(positionAngle);
            Vector2 projectilePosition = new Vector2((xPos * stack.radius) + transform.position.x + stack.offset.x, 
                (yPos * stack.radius) + transform.position.y + stack.offset.y);
            GameObject projectile = Instantiate(stack.projectile, projectilePosition, Quaternion.identity, transform);
            Projectile proj = projectile.GetComponent<Projectile>();
            float angle = HelperFunctions.CaluclateProjectileMovementAngle(i, stack, projectilePosition);
            proj.ConstructProjectile(stack.speed + (stack.speed * stack.speedMultiplier * (i+1)), angle);
        }
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