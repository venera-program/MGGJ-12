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
        transform.parent.GetComponent<Health>().healthChange.AddListener(ProgressPattern);
    }

    // percentage of health
    void Start(){
        groupSpawner.StartGroup(Patterns[index].groups);
    }
    
    private void ProgressPattern(float currHealth, float maxHealth){
        if(Patterns[index].HPValueEnd >= ((currHealth/maxHealth) * 100f)){
            index++;
            if(index >= Patterns.Length){
                index = 0;
            }
            groupSpawner.StartGroup(Patterns[index].groups);
        }
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
}

[Serializable]
public struct Pattern{
    public Group[] groups;
    [Tooltip("Percentage. Determines when the current pattern ends")]
    public float HPValueEnd;
}

