using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Player Projectile Lists")]
    private Queue<GameObject> anglePool = new Queue<GameObject>();
    private Queue<GameObject> forwardPool = new Queue<GameObject>();


    void Awake(){
        if (instance != null && instance != this){
            Destroy(gameObject);
        } else {
            instance = this;
        }
        GenerateEnemyProjectilePools();
        GeneratePlayerProjectilePools();
    }

    private void GenerateEnemyProjectilePools(){
        for(int i = 0; i < directed; i++){
            GameObject newProject = Instantiate(ProjectileResources.instance.directed,transform);
            newProject.GetComponent<Projectile>().projectileType = ProjectileType.directed;
            newProject.SetActive(false);
            directedPool.Enqueue(newProject);
        }
        for(int i = 0; i < directed2; i++){
            GameObject newProject = Instantiate(ProjectileResources.instance.directed2,transform);
            newProject.GetComponent<Projectile>().projectileType = ProjectileType.directed2;
            newProject.SetActive(false);
            directed2Pool.Enqueue(newProject);
        }
        for(int i = 0; i < undirected; i++){
            GameObject newProject = Instantiate(ProjectileResources.instance.undirected,transform);
            newProject.GetComponent<Projectile>().projectileType = ProjectileType.undirected;
            newProject.SetActive(false);
            undirectedPool.Enqueue(newProject);
        }
        for(int i = 0; i < undirected2; i++){
            GameObject newProject = Instantiate(ProjectileResources.instance.undirected2,transform);
            newProject.GetComponent<Projectile>().projectileType = ProjectileType.undirected2;
            newProject.SetActive(false);
            undirected2Pool.Enqueue(newProject);
        }
    }

    private void GeneratePlayerProjectilePools(){
        for(int i = 0; i < angle; i++){
            GameObject newProject = Instantiate(ProjectileResources.instance.angle,transform);
            newProject.GetComponent<Projectile>().projectileType = ProjectileType.angle;
            newProject.SetActive(false);
            anglePool.Enqueue(newProject);
        }
        for(int i = 0; i < forward; i++){
            GameObject newProject = Instantiate(ProjectileResources.instance.forward,transform);
            newProject.GetComponent<Projectile>().projectileType = ProjectileType.forward;
            newProject.SetActive(false);
            forwardPool.Enqueue(newProject);
        }
    }

    public GameObject ActivateProjectile(ProjectileType type){
        GameObject project;
        switch (type){
            case(ProjectileType.directed):
                project = directedPool.Count > 0 ? directedPool.Dequeue() : ProjectileResources.instance.defaultProjectile;
                break;
            case(ProjectileType.directed2):
                project = directed2Pool.Count > 0 ? directed2Pool.Dequeue() : ProjectileResources.instance.defaultProjectile;
                break;
            case(ProjectileType.undirected):
                project = undirectedPool.Count > 0 ? undirectedPool.Dequeue() : ProjectileResources.instance.defaultProjectile;
                break;
            case(ProjectileType.undirected2):
                project = undirected2Pool.Count > 0 ? undirected2Pool.Dequeue() : ProjectileResources.instance.defaultProjectile ;
                break;
            case(ProjectileType.angle):
                project = anglePool.Count > 0 ? anglePool.Dequeue() : ProjectileResources.instance.defaultProjectile;
                break;
            case(ProjectileType.forward):
                project = forwardPool.Count > 0 ? forwardPool.Dequeue() : ProjectileResources.instance.defaultProjectile;
                break;
            default:
                project = ProjectileResources.instance.defaultProjectile;
                break;
        }

        project.SetActive(true);
        return project;
    }

    public void DeactivateProjectile(Projectile project){
        project.gameObject.SetActive(false);
        switch (project.projectileType){
            case(ProjectileType.directed):
                directedPool.Enqueue(project.gameObject);
                break;
            case(ProjectileType.directed2):
                directed2Pool.Enqueue(project.gameObject);
                break;
            case(ProjectileType.undirected):
                undirectedPool.Enqueue(project.gameObject);
                break;
            case(ProjectileType.undirected2):
                undirected2Pool.Enqueue(project.gameObject);
                break;
            case(ProjectileType.angle):
                anglePool.Enqueue(project.gameObject);
                break;
            case(ProjectileType.forward):
                forwardPool.Enqueue(project.gameObject);
                break;
        }
    }

}

public enum ProjectileType {
    directed,
    directed2, 
    undirected,
    undirected2,
    forward,
    angle,
    def
}
