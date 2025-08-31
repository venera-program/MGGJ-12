using UnityEngine;
using MGGJ25.Shared;

[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    public float score;
    private Animator animator;
    [SerializeField] private float deathDelay;
    private Collider2D collider;

    void Awake()
    {
        GetComponent<Health>().healthChange.AddListener(OnDeath);
        animator = this.GetComponentInChildren<Animator>();
        collider = GetComponent<Collider2D>();
    }

    public void OnDeath(float currHealth, float maxHealth)
    {
        if (currHealth <= 0)
        {
            collider.enabled = false;
            AudioManager.Instance.PlayEnemyDies_SFX();
            PlayerData.UpdateScore(score);
            animator.SetBool("isDead", true);
            Destroy(gameObject, deathDelay);
        }
    }
}