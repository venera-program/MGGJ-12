using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(GroupController))]
public class PatternController : MonoBehaviour
{
    public Pattern[] Patterns;
    private int index = 0;
    private float timer = 0;
    private bool hasStarted = false;
    private GroupController groupSpawner;

    void Awake(){
        groupSpawner = GetComponent<GroupController>();
    }

    // percentage of health
    void Start(){
        groupSpawner.StartGroup(Patterns[0].groups);
    }

}

[Serializable]
public struct Group {
    public GroupType pattern;
    public float projectileCount;
    [Range(0,360)]public float startingAngle;
    [Range(0,360)]public float endingAngle;
    public float spawnInterval;
    public float delay;
    public float radius;
    public Vector2 offset;
    public MovementAngle movementAngle;
    public PositionAngle positionAngle;
    public float speed;
    public float speedMultiplier;
    public GameObject projectile;
}

[Serializable]
public struct Pattern{
    public Group[] groups;
    public float HPValueEnd;
}

