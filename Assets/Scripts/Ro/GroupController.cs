using UnityEngine;
using System;
using MGGJ25.Shared;


public class GroupController : MonoBehaviour
{

    private const int MAX_GROUPS = 50;
    private Timer[] projectileSpawnTimers = new Timer[MAX_GROUPS];

    private Group[] groups;
    private bool startSpawning = false;
    private GameObject parent;
    private Animator animator;
    private EnemyAnimation enemyAnimation;

    void Awake()
    {
        parent = transform.parent.gameObject;
        animator = parent.GetComponentInChildren<Animator>();
        enemyAnimation = parent.GetComponentInChildren<EnemyAnimation>();
    }

    void Update()
    {
        if (startSpawning){
            for (int i = 0; i < groups.Length; i++){
                projectileSpawnTimers[i].Update(Time.deltaTime);
                if(projectileSpawnTimers[i].isTimerDone()){
                    DebugMethods.PrintGroupInformation(groups[i], projectileSpawnTimers[i].CurrentTime(), i, parent.name);
                    projectileSpawnTimers[i].ResetTimer();
                    StartSpawning(groups[i]);
                }
            }
        }
    }

    public void StartGroup(Group[] currGroup)
    {
        groups = currGroup;
        for(int i = 0 ; i < currGroup.Length; i++){
            projectileSpawnTimers[i].Initialize(currGroup[i].spawnInterval, currGroup[i].delay);
        }
        startSpawning = true;
    }
    private void StartSpawning(Group currGroup)
    {
        enemyAnimation.PukeBullets();
        if(currGroup.projectileCount != 0){
            AudioManager.Instance.PlayEnemyBullet_SFX();
        }
        switch (currGroup.pattern)
        {
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

    private void SpawnRing(Group ring)
    {
        for (int i = 0; i < ring.projectileCount; i++)
        {
            float positionAngle = HelperFunctions.CalculateProjectilePositionAngle(i, ring);
            float xPos = Mathf.Cos(positionAngle);
            float yPos = Mathf.Sin(positionAngle);
            Vector2 projectilePosition = new Vector2 ((xPos * ring.radius) + transform.position.x + ring.offset.x,
                                (yPos * ring.radius) + transform.position.y + ring.offset.y);
            GameObject projectile;
            if (ring.movementAngle == MovementAngle.Fixed)
            {
                projectile = ProjectilePool.instance.ActivateProjectile(ProjectileType.undirected);
            }
            else if (ring.movementAngle == MovementAngle.TowardsPlayerDistorted || ring.movementAngle == MovementAngle.TowardsPlayerRigid)
            {
                projectile = ProjectilePool.instance.ActivateProjectile(ProjectileType.directed);
            }
            else
            {
                Debug.LogError("How did you get here?");
                projectile = ProjectilePool.instance.ActivateProjectile(ProjectileType.def);
            }

            projectile.transform.position = projectilePosition;
            Projectile proj = projectile.GetComponent<Projectile>();
            float movementAngle = HelperFunctions.CaluclateProjectileMovementAngle(i, ring, projectilePosition, transform.position);
            proj.ConstructProjectile(ring.speed, movementAngle);
        }
    }

    private void SpawnStack(Group stack)
    {
        for (int i = 0; i < stack.projectileCount; i++)
        {
            float positionAngle = HelperFunctions.CalculateProjectilePositionAngle(i, stack);
            float xPos = Mathf.Cos(positionAngle);
            float yPos = Mathf.Sin(positionAngle);
            Vector2 projectilePosition = new Vector2 ((xPos * stack.radius) + transform.position.x + stack.offset.x,
                                             (yPos * stack.radius) + transform.position.y + stack.offset.y);

            GameObject projectile;
            if (stack.movementAngle == MovementAngle.Fixed)
            {
                projectile = ProjectilePool.instance.ActivateProjectile(ProjectileType.undirected);
            }
            else if (stack.movementAngle == MovementAngle.TowardsPlayerDistorted || stack.movementAngle == MovementAngle.TowardsPlayerRigid)
            {
                projectile = ProjectilePool.instance.ActivateProjectile(ProjectileType.directed);
            }
            else
            {
                Debug.LogError("How did you get here?");
                projectile = ProjectilePool.instance.ActivateProjectile(ProjectileType.def);
            }
            projectile.transform.position = projectilePosition;
            float angle = HelperFunctions.CaluclateProjectileMovementAngle(i, stack, projectilePosition, transform.position);
            projectile.GetComponent<Projectile>().ConstructProjectile(stack.speed + (stack.speed * stack.speedMultiplier * (i + 1)), angle);
        }
    }

    public void StopSpawning(){
        startSpawning = false;
    }

    public void UnpauseSpawning(){
        startSpawning = true;
    }

}

public enum MovementAngle
{
    Fixed,
    TowardsPlayerDistorted,
    TowardsPlayerRigid
}

[Serializable]
public enum PositionAngle
{
    FixedPosition,
    RandomPosition,
}

public enum GroupType
{
    Ring,
    Spread,
    Stack
}