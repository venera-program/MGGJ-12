using System.Collections;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private float pukeTime;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PukeBullets()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
        {
            animator.Play("Attacking");
            StartCoroutine(EndPukeBullets(pukeTime));
        }
    }

    private IEnumerator EndPukeBullets(float duration)
    {
        yield return new WaitForSeconds(duration);
        animator.Play("Moving");
    }
}
