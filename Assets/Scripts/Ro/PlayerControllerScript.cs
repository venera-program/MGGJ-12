using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControllerScript : MonoBehaviour
{
   public PlayerController controller;
   public static PlayerControllerScript instance;
   private Vector2 direction = Vector2.zero;
   private Rigidbody2D rb; 
   [Range(0f,30f)]
   public float speed = 5;
   
   void Awake(){
        if(instance != null && instance != this){
            Destroy(this);
        } else {
            instance = this;
        }

        controller = new PlayerController();
        rb = GetComponent<Rigidbody2D>();
        controller.Enable();
   }

   void FixedUpdate(){
        direction = controller.Main.Move.ReadValue<Vector2>();
        Move(direction);
   }    

   private void Move(Vector2 direction){
        rb.MovePosition((direction * speed * Time.fixedDeltaTime) + (Vector2)transform.position );
   }
}
