using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using MGGJ25.Shared;

[RequireComponent(typeof(Rigidbody2D), typeof(Health), typeof(Collider2D))]
public class PlayerControllerScript : MonoBehaviour
{
     private const float RESPAWN_DELAY = 0.5f;
     [SerializeField] private float I_SECONDS = 120;
     private const string IS_MOVING = "isMoving";
     private const string WAS_HIT = "wasHit";
     private bool flipped;
    
     public Transform spawnPoint;

     public static PlayerControllerScript instance;

     public Vector2 PlayerInputRaw { get => direction; set => direction = value; }

     public PlayerController controller;

     private Animator animator;
     private Image playerImage;
     private PlayerSkill script;


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

     private Collider2D collider;

     private bool canMove = true;

     void Awake()
     {
          if (instance != null && instance != this)
          {
               Destroy(this);
          }
          instance = this;

          rb = GetComponent<Rigidbody2D>();
          collider = GetComponent<Collider2D>();

          controller = new PlayerController();
          
          animator = GetComponentInChildren<Animator>();
          playerImage = GetComponentInChildren<Image>();
          script = GetComponent<PlayerSkill>();
     }

     private void OnEnable()
     {
          controller.Enable();
          controller.Main.Shoot.started += Shoot;
          controller.Main.Shoot.canceled += StopShoot;
          controller.Main.Skill.started += StartSkillUse;
        if (MainMenu.instance != null)
        {
            controller.Main.Escape.performed += MainMenu.instance.OpenPauseMenu;

            controller.UI.Cancel.started += MainMenu.instance.BackButton;
        }
          GetComponent<Health>().healthChange.AddListener(OnHit);
          Graze.instance.endSkillTimer.AddListener(EndSkillUse);
          LevelManager.OnLevelChange += HealMC;
          LevelManager.OnLevelUnload += ResetMC;
          LevelManager.OnLevelUnload += PlayerData.ClearScore;
     }

     private void Start()
     {
          projectileTimer = projectileSpawnInterval;
          _spawnPoint = transform.position;
     }

     private void HealMC(sbyte currentLevel){
          if (currentLevel == 0){
               GetComponent<Health>().FullHeal();
          }
     }

     private void ResetMC(){
          StopCoroutine(RespawnPlayer());
          Move(Vector2.zero);
          startGeneratingProject = false;
          animator.SetBool("wasHit", false);
          transform.position = spawnPoint.position;
          collider.enabled = true;
          controller.Main.Move.Enable();
          controller.Main.Shoot.Enable();
          controller.Main.Skill.Enable();
          GetComponent<Health>().StopIFrames();
     }

     void OnDisable()
     {
          controller.Main.Shoot.started -= Shoot;
          controller.Main.Shoot.canceled -= StopShoot;
          controller.Main.Skill.started -= StartSkillUse; 
          controller.Main.Escape.performed -= MainMenu.instance.OpenPauseMenu;

          controller.UI.Cancel.started -= MainMenu.instance.BackButton;
          LevelManager.OnLevelChange -= HealMC;
          LevelManager.OnLevelUnload -= ResetMC;
          LevelManager.OnLevelUnload -= PlayerData.ClearScore;

          controller.Disable();
          InputSystem.PauseHaptics();
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
          if (!canMove)
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
               if(!skillActivated){
                    AudioManager.Instance.PlayPlayerBullet_SFX();
               } else {
                    AudioManager.Instance.PlayPlayerSpecial_SFX();
               }
          }
     }

     private void StopShoot(UnityEngine.InputSystem.InputAction.CallbackContext cont)
     {
          if (cont.interaction is HoldInteraction)
          {
               if(!skillActivated){
                    AudioManager.Instance.StopPlayerBullet_SFX();
               } else {
                    AudioManager.Instance.StopPlayerSpecial_SFX();
               }
               startGeneratingProject = false;
          }
     }

     private void StartSkillUse(UnityEngine.InputSystem.InputAction.CallbackContext cont){
          if (!Graze.instance.IsGrazeFull()) return; 
          if(!skillActivated){
               Graze.instance.StartSkillTimer();
               skillActivated = true;
               script.switchToSkillAnimation();
          }
     }

     private void EndSkillUse(){
          AudioManager.Instance.StopPlayerSpecial_SFX();
          Debug.Log("You have reached the endskilluse method, please leave a message after the tone.");
          script.skillIsOver();
          if(startGeneratingProject){
               AudioManager.Instance.PlayPlayerBullet_SFX();
          }
          skillActivated = false;
          
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
     }

     private void GeneratePlayerSpecialProjectiles(){
          bool enoughSpecialAngle = ProjectilePool.instance.GetAvailableProjectileCount(ProjectileType.specialAngle) > specialShootingPattern.Length - 1;
          bool enoughSpecialForward = ProjectilePool.instance.GetAvailableProjectileCount(ProjectileType.specialForward) > 1;
          if(!enoughSpecialAngle || !enoughSpecialForward) return;

               for(int i = 0; i < specialShootingPattern.Length ; i++){
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

     public void EnablePlayerControls(){
          controller.Main.Enable();
          canMove = true;
          controller.Main.Shoot.started += Shoot;
          controller.Main.Shoot.canceled += StopShoot;
          controller.Main.Skill.started += StartSkillUse;
          controller.Main.Escape.Enable();
     }

     public void DisablePlayerControls(){
          controller.Main.Shoot.started -= Shoot;
          controller.Main.Shoot.canceled -= StopShoot;
          controller.Main.Skill.started -= StartSkillUse;
          controller.Main.Escape.Disable();
          canMove = false;
          controller.Main.Disable();
          InputSystem.PauseHaptics();
     }
 

     private void OnHit(float currHealth, float maxHealth)
     {
          if (currHealth < maxHealth)
          {
               AudioManager.Instance.PlayPlayerDies_SFX();
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

          GetComponent<Health>().TriggerIFrames(I_SECONDS);
     }
}
