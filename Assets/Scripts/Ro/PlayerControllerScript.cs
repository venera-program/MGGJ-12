using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using MGGJ25.Shared;

[RequireComponent(typeof(Rigidbody2D), typeof(Health), typeof(Collider2D))]
public class PlayerControllerScript : MonoBehaviour
{
     private const float RESPAWN_DELAY = 0.5f;
     private const int I_FRAMES = 120;
     private const string IS_MOVING = "isMoving";
     private const string MOVING_LEFT = "movingLeft";
     private const string WAS_HIT = "wasHit";

     public static PlayerControllerScript instance;

     public Vector2 PlayerInputRaw { get => direction; set => direction = value; }

     public PlayerController controller;

     public Animator animator;

     [Range(0f, 30f)] public float speed = 5;
     public Group[] shootingPattern;
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

     void Awake()
     {
          if (instance != null && instance != this)
          {
               Destroy(this);
          }
          instance = this;

          rb = GetComponent<Rigidbody2D>();

          controller = new PlayerController();
          _spawnPoint = transform.position;
     }

     private void OnEnable()
     {
          controller.Enable();
          controller.Main.Shoot.started += Shoot;
          controller.Main.Shoot.canceled += StopShoot;
          controller.Main.Move.performed += UpdateMovementAnimations;
          GetComponent<Health>().healthChange.AddListener(OnHit);
     }

     private void Start()
     {
          projectileTimer = projectileSpawnInterval;
     }

     void OnDisable()
     {
          controller.Main.Shoot.started -= Shoot;
          controller.Main.Shoot.canceled -= StopShoot;
          controller.Main.Move.performed -= UpdateMovementAnimations;
          controller.Disable();
          GetComponent<Health>().healthChange.RemoveListener(OnHit);
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
                         GeneratePlayerProjectiles();
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

     private void GeneratePlayerProjectiles()
     {
          for (int i = 0; i < shootingPattern.Length; i++)
          {
               float rad = shootingPattern[i].startingAngle * Mathf.Deg2Rad;
               float xPos = Mathf.Cos(rad) * shootingPattern[i].radius;
               float yPos = Mathf.Sin(rad) * shootingPattern[i].radius;
               Vector3 finalPosition = new Vector3(transform.position.x + xPos + shootingPattern[i].offset.x,
                    transform.position.y + yPos + shootingPattern[i].offset.y, 0f);
               GameObject projectile;
               if (Mathf.Approximately(shootingPattern[i].startingAngle, 90f))
               {
                    projectile = ProjectilePool.instance.ActivateProjectile(ProjectileType.forward);
               }
               else
               {
                    projectile = ProjectilePool.instance.ActivateProjectile(ProjectileType.angle);
               }
               projectile.transform.position = finalPosition;
               Projectile script = projectile.GetComponent<Projectile>();
               Debug.Log(shootingPattern[i].startingAngle);
               script.ConstructProjectile(shootingPattern[i].speed, shootingPattern[i].startingAngle);
          }
          AudioManager.Instance.PlayPlayerBullet_SFX();
     }

     private void UpdateMovementAnimations(UnityEngine.InputSystem.InputAction.CallbackContext _)
     {
          animator.SetBool(IS_MOVING, PlayerInputRaw != Vector2.zero);
          animator.SetBool(MOVING_LEFT, PlayerInputRaw.x < 0);
     }

     private void OnHit(float currHealth, float maxHealth)
     {
          if (currHealth < maxHealth)
          {
               StartCoroutine(RespawnPlayer());
          }
     }

     private IEnumerator RespawnPlayer()
     {
          animator.SetBool(WAS_HIT, true);
          controller.Main.Move.Disable();
          controller.Main.Shoot.Disable();
          controller.Main.Skill.Disable();

          // Reset logic states
          Move(Vector2.zero);
          startGeneratingProject = false;

          var collider = GetComponent<Collider2D>();
          collider.enabled = false;

          yield return new WaitForSeconds(RESPAWN_DELAY);

          transform.position = _spawnPoint;
          collider.enabled = true;

          controller.Main.Move.Enable();
          controller.Main.Shoot.Enable();
          controller.Main.Skill.Enable();

          GetComponent<Health>().TriggerIFrames(I_FRAMES);
     }
}
