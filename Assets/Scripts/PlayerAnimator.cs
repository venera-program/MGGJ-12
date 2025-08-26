using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField]
    private Health healthTracker;

    private Animator animator;

    private const string IS_MOVING = "isMoving";
    private const string MOVING_LEFT = "movingLeft";
    private const string WAS_HIT = "wasHit";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        healthTracker.healthChange.AddListener(OnHit);
    }

    private void OnEnable()
    {
        PlayerControllerScript.instance.controller.Main.Move.performed += UpdateAnimations;
    }

    private void OnDisable()
    {
        PlayerControllerScript.instance.controller.Main.Move.performed -= UpdateAnimations;
    }

    private void UpdateAnimations(UnityEngine.InputSystem.InputAction.CallbackContext cont)
    {
        animator.SetBool(IS_MOVING, PlayerControllerScript.instance.PlayerInputRaw != Vector2.zero);
        animator.SetBool(MOVING_LEFT, PlayerControllerScript.instance.PlayerInputRaw.x < 0);
    }

    // Triggering wasHit=false will transition out of the wasHit animation (via controller)
    private void OnHit(float currHealth, float maxHealth)
    {
        animator.SetBool(WAS_HIT, currHealth != maxHealth);
    }
}