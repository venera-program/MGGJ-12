using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class GroupController : MonoBehaviour{

    private Group[] groups;
    private float[] spawnedThisSecond;
    public float timer;
    private bool startSpawning = false;
    public int decimalPlaces = 2;
    

    void Update(){
    
        if(startSpawning){
           timer += Time.deltaTime; 
           for (int i = 0 ; i < groups.Length ; i++){
                float truncTime = HelperFunctions.RoundToDecimal(timer, decimalPlaces); // be able to divide to spawn 
                if(truncTime != spawnedThisSecond[i]){ // statement used so that spawning doesn't happen multiple times per parts of a second
                bool isDivisible = ((truncTime - groups[i].delay) % groups[i].spawnInterval) == 0;
                if(Mathf.Approximately(truncTime, groups[i].delay)){
                    StartSpawning(groups[i]);
                    spawnedThisSecond[i] = truncTime;
                } else if (isDivisible){
                    StartSpawning(groups[i]);
                    spawnedThisSecond[i] = truncTime;
                }
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
            GameObject projectile;
            if(ring.movementAngle == MovementAngle.Fixed){
                projectile = Instantiate(ProjectileResources.instance.unDirected, projectilePosition, Quaternion.identity, ProjectilePool.instance.transform);
            } else if (ring.movementAngle == MovementAngle.TowardsPlayer){
                projectile = Instantiate(ProjectileResources.instance.directed, projectilePosition, Quaternion.identity, ProjectilePool.instance.transform);
            } else {
                Debug.LogError("How did you get here?");
                projectile = Instantiate(ProjectileResources.instance.defaultProjectile, projectilePosition, Quaternion.identity, ProjectilePool.instance.transform);
            }
            
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

            GameObject projectile;
            if(stack.movementAngle == MovementAngle.Fixed){
                projectile = Instantiate(ProjectileResources.instance.unDirected, projectilePosition, Quaternion.identity, ProjectilePool.instance.transform);
            } else if (stack.movementAngle == MovementAngle.TowardsPlayer){
                projectile = Instantiate(ProjectileResources.instance.directed, projectilePosition, Quaternion.identity, ProjectilePool.instance.transform);
            } else {
                Debug.LogError("How did you get here?");
                projectile = Instantiate(ProjectileResources.instance.defaultProjectile, projectilePosition, Quaternion.identity, ProjectilePool.instance.transform);
            }
            Projectile proj = projectile.GetComponent<Projectile>();
            // projectile.GetComponent<SpriteRenderer>().sprite = ProjectileResources.instance.Directed;
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