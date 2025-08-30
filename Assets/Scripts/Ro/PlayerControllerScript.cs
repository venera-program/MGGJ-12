using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;
using MGGJ25.Shared;

[RequireComponent(typeof(Rigidbody2D), typeof(Health), typeof(Collider2D))]
public class PlayerControllerScript : MonoBehaviour
{
     private const float RESPAWN_DELAY = 0.5f;
     private const int I_FRAMES = 120;
     private const string IS_MOVING = "isMoving";
     private const string WAS_HIT = "wasHit";
     private bool flipped;

    public Transform spawnPoint;

     public static PlayerControllerScript instance;

     public Vector2 PlayerInputRaw { get => direction; set => direction = value; }

     public PlayerController controller;

     private Animator animator;
     private Image playerImage;


     [Range(0f, 30f)] public float speed = 5;
     public Group[] regularShootingPattern;
     public Group[] specialShootingPattern;
     public float maxSpeed;
     public float minSpeed;
     public float accel;
     public float deccel;
     [SerializeField] private float projectileSpawnInterval;

     private Vector2 _spawnPoint;
     private Vector2 direction = Vector2.zero;
     private Rigidbody2D rb;
     private float projectileTimer = 0f;
     private bool startProjectSpawnTimer = false;
     private bool startGeneratingProject = false;
     private bool skillActivated = false;
     [SerializeField] private int specialProjectileAmount = 50;
     private int specialProjectileCount = 0;

     void Awake()
     {
          if (instance != null && instance != this)
          {
               Destroy(this);
          }
          instance = this;

          rb = GetComponent<Rigidbody2D>();

          controller = new PlayerController();
          
          animator = GetComponentInChildren<Animator>();
          playerImage = GetComponentInChildren<Image>();
     }

     private void OnEnable()
     {
          controller.Enable();
          controller.Main.Shoot.started += Shoot;
          controller.Main.Shoot.canceled += StopShoot;
          controller.Main.Skill.started += StartSkillUse;

          GetComponent<Health>().healthChange.AddListener(OnHit);
          Graze.instance.endSkillTimer.AddListener(EndSkillUse);
     }

     private void Start()
     {
          projectileTimer = projectileSpawnInterval;
          _spawnPoint = transform.position;
     }

     void OnDisable()
     {
          controller.Main.Shoot.started -= Shoot;
          controller.Main.Shoot.canceled -= StopShoot;
          controller.Main.Skill.started -= StartSkillUse; 

          controller.Disable();
          GetComponent<Health>().healthChange.RemoveListener(OnHit);
          Graze.instance.endSkillTimer.RemoveListener(EndSkillUse);
     }

     void Update()
     {
          if (!controller.Main.Shoot.enabled)
          {
               return;
          }

          if (startProjectSpawnTimer)
          {
               projectileTimer += Time.deltaTime;

               if (startGeneratingProject)
               {
                    if (projectileTimer > projectileSpawnInterval)
                    {
                         projectileTimer = 0f;
                         if(!skillActivated){
                              GeneratePlayerProjectiles();
                         } else {
                              GeneratePlayerSpecialProjectiles();
                         }
                         
                    }
               }
          }
     }

     void FixedUpdate()
     {
          if (!controller.Main.Move.enabled)
          {
               return;
          }

          direction = controller.Main.Move.ReadValue<Vector2>();
          Move(direction);
     }

     private void Move(Vector2 direction)
     {
          float velocity = 0f;
          if (direction == Vector2.zero)
          {
               velocity = Mathf.Clamp(speed - Time.fixedDeltaTime * deccel, minSpeed, maxSpeed);
          }
          else
          {
               velocity = Mathf.Clamp(Time.fixedDeltaTime * accel + speed, minSpeed, maxSpeed);
          }
          rb.MovePosition((direction * velocity * Time.fixedDeltaTime) + (Vector2)transform.position);
          animator.SetBool(IS_MOVING, PlayerInputRaw != Vector2.zero);
          flipped = direction.x < 0;
          playerImage.transform.rotation = Quaternion.Euler(new Vector3(0f, flipped ? 180f : 0f, 0f));
     }

     private void Shoot(UnityEngine.InputSystem.InputAction.CallbackContext cont)
     {
          startProjectSpawnTimer = true;
          if (cont.interaction is HoldInteraction)
          {
               startGeneratingProject = true;
          }
     }

     private void StopShoot(UnityEngine.InputSystem.InputAction.CallbackContext cont)
     {
          if (cont.interaction is HoldInteraction)
          {
               startGeneratingProject = false;
          }
     }

     private void StartSkillUse(UnityEngine.InputSystem.InputAction.CallbackContext cont){
          if (!Graze.instance.IsGrazeFull()) return; 
          if(!skillActivated){
               Graze.instance.StartSkillTimer();
               skillActivated = true;
          }
     }

     private void EndSkillUse(){
          skillActivated = false;
          specialProjectileCount = 0;
     }

     private void GeneratePlayerProjectiles(){
          for (int i = 0; i < regularShootingPattern.Length; i++){
               float rad = regularShootingPattern[i].startingAngle * Mathf.Deg2Rad;
               float xPos = Mathf.Cos(rad) * regularShootingPattern[i].radius;
               float yPos = Mathf.Sin(rad) * regularShootingPattern[i].radius;
               Vector3 finalPosition = new Vector3(transform.position.x + xPos + regularShootingPattern[i].offset.x,
                    transform.position.y + yPos + regularShootingPattern[i].offset.y, 0f);
               GameObject projectile;
               if (Mathf.Approximately(regularShootingPattern[i].startingAngle, 90f))
               {
                    projectile = ProjectilePool.instance.ActivateProjectile(ProjectileType.forward);
               }
               else
               {
                    projectile = ProjectilePool.instance.ActivateProjectile(ProjectileType.angle);
               }
               projectile.transform.position = finalPosition;
               Projectile script = projectile.GetComponent<Projectile>();
               script.ConstructProjectile(regularShootingPattern[i].speed, regularShootingPattern[i].startingAngle);
          }
          AudioManager.Instance.PlayPlayerBullet_SFX();
     }

     private void GeneratePlayerSpecialProjectiles(){
          if(specialProjectileCount < specialProjectileAmount){
               for(int i = 0; i < specialShootingPattern.Length ; i++){
                    specialProjectileCount++;
                    float rad = specialShootingPattern[i].startingAngle * Mathf.Deg2Rad;
                    float xPos = Mathf.Cos(rad) * specialShootingPattern[i].radius;
                    float yPos = Mathf.Sin(rad) * specialShootingPattern[i].radius;
                    Vector3 finalPosition = new Vector3(transform.position.x + xPos + specialShootingPattern[i].offset.x,
                    transform.position.y + yPos + specialShootingPattern[i].offset.y, 0f);
                    GameObject projectile;
                    if (Mathf.Approximately(specialShootingPattern[i].startingAngle, 90f)){
                         projectile = ProjectilePool.instance.ActivateProjectile(ProjectileType.specialForward);
                    }
                    else{
                         projectile = ProjectilePool.instance.ActivateProjectile(ProjectileType.specialAngle);
                    }
                    projectile.transform.position = finalPosition;
                    Projectile script = projectile.GetComponent<Projectile>();
                    script.ConstructProjectile(specialShootingPattern[i].speed, specialShootingPattern[i].startingAngle);
               }
          }
          
     }
 

     private void OnHit(float currHealth, float maxHealth)
     {
          if (currHealth < maxHealth)
          {
               animator.SetBool("wasHit",true);
               StartCoroutine(RespawnPlayer());
          }
     }

     private IEnumerator RespawnPlayer()
     {
          
          var collider = GetComponent<Collider2D>();
          collider.enabled = false;
          controller.Main.Move.Disable();
          controller.Main.Shoot.Disable();
          controller.Main.Skill.Disable();

          // Reset logic states
          Move(Vector2.zero);
          startGeneratingProject = false;

          yield return new WaitForSeconds(RESPAWN_DELAY);

          animator.SetBool("wasHit", false);
          transform.position = spawnPoint.position;
          //Debug.Log("The new position is " + transform.position);
          collider.enabled = true;

          controller.Main.Move.Enable();
          controller.Main.Shoot.Enable();
          controller.Main.Skill.Enable();

          GetComponent<Health>().TriggerIFrames(I_FRAMES);
     }
}
