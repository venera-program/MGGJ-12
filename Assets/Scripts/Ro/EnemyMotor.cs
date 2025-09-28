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

    private bool flipped;
    private Image enemyImage;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        imagebounds = GetComponentInChildren<Image>().sprite.rect;
        enemyImage = GetComponentInChildren<Image>();
    }

    void Start()
    {
        switch (movementType)
        {
            case EnemyMovementType.Floaty:
                nextDestination = MovementPatternCalculation.CalculateFloatyPosition(transform.position, movementDistance, topBorder, bottomBorder, leftBorder, rightBorder);
                PrintFloatyDestination(nextDestination);
                DebugEnemyMotorPositions.instance.SetFloatyPositionMarker(gameObject.GetInstanceID(), nextDestination);
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

    void FixedUpdate()
    {
        if (isMoving)
        {
            StartMoving(movementType);
        }
    }

    private void StartMoving(EnemyMovementType type)
    {
        switch (type)
        {
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

    private void HandleFloatyMovement()
    {
        if (HelperFunctions.IsAtPosition(rb.position, nextDestination, accuracy))
        {
            nextDestination = MovementPatternCalculation.CalculateFloatyPosition(rb.position, movementDistance, topBorder, bottomBorder, leftBorder, rightBorder);
            PrintFloatyDestination(nextDestination);
            DebugEnemyMotorPositions.instance.SetFloatyPositionMarker(gameObject.GetInstanceID(), nextDestination);
        }
        Vector2 direction = ((Vector2)nextDestination - rb.position).normalized;
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * direction);
        flipped = direction.x < 0;
        transform.rotation = Quaternion.Euler(new Vector3(0f, flipped ? 180 : 0f, 0f));
    }

    private void HandleDirectedScreenMovement()
    {
        if (HelperFunctions.IsAtPosition(rb.position, nextDestination, accuracy))
        {
            Destroy(gameObject);
        }
        Vector2 direction = ((Vector2)nextDestination - rb.position).normalized;
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * direction);
        flipped = direction.x < 0;
        enemyImage.transform.rotation = Quaternion.Euler(new Vector3(0f, flipped ? 180 : 0f, 0f));
    }

    private void HandleDirectedPlayerMovement()
    {
        updateTargetTimer += Time.fixedDeltaTime;
        if (HelperFunctions.IsAtPosition(rb.position, nextDestination, accuracy) || updateTargetTimer >= updateTargetInterval)
        {
            updateTargetTimer = 0;
            nextDestination = MovementPatternCalculation.CalculateDirectedPlayerPosition(rb.position);
        }
        Vector2 direction = ((Vector2)nextDestination - rb.position).normalized;
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * direction);
        flipped = direction.x < 0;
        enemyImage.transform.rotation = Quaternion.Euler(new Vector3(0f, flipped ? 180 : 0f, 0f));
    }

    private void HandleDirectedBossMovement()
    {
        if (HelperFunctions.IsAtPosition(rb.position, nextDestination, bossDistance))
        {
            nextDestination = MovementPatternCalculation.CalculateDirectedBossPosition(rb.position, bossDistance);
        }
        else
        {
            Vector2 direction = ((Vector2)nextDestination - rb.position).normalized;
            rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * direction);
            flipped = direction.x < 0;
            enemyImage.transform.rotation = Quaternion.Euler(new Vector3(0f, flipped ? 180 : 0f, 0f));
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(nextDestination, .1f);
        Gizmos.DrawLine(transform.position, nextDestination);

        if (editmode)
        {
            Vector3[] screenPoints = HelperFunctions.ScreenCorners(topBorder, bottomBorder, leftBorder, rightBorder);
            for (int i = 0; i < 4; i++)
            {
                Gizmos.DrawSphere(screenPoints[i], .1f);
            }
            Gizmos.DrawLineList(screenPoints);

            if(movementType == EnemyMovementType.Floaty){
                Gizmos.color = Color.green;
                Vector3 currPosition = transform.position;

                Vector3 right = currPosition + new Vector3(movementDistance, 0f, 0f);
                Vector3 upRight = currPosition + (movementDistance * new Vector3(Mathf.Cos(45f * Mathf.Deg2Rad), Mathf.Sin(45f * Mathf.Deg2Rad), 0f));
                Vector3 up = currPosition + new Vector3(0f, movementDistance,0f);
                Vector3 upLeft = currPosition + (movementDistance * new Vector3(Mathf.Cos(135f * Mathf.Deg2Rad), Mathf.Sin(135f * Mathf.Deg2Rad), 0f));
                Vector3 left = currPosition + new Vector3(-movementDistance, 0f, 0f);
                Vector3 downLeft = currPosition + (movementDistance * new Vector3(Mathf.Cos(225f * Mathf.Deg2Rad), Mathf.Sin(225f * Mathf.Deg2Rad), 0f));
                Vector3 down = currPosition + new Vector3(0f, -movementDistance, 0f);
                Vector3 downRight = currPosition + (movementDistance * new Vector3(Mathf.Cos(315f * Mathf.Deg2Rad), Mathf.Sin(315f * Mathf.Deg2Rad), 0f));

                Gizmos.DrawLine(currPosition, right);
                Gizmos.DrawLine(currPosition, upRight);
                Gizmos.DrawLine(currPosition, up);
                Gizmos.DrawLine(currPosition, upLeft);
                Gizmos.DrawLine(currPosition, left);
                Gizmos.DrawLine(currPosition, downLeft);
                Gizmos.DrawLine(currPosition, down);
                Gizmos.DrawLine(currPosition, downRight);

            }
        }
    }

    private void PrintFloatyDestination(Vector3 position){
        #if UNITY_EDITOR || DEVELOPMENT_BUILD
        Debug.Log($"Current Floaty Destination for {gameObject.name} : {position.ToString()}");
        #endif
    }
    void OnDestroy()
    {
        #if UNITY_EDITOR || DEVELOPMENT_BUILD
        DebugEnemyMotorPositions.RemoveFloatyPositionMarker(gameObject.GetInstanceID());
        #endif
    }
}

public enum EnemyMovementType
{
    Floaty,
    DirectedScreen,
    DirectedPlayer,
    DirectedBoss
}
