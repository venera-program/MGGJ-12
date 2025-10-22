using System.Collections.Generic;
using UnityEngine;
using MGGJ25.Shared;
using UnityEngine.Events;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool instance;

    [Header("Enemy Projectile Count")]
    [SerializeField] private float directed;
    [SerializeField] private float directed2;
    [SerializeField] private float undirected;
    [SerializeField] private float undirected2;

    [Header("Enemy Projectile Lists")]
    private Queue<GameObject> directedPool = new Queue<GameObject>();
    private Queue<GameObject> directed2Pool = new Queue<GameObject>();
    private Queue<GameObject> undirectedPool = new Queue<GameObject>();
    private Queue<GameObject> undirected2Pool = new Queue<GameObject>();

    [Header("Player Projectile Count")]
    [SerializeField] private float angle;
    [SerializeField] private float forward;
    [SerializeField] private float specialAngle;
    [SerializeField] private float specialForward;

    [Header("Player Projectile Lists")]
    private Queue<GameObject> anglePool = new Queue<GameObject>();
    private Queue<GameObject> forwardPool = new Queue<GameObject>();
    private Queue<GameObject> specialAnglePool = new Queue<GameObject>();
    private Queue<GameObject> specialForwardPool = new Queue<GameObject>();

    [Header("Special Attack Event")]
    public UnityEvent<int> specialAttackCount = new UnityEvent<int>();

    private List<Projectile> projectiles = new List<Projectile>();

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        specialAngle = specialForward * 4;
        GenerateEnemyProjectilePools();
        GeneratePlayerProjectilePools();
    }

    private void GenerateEnemyProjectilePools()
    {
        for (int i = 0; i < directed; i++)
        {
            GameObject newProject = Instantiate(ProjectileResources.instance.directed, transform);
            Projectile temp = newProject.GetComponent<Projectile>();
            temp.projectileType = ProjectileType.directed;
            projectiles.Add(temp);
            newProject.SetActive(false);
            directedPool.Enqueue(newProject);
        }
        for (int i = 0; i < directed2; i++)
        {
            GameObject newProject = Instantiate(ProjectileResources.instance.directed2, transform);
            Projectile temp = newProject.GetComponent<Projectile>();
            temp.projectileType = ProjectileType.directed2;
            projectiles.Add(temp);
            newProject.SetActive(false);
            directed2Pool.Enqueue(newProject);
        }
        for (int i = 0; i < undirected; i++)
        {
            GameObject newProject = Instantiate(ProjectileResources.instance.undirected, transform);
            Projectile temp = newProject.GetComponent<Projectile>();
            temp.projectileType = ProjectileType.undirected;
            projectiles.Add(temp);
            newProject.SetActive(false);
            undirectedPool.Enqueue(newProject);
        }
        for (int i = 0; i < undirected2; i++)
        {
            GameObject newProject = Instantiate(ProjectileResources.instance.undirected2, transform);
            Projectile temp = newProject.GetComponent<Projectile>();
            temp.projectileType = ProjectileType.undirected2;
            projectiles.Add(temp);
            newProject.SetActive(false);
            undirected2Pool.Enqueue(newProject);
        }
    }

    private void GeneratePlayerProjectilePools()
    {
        for (int i = 0; i < angle; i++)
        {
            GameObject newProject = Instantiate(ProjectileResources.instance.angle, transform);
            Projectile temp = newProject.GetComponent<Projectile>();
            temp.projectileType = ProjectileType.angle;
            projectiles.Add(temp);
            newProject.SetActive(false);
            anglePool.Enqueue(newProject);
        }
        for (int i = 0; i < forward; i++)
        {
            GameObject newProject = Instantiate(ProjectileResources.instance.forward, transform);
            Projectile temp = newProject.GetComponent<Projectile>();
            temp.projectileType = ProjectileType.forward;
            projectiles.Add(temp);
            newProject.SetActive(false);
            forwardPool.Enqueue(newProject);
        }
        for (int i = 0; i < specialAngle; i++)
        {
            GameObject newProject = Instantiate(ProjectileResources.instance.specialAngle, transform);
            Projectile temp = newProject.GetComponent<Projectile>();
            temp.projectileType = ProjectileType.specialAngle;
            projectiles.Add(temp);
            newProject.SetActive(false);
            specialAnglePool.Enqueue(newProject);
        }
        for (int i = 0; i < specialForward; i++)
        {
            GameObject newProject = Instantiate(ProjectileResources.instance.specialForward, transform);
            Projectile temp = newProject.GetComponent<Projectile>();
            temp.projectileType = ProjectileType.specialForward;
            projectiles.Add(temp);
            newProject.SetActive(false);
            specialForwardPool.Enqueue(newProject);
        }
    }

    public GameObject ActivateProjectile(ProjectileType type)
    {
        GameObject project;

        switch (type)
        {
            case ProjectileType.directed:
                project = directedPool.Count > 0 ? directedPool.Dequeue() : ProjectileResources.instance.defaultProjectile;
                break;
            case ProjectileType.directed2:
                project = directed2Pool.Count > 0 ? directed2Pool.Dequeue() : ProjectileResources.instance.defaultProjectile;
                break;
            case ProjectileType.undirected:
                project = undirectedPool.Count > 0 ? undirectedPool.Dequeue() : ProjectileResources.instance.defaultProjectile;
                break;
            case ProjectileType.undirected2:
                project = undirected2Pool.Count > 0 ? undirected2Pool.Dequeue() : ProjectileResources.instance.defaultProjectile;
                break;
            case ProjectileType.angle:
                project = anglePool.Count > 0 ? anglePool.Dequeue() : ProjectileResources.instance.defaultProjectile;
                break;
            case ProjectileType.forward:
                project = forwardPool.Count > 0 ? forwardPool.Dequeue() : ProjectileResources.instance.defaultProjectile;
                break;
            case ProjectileType.specialAngle:
                project = specialAnglePool.Count > 0 ? specialAnglePool.Dequeue() : ProjectileResources.instance.defaultProjectile;
                specialAttackCount.Invoke(specialAnglePool.Count + specialForwardPool.Count);
                break;
            case ProjectileType.specialForward:
                project = specialForwardPool.Count > 0 ? specialForwardPool.Dequeue() : ProjectileResources.instance.defaultProjectile;
                specialAttackCount.Invoke(specialAnglePool.Count + specialForwardPool.Count);
                break;
            default:
                project = ProjectileResources.instance.defaultProjectile;
                break;
        }

        project.SetActive(true);
        return project;
    }

    public void DeactivateProjectile(Projectile project)
    {
        project.gameObject.SetActive(false);
        switch (project.projectileType)
        {
            case ProjectileType.directed:
                directedPool.Enqueue(project.gameObject);
                break;
            case ProjectileType.directed2:
                directed2Pool.Enqueue(project.gameObject);
                break;
            case ProjectileType.undirected:
                undirectedPool.Enqueue(project.gameObject);
                break;
            case ProjectileType.undirected2:
                undirected2Pool.Enqueue(project.gameObject);
                break;
            case ProjectileType.angle:
                anglePool.Enqueue(project.gameObject);
                break;
            case ProjectileType.forward:
                forwardPool.Enqueue(project.gameObject);
                break;
            case ProjectileType.specialAngle:
                specialAnglePool.Enqueue(project.gameObject);
                specialAttackCount.Invoke(specialAnglePool.Count + specialForwardPool.Count);
                break;
            case ProjectileType.specialForward:
                specialForwardPool.Enqueue(project.gameObject);
                specialAttackCount.Invoke(specialAnglePool.Count + specialForwardPool.Count);
                break;
        }
    }

    public int GetAvailableProjectileCount(ProjectileType type)
    {
        switch (type)
        {
            case ProjectileType.directed:
                return directedPool.Count;
            case ProjectileType.directed2:
                return directed2Pool.Count;
            case ProjectileType.undirected:
                return undirectedPool.Count;
            case ProjectileType.undirected2:
                return undirected2Pool.Count;
            case ProjectileType.angle:
                return anglePool.Count;
            case ProjectileType.forward:
                return forwardPool.Count;
            case ProjectileType.specialAngle:
                return specialAnglePool.Count;
            case ProjectileType.specialForward:
                return specialForwardPool.Count;
            default:
                return 0;
        }
    }

    public void ResetAllProjectiles()
    {
        foreach (GameObject a in directedPool)
        {
            a.SetActive(false);
        }
        foreach (GameObject a in directed2Pool)
        {
            a.SetActive(false);
        }
        foreach (GameObject a in undirectedPool)
        {
            a.SetActive(false);
        }
        foreach (GameObject a in undirected2Pool)
        {
            a.SetActive(false);
        }
        foreach (GameObject a in anglePool)
        {
            a.SetActive(false);
        }
        foreach (GameObject a in forwardPool)
        {
            a.SetActive(false);
        }
        foreach (GameObject a in specialAnglePool)
        {
            a.SetActive(false);
        }
        foreach (GameObject a in specialForwardPool)
        {
            a.SetActive(false);
        }
        specialAttackCount.Invoke(specialAnglePool.Count + specialForwardPool.Count);
    }
    public void UnpauseProjectilesMovement(){
        foreach(Projectile A in projectiles){
            A.StartMoving();
        }
    }
    public void StopProjectilesMovement(){
        foreach(Projectile A in projectiles){
            A.StopMoving();
        }
    }
}

public enum ProjectileType
{
    directed,
    directed2,
    undirected,
    undirected2,
    forward,
    angle,
    specialForward,
    specialAngle,
    def
}
