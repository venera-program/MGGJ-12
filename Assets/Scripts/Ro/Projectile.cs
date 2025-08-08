using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Projectile : MonoBehaviour
{
    public bool isMoving = false;
    public float speed = 0f;
    public float movementAngle = 0f;

    public void Update(){
        if(isMoving){
            Move(speed);
            CheckIfOutOfBounds();
        }
    }
    public void ConstructProjectile(float speed, float movementAngle){
        this.speed = speed;
        transform.Rotate(new Vector3(0f,0f,movementAngle));
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
            DestroySelf();
        }
    }

    private void DestroySelf(){
        Destroy(gameObject);
    }
}

