using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Projectile : MonoBehaviour
{
    public bool isMoving = false;
    public float speed = 0f;
    public float movementAngle = 0f;
    public ProjectileType projectileType = ProjectileType.def;
    public Transform image;

    public void Update(){
        if(isMoving){
            Move(speed);
            CheckIfOutOfBounds();
        }
    }
    public void ConstructProjectile(float speed, float movementAngle){
        this.speed = speed;
        transform.rotation = Quaternion.identity;
        image.rotation = Quaternion.identity;
        transform.Rotate(new Vector3(0f,0f,movementAngle), Space.World);
        if(projectileType != ProjectileType.angle){ 
            // this is done because all the sprite art face 90 degrees by default, but the angle sprite faces 0
             image.Rotate(new Vector3(0f, 0f, -90f), Space.Self);
        }
        
        StartMoving();
    }
    
    private void StartMoving(){
        isMoving = true;
    }

    public void Move(float movementSpeed){
        transform.position = (transform.right * movementSpeed * Time.deltaTime) + transform.position;
    }

    private void CheckIfOutOfBounds(){
        if(!HelperFunctions.IsOnScreen(transform.position)){
            isMoving = false;
            ProjectilePool.instance.DeactivateProjectile(this);
        }
    }
}

public enum ProjectileSprite {
    Directed, 
    NonDirected
}

