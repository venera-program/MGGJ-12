using UnityEngine;
using MGGJ25.Shared;

[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    public float score;
    private Animator animator;

    void Awake()
    {
        GetComponent<Health>().healthChange.AddListener(OnDeath);
        animator = this.GetComponentInChildren<Animator>();
    }

    public void OnDeath(float currHealth, float maxHealth)
    {
        if (currHealth <= 0)
        {
            AudioManager.Instance.PlayEnemyDies_SFX();
            PlayerData.UpdateScore(score);
            animator.SetBool("isDead", true);
            Destroy(gameObject, 0.5f);
        }
    }
}