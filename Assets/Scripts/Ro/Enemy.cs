using UnityEngine;
using MGGJ25.Shared;

[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    public float score;
    private Animator animator;
    [SerializeField] private float deathDelay;
    private Collider2D collider;
    private EnemyMotor script;

    void Awake()
    {
        GetComponent<Health>().healthChange.AddListener(OnDeath);
        animator = this.GetComponentInChildren<Animator>();
        collider = GetComponent<Collider2D>();
        script = GetComponent<EnemyMotor>();
    }

    public void OnDeath(float currHealth, float maxHealth)
    {
        if (currHealth <= 0)
        {
            collider.enabled = false;
            script.speed = 0;
            AudioManager.Instance.PlayEnemyDies_SFX();
            animator.Play("Dead");
            PlayerData.UpdateScore(score);
            Destroy(gameObject, deathDelay);
        }
    }
}