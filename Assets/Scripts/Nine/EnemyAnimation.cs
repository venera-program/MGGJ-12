using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{

    private Animator animator;
    [SerializeField] private float pukeTime;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void pukeBullets()
    {
        if (!(animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking")))
        {
            animator.Play("Attacking");
            StartCoroutine(endPukeBullets(pukeTime));
        }
    }

    private IEnumerator endPukeBullets(float duration)
    {
        yield return new WaitForSeconds(duration);
        animator.Play("Moving");
    }
}
