using System.Collections;
using UnityEngine;
public class PlayerSkill : MonoBehaviour
{
    private Animator animator;
    private Animator _default;
    [SerializeField] private AnimatorOverrideController mainAnimator;
    [SerializeField] private AnimatorOverrideController skillOverride;
    [SerializeField] private AnimatorOverrideController flickerOverride;
    [SerializeField] private float flickerDelay;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController = mainAnimator;
    }

    // Player Controller script will call this function to switch to our skill override
    public void switchToSkillAnimation()
    {
        animator.runtimeAnimatorController = skillOverride;
        StartCoroutine(skillIsEnding(flickerDelay));
    }

    //Coroutine that will switch to the MC_Time_Out_Override after a set amount of time
    private IEnumerator skillIsEnding(float waitForIt)
    {
        yield return new WaitForSeconds(waitForIt);
        animator.runtimeAnimatorController = flickerOverride;
    }

    //Player Controller Script will call this when the skill is over to switch back to our MC_Animation_Controller
    public void skillIsOver()
    {
        animator.runtimeAnimatorController = mainAnimator;
        Debug.Log("the skill definitely should have stopped by now");
    }
}
