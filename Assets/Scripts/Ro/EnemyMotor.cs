using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMotor : MonoBehaviour
{
    [Header("Movement Type")]
    public EnemyMovementType movementType; 
    private Rigidbody2D rb; 

    [Header("Movement Settings")]
    [Tooltip("In unity units")]
    public float movementDistance;
    [Tooltip("Unity Units / seconds")]
    public float speed;
    [Tooltip("Determines how close an enemy needs to get to the destination before going to another destination")]
    public float accuracy;
    public bool isMoving;

    [Header("For Following Player Only")] 
    [Tooltip("Amount of time in seconds before the enemy updates their target position to where the player currently is")]
    public float updateTargetInterval = .1f;
    private float updateTargetTimer = 0f;

    [Header("For Following Boss Only")]
    public float bossDistance;

    
    [Header("Border padding")]
    [Tooltip("In pixels")]
    public float topBorder;
    [Tooltip("In pixels")]
    public float bottomBorder;
    [Tooltip("In pixels")]
    public float leftBorder;
    [Tooltip("In pixels")]
    public float rightBorder;
    [Tooltip("In pixels")]
    private Vector3 nextDestination;
    private Rect imagebounds;

    //FOR DEBUGGING PURPOSES
    [Header("For debugging purposes")]
    public bool editmode = false;

    //for Animating enemy sprites
    [Header("For animation purposes")]
    private Animator animator;
    private const string moving = "isMoving";
    private bool flipped;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        imagebounds = GetComponentInChildren<Image>().sprite.rect;
        animator = GetComponentInChildren<Animator>();
    }

    void Start(){
        switch(movementType){
            case EnemyMovementType.Floaty:
            nextDestination = MovementPatternCalculation.CalculateFloatyPosition(transform.position, movementDistance, topBorder, bottomBorder, leftBorder, rightBorder);
                break;
            case EnemyMovementType.DirectedScreen:
            nextDestination = MovementPatternCalculation.CalculateDirectedScreenPosition(transform.position, imagebounds);
                break;
            case EnemyMovementType.DirectedPlayer:
            nextDestination = MovementPatternCalculation.CalculateDirectedPlayerPosition(transform.position);
                break;
            case EnemyMovementType.DirectedBoss:
            nextDestination = MovementPatternCalculation.CalculateDirectedBossPosition(transform.position, bossDistance);
                break;
        }
    }

    void FixedUpdate(){
        if (isMoving){
            StartMoving(movementType);
            animator.SetBool(moving, true);
            Debug.Log("isMoving is " + isMoving);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    private void StartMoving(EnemyMovementType type){
        switch(type){
            case EnemyMovementType.Floaty:
            HandleFloatyMovement();
                break;
            case EnemyMovementType.DirectedScreen:
            HandleDirectedScreenMovement();
                break;
            case EnemyMovementType.DirectedPlayer:
            HandleDirectedPlayerMovement();
                break;
            case EnemyMovementType.DirectedBoss:
            HandleDirectedBossMovement();
                break;
            default: 
                break;
        }
    }

    private void HandleFloatyMovement(){
        if(HelperFunctions.IsAtPosition(rb.position, nextDestination, accuracy)){
            nextDestination = MovementPatternCalculation.CalculateFloatyPosition(rb.position, movementDistance, topBorder, bottomBorder, leftBorder, rightBorder);
        }
        Vector2 direction = ((Vector2)nextDestination - rb.position).normalized;
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * direction);
        flipped = direction.x < 0;
        this.transform.rotation = Quaternion.Euler(new Vector3(0f, flipped ? 180 : 0f, 0f));
    }

    private void HandleDirectedScreenMovement(){
        if(HelperFunctions.IsAtPosition(rb.position, nextDestination, accuracy)){
           isMoving = false;
        }
        Vector2 direction = ((Vector2)nextDestination - rb.position).normalized;
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * direction);
        flipped = direction.x < 0;
        this.transform.rotation = Quaternion.Euler(new Vector3(0f, flipped ? 180 : 0f, 0f));
    }

    private void HandleDirectedPlayerMovement(){
        updateTargetTimer += Time.fixedDeltaTime;
        if(HelperFunctions.IsAtPosition(rb.position, nextDestination, accuracy) || updateTargetTimer >= updateTargetInterval){
            updateTargetTimer = 0;
            nextDestination = MovementPatternCalculation.CalculateDirectedPlayerPosition(rb.position);
        }
        Vector2 direction = ((Vector2)nextDestination - rb.position).normalized;
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * direction);
        flipped = direction.x < 0;
        this.transform.rotation = Quaternion.Euler(new Vector3(0f, flipped ? 180 : 0f, 0f));
    }

    private void HandleDirectedBossMovement(){
        if(HelperFunctions.IsAtPosition(rb.position, nextDestination, bossDistance)){
            nextDestination = MovementPatternCalculation.CalculateDirectedBossPosition(rb.position, bossDistance);
        } else {
             Vector2 direction = ((Vector2)nextDestination - rb.position).normalized;
             rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * direction);
            flipped = direction.x < 0;
            this.transform.rotation = Quaternion.Euler(new Vector3(0f, flipped ? 180 : 0f, 0f));
        }
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(nextDestination,.1f);
        Gizmos.DrawLine(transform.position, nextDestination);
        
        if(editmode){
            Vector3[] screenPoints = HelperFunctions.ScreenCorners(topBorder, bottomBorder, leftBorder, rightBorder);
             for(int i = 0 ; i < 4; i++){
                Gizmos.DrawSphere(screenPoints[i], .1f);
            }
            Gizmos.DrawLineList(screenPoints);
        }        
       
    }
}


public enum EnemyMovementType {
    Floaty,
    DirectedScreen,
    DirectedPlayer,
    DirectedBoss
}
