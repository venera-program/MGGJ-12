using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControllerScript : MonoBehaviour
{
   public PlayerController controller;
   public static PlayerControllerScript instance;
   private Vector2 direction = Vector2.zero;
   private Rigidbody2D rb; 
   [Range(0f,30f)]
   public float speed = 5;

   public Group[] shootingPattern;
   public float maxSpeed;
   public float minSpeed;
   public float accel;
   public float deccel;

   private float projectileTimer = 0f;

   [SerializeField]
   private float projectileSpawnInterval;
   private bool startProjectSpawnTimer = false;
   private bool startGeneratingProject = false;

   
   void Awake(){
        if(instance != null && instance != this){
            Destroy(this);
        } else {
            instance = this;
        }

        controller = new PlayerController();
        rb = GetComponent<Rigidbody2D>();
        controller.Enable();
        controller.Main.Shoot.started += Shoot;
        controller.Main.Shoot.canceled += StopShoot;
        projectileTimer = projectileSpawnInterval;
   }

   void OnDisable(){
     controller.Main.Shoot.started -= Shoot;
     controller.Main.Shoot.canceled -= StopShoot;
     controller.Disable();
   }

   void Update(){
          if (startProjectSpawnTimer){
               projectileTimer += Time.deltaTime;
               if (startGeneratingProject){
                    if (projectileTimer > projectileSpawnInterval){
                         projectileTimer = 0f;
                         GeneratePlayerProjectiles();
                    }
               }
          }   
   }

   void FixedUpdate(){
        direction = controller.Main.Move.ReadValue<Vector2>();
        Move(direction);
   }    

   private void Move(Vector2 direction){
          float velocity = 0f;
          if (direction == Vector2.zero){
               velocity = Mathf.Clamp(speed - Time.fixedDeltaTime * deccel, minSpeed, maxSpeed); 
          } else {
               velocity = Mathf.Clamp(Time.fixedDeltaTime * accel + speed, minSpeed, maxSpeed); 
          }
        rb.MovePosition((direction * velocity * Time.fixedDeltaTime) + (Vector2)transform.position );
   }

   private void Shoot(UnityEngine.InputSystem.InputAction.CallbackContext cont){
     startProjectSpawnTimer = true;
     if (cont.interaction is HoldInteraction) {
          startGeneratingProject = true;
     }
   }

   private void StopShoot(UnityEngine.InputSystem.InputAction.CallbackContext cont){
     if(cont.interaction is HoldInteraction){
          startGeneratingProject = false;
     }
   }

   private void GeneratePlayerProjectiles(){
      for(int i = 0 ; i < shootingPattern.Length ; i++){
               float rad = shootingPattern[i].startingAngle * Mathf.Deg2Rad;
               float xPos = Mathf.Cos(rad) * shootingPattern[i].radius;
               float yPos = Mathf.Sin(rad) * shootingPattern[i].radius;
               Vector3 finalPosition = new Vector3(transform.position.x + xPos + shootingPattern[i].offset.x, 
                    transform.position.y + yPos + shootingPattern[i].offset.y, 0f);
               GameObject projectile;
               if(Mathf.Approximately(shootingPattern[i].startingAngle, 90f)){
                    projectile = ProjectilePool.instance.ActivateProjectile(ProjectileType.forward);
               } else {
                    projectile = ProjectilePool.instance.ActivateProjectile(ProjectileType.angle);
               }
               projectile.transform.position = finalPosition;
               Projectile script = projectile.GetComponent<Projectile>();
               script.ConstructProjectile(shootingPattern[i].speed, shootingPattern[i].startingAngle);
          }
   }
}
