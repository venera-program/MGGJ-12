using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //FOR DEBUGGING PURPOSES
    [Header("For debugging purposes")]
    public bool editmode = false;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        switch(movementType){
            case EnemyMovementType.Floaty:
            nextDestination = MovementPatternCalculation.CalculateFloatyPosition(transform.position, movementDistance, topBorder, bottomBorder, leftBorder, rightBorder);
                break;
            default:
            nextDestination = Vector3.zero;
                break;
        }
    }

    void FixedUpdate(){
        if (isMoving){
            StartMoving(movementType);
        }
    }

    private void StartMoving(EnemyMovementType type){
        switch(type){
            case EnemyMovementType.Floaty:
            HandleFloatyMovement();
                break;
            case EnemyMovementType.Directed:
            HandleDirectedMovement();
                break;
            default: 
                break;
        }
    }

    private void HandleFloatyMovement(){
        if(HelperFunctions.IsAtPosition(transform.position, nextDestination, accuracy)){
            nextDestination = MovementPatternCalculation.CalculateFloatyPosition(rb.position, movementDistance, topBorder, bottomBorder, leftBorder, rightBorder);
        }
        Vector2 direction = ((Vector2)nextDestination - rb.position).normalized;
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * direction); 
    }

    private void HandleDirectedMovement(){

    }

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
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
    Directed
}
