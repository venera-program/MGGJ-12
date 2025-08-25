using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ProjectileResources : ScriptableObject {
    private static string assetName = nameof(ProjectileResources);
    [Header("Enemy Projectiles")]
    public GameObject directed;
    public GameObject directed2;
    public GameObject undirected;
    public GameObject undirected2;
    [Header("Player Projectiles")]
    public GameObject forward;
    public GameObject angle;

    [Header("Default Projectiles")]
    public GameObject defaultProjectile;

    private static ProjectileResources _instance;
    public static ProjectileResources instance {
        get {
            if (_instance != null){
                return _instance;
            }
            _instance = Resources.Load<ProjectileResources>(assetName);
            if (_instance != null){
                Debug.Log("Loaded projectile resources from assets");
                return _instance;
            } else {
                _instance = CreateInstance<ProjectileResources>();
                #if UNITY_EDITOR
                UnityEditor.AssetDatabase.CreateAsset(_instance, $"Assets/Scripts/Ro/Resources/SO/{assetName}.asset");
                Debug.Log("New instance");
                #endif
                return _instance;
            }

        }
    }

 
}