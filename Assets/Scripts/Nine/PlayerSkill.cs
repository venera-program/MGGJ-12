using System.Collections;
using UnityEngine;
public class PlayerSkill : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private AnimatorOverrideController mainAnimator;
    [SerializeField] private AnimatorOverrideController skillOverride;
    [SerializeField] private AnimatorOverrideController flickerOverride;
    [SerializeField] private float flickerDelay;

    private Coroutine _flickeringRoutine;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController = mainAnimator;
    }

    private void OnEnable()
    {
        LevelManager.OnLevelUnload += SkillIsOver;
    }

    private void OnDisable()
    {
        LevelManager.OnLevelUnload -= SkillIsOver;
    }

    // Player Controller script will call this function to switch to our skill override
    public void SwitchToSkillAnimation()
    {
        animator.runtimeAnimatorController = skillOverride;
        _flickeringRoutine = StartCoroutine(SkillIsEnding(flickerDelay));
    }

    //Coroutine that will switch to the MC_Time_Out_Override after a set amount of time
    private IEnumerator SkillIsEnding(float waitForIt)
    {
        yield return new WaitForSeconds(waitForIt);
        animator.runtimeAnimatorController = flickerOverride;
    }

    //Player Controller Script will call this when the skill is over to switch back to our MC_Animation_Controller
    public void SkillIsOver()
    {
        if (_flickeringRoutine != null)
        {
            StopCoroutine(_flickeringRoutine);
            _flickeringRoutine = null;
        }

        animator.runtimeAnimatorController = mainAnimator;
        Debug.Log("the skill definitely should have stopped by now");
    }
}
