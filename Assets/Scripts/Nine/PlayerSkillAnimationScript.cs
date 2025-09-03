using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillAnimationScript : MonoBehaviour
{
    private Animator animator;
    private Animator _default;
    [SerializeField] private Animator mainAnimator;
    [SerializeField] private Animator skillOverride;
    [SerializeField] private Animator flickerOverride;
    [SerializeField] private float flickerDelay;

    void Awake()
    {
        animator = mainAnimator;
    }

    // Player Controller script will call this function to switch to our skill override
    private void switchToSkillAnimation()
    {
        animator = skillOverride;
        StartCoroutine(skillIsEnding(flickerDelay));
    }

    //Coroutine that will switch to the MC_Time_Out_Override after a set amount of time
    private IEnumerator skillIsEnding(float waitForIt)
    {
        yield return new WaitForSeconds(waitForIt);
        animator = flickerOverride;
    }

    //Player Controller Script will call this when the skill is over to switch back to our MC_Animation_Controller
    private void skillIsOver()
    {
        animator = mainAnimator;
    }
}
