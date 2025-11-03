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
     private const string IS_MOVING = "isMoving";
     private const string WAS_HIT = "wasHit";
     private bool flipped;

     public Transform spawnPoint;

     public static PlayerControllerScript instance;

     public Vector2 PlayerInputRaw { get => direction; set => direction = value; }

     private PlayerController _controller;
     private Animator _animator;
     private SpriteRenderer sprite;
     private PlayerSkill _skill;

     [Range(0f, 30f)] public float speed = 5;
     public Group[] regularShootingPattern;
     public Group[] specialShootingPattern;
     public float maxSpeed;
     public float minSpeed;
     public float accel;
     public float deccel;
     [SerializeField] private float projectileSpawnInterval;

     private Vector2 direction = Vector2.zero;
     private Rigidbody2D _RB;
     private float projectileTimer = 0f;
     private bool startProjectSpawnTimer = false;
     private bool startGeneratingProject = false;
     private bool skillActivated = false;
     private Collider2D _Collider;
     private bool canMove = true;

     void Awake()
     {
          if (instance != null && instance != this)
          {
               Destroy(this);
          }
          instance = this;

          _controller = new PlayerController();
          _RB = GetComponent<Rigidbody2D>();
          _Collider = GetComponent<Collider2D>();
          _animator = GetComponentInChildren<Animator>();
          sprite = GetComponentInChildren<SpriteRenderer>();
          _skill = GetComponent<PlayerSkill>();
     }

     private void OnEnable()
     {
          _controller.Enable();
          _controller.Main.Shoot.started += Shoot;
          _controller.Main.Shoot.canceled += StopShoot;
          _controller.Main.Skill.started += StartSkillUse;
          GetComponent<Health>().healthChange.AddListener(OnHit);
          Graze.instance.endSkillTimer.AddListener(EndSkillUse);
          _controller.UI.Cancel.started += UICancel;
          _controller.Main.Escape.performed += OpenPauseMenu;
          LevelManager.OnLevelChange += HealMC;
          LevelManager.OnLevelUnload += ResetMC;
          LevelManager.OnLevelUnload += PlayerData.ClearScore;
     }

     private void Start(){
          projectileTimer = projectileSpawnInterval;
     }

     private void HealMC(sbyte currentLevel)
     {
          GetComponent<Health>().FullHeal();
     }

     private void ResetMC()
     {
          StopCoroutine(RespawnPlayer());
          Move(Vector2.zero);
          startGeneratingProject = false;
          _animator.SetBool(WAS_HIT, false);
          EndSkillUse();
          transform.position = spawnPoint.position;
          _Collider.enabled = true;
          _controller.Main.Move.Enable();
          _controller.Main.Shoot.Enable();
          _controller.Main.Skill.Enable();
          GetComponent<Health>().StopIFrames();
     }

     void OnDisable()
     {
          _controller.Main.Shoot.started -= Shoot;
          _controller.Main.Shoot.canceled -= StopShoot;
          _controller.Main.Skill.started -= StartSkillUse;
          _controller.Main.Escape.performed -= OpenPauseMenu;
          _controller.UI.Cancel.started -= UICancel;
          LevelManager.OnLevelChange -= HealMC;
          LevelManager.OnLevelUnload -= ResetMC;
          LevelManager.OnLevelUnload -= PlayerData.ClearScore;

          _controller.Disable();
          InputSystem.PauseHaptics();
          GetComponent<Health>().healthChange.RemoveListener(OnHit);
          Graze.instance.endSkillTimer.RemoveListener(EndSkillUse);
     }

     void Update()
     {
          if (!_controller.Main.Shoot.enabled)
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
                         if (!skillActivated)
                         {
                              GeneratePlayerProjectiles();
                         }
                         else
                         {
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

          direction = _controller.Main.Move.ReadValue<Vector2>();
          Move(direction);
     }

     private void Move(Vector2 direction)
     {
          float velocity;
          if (direction == Vector2.zero)
          {
               velocity = Mathf.Clamp(speed - Time.fixedDeltaTime * deccel, minSpeed, maxSpeed);
          }
          else
          {
               velocity = Mathf.Clamp(Time.fixedDeltaTime * accel + speed, minSpeed, maxSpeed);
          }
          _RB.MovePosition((direction * velocity * Time.fixedDeltaTime) + (Vector2)transform.position);
          _animator.SetBool(IS_MOVING, PlayerInputRaw != Vector2.zero);
          flipped = direction.x < 0;
          sprite.flipX = flipped;
     }

     private void Shoot(InputAction.CallbackContext cont)
     {
          startProjectSpawnTimer = true;
          if (cont.interaction is HoldInteraction)
          {
               startGeneratingProject = true;
               if (!skillActivated)
               {
                    AudioManager.Instance.PlayPlayerBullet_SFX();
               }
               else
               {
                    AudioManager.Instance.PlayPlayerSpecial_SFX();
               }
          }
     }

     private void StopShoot(InputAction.CallbackContext cont)
     {
          if (cont.interaction is HoldInteraction)
          {
              StopShoot();
          }
     }

     private void StopShoot(){
          if (!skillActivated)
               {
                    AudioManager.Instance.StopPlayerBullet_SFX();
               }
               else
               {
                    AudioManager.Instance.StopPlayerSpecial_SFX();
               }
               startGeneratingProject = false;
     }

     private void StartSkillUse(InputAction.CallbackContext cont)
     {
          if (!Graze.instance.IsGrazeFull()) return;
          if (!skillActivated)
          {
               skillActivated = true;
               Graze.instance.StartSkillTimer();
               _skill.SwitchToSkillAnimation();
          }
     }

     public bool isSkillActivated(){
          return skillActivated;
     }

     private void EndSkillUse()
     {
          AudioManager.Instance.StopPlayerSpecial_SFX();
          Debug.Log("You have reached the endskilluse method, please leave a message after the tone.");
          _skill.SkillIsOver();
          // If firing projectiles when skill disabled, turn on default soundloop
          if (startGeneratingProject)
          {
               AudioManager.Instance.PlayPlayerBullet_SFX();
          }
          skillActivated = false;
     }

     private void GeneratePlayerProjectiles()
     {
          for (int i = 0; i < regularShootingPattern.Length; i++)
          {
               float rad = regularShootingPattern[i].startingAngle * Mathf.Deg2Rad;
               float xPos = Mathf.Cos(rad) * regularShootingPattern[i].radius;
               float yPos = Mathf.Sin(rad) * regularShootingPattern[i].radius;
               GameObject projectile;
               if (Mathf.Approximately(regularShootingPattern[i].startingAngle, 90f))
               {
                    projectile = ProjectilePool.instance.ActivateProjectile(ProjectileType.forward);
               }
               else
               {
                    projectile = ProjectilePool.instance.ActivateProjectile(ProjectileType.angle);
               }
               projectile.transform.position = new Vector3(transform.position.x + xPos + regularShootingPattern[i].offset.x,
                                                           transform.position.y + yPos + regularShootingPattern[i].offset.y,
                                                           0f);
               projectile.GetComponent<Projectile>().ConstructProjectile(regularShootingPattern[i].speed, regularShootingPattern[i].startingAngle);
          }
     }

     private void GeneratePlayerSpecialProjectiles()
     {
          bool enoughSpecialAngle = ProjectilePool.instance.GetAvailableProjectileCount(ProjectileType.specialAngle) > specialShootingPattern.Length - 1;
          bool enoughSpecialForward = ProjectilePool.instance.GetAvailableProjectileCount(ProjectileType.specialForward) > 1;
          if (!enoughSpecialAngle || !enoughSpecialForward) return;

          for (int i = 0; i < specialShootingPattern.Length; i++)
          {
               float rad = specialShootingPattern[i].startingAngle * Mathf.Deg2Rad;
               float xPos = Mathf.Cos(rad) * specialShootingPattern[i].radius;
               float yPos = Mathf.Sin(rad) * specialShootingPattern[i].radius;
               GameObject projectile;
               if (Mathf.Approximately(specialShootingPattern[i].startingAngle, 90f))
               {
                    projectile = ProjectilePool.instance.ActivateProjectile(ProjectileType.specialForward);
               }
               else
               {
                    projectile = ProjectilePool.instance.ActivateProjectile(ProjectileType.specialAngle);
               }
               projectile.transform.position = new Vector3(transform.position.x + xPos + specialShootingPattern[i].offset.x,
                                                           transform.position.y + yPos + specialShootingPattern[i].offset.y,
                                                           0f);
               projectile.GetComponent<Projectile>().ConstructProjectile(specialShootingPattern[i].speed, specialShootingPattern[i].startingAngle);
          }
     }

     private void UICancel(InputAction.CallbackContext cont){
          if(MainMenu.instance != null){
               MainMenu.instance.BackButton(cont);
          }
     }

     private void OpenPauseMenu(InputAction.CallbackContext cont){
          if(MainMenu.instance != null){
               MainMenu.instance.OpenPauseMenu(cont);
          }

     }

     public void EnablePlayerControls(){
          _controller.Main.Enable();
          canMove = true;
          _controller.Main.Shoot.started += Shoot;
          _controller.Main.Shoot.canceled += StopShoot;
          _controller.Main.Skill.started += StartSkillUse;
          _controller.Main.Escape.Enable();
     }

     public void DisablePlayerControls()
     {
          StopShoot();
          _controller.Main.Shoot.started -= Shoot;
          _controller.Main.Shoot.canceled -= StopShoot;
          _controller.Main.Skill.started -= StartSkillUse;
          _controller.Main.Escape.Disable();
          canMove = false;
          _controller.Main.Disable();
          Gamepad.current?.PauseHaptics();
     }

     public void EnablePauseButton()
     {
          _controller.Main.Escape.Enable();
     }

     public void DisablePauseButton()
     {
          _controller.Main.Escape.Disable();
     }

     public void StopAnimations(){
          _animator.enabled = false;
     }

     public void StartAnimations(){
          _animator.enabled = true;
     }

     private void OnHit(float currHealth, float maxHealth)
     {
          if (currHealth < maxHealth)
          {
               AudioManager.Instance.PlayPlayerDies_SFX();
               _animator.SetBool(WAS_HIT, true);
               StartCoroutine(RespawnPlayer());
          }
     }

     private IEnumerator RespawnPlayer()
     {
          var collider = GetComponent<Collider2D>();
          collider.enabled = false;
          _controller.Main.Move.Disable();
          _controller.Main.Shoot.Disable();
          _controller.Main.Skill.Disable();

          // Reset logic states
          Move(Vector2.zero);
          startGeneratingProject = false;

          yield return new WaitForSeconds(RESPAWN_DELAY);

          _animator.SetBool(WAS_HIT, false);
          transform.position = spawnPoint.position;
          collider.enabled = true;

          _controller.Main.Move.Enable();
          _controller.Main.Shoot.Enable();
          _controller.Main.Skill.Enable();
     }
}