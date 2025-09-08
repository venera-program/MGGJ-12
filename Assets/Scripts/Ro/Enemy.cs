using UnityEngine;
using MGGJ25.Shared;

[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    public float score;
    [SerializeField] private float deathDelay;

    private Animator _animator;
    private Collider2D _collider;

    void Awake()
    {
        GetComponent<Health>().healthChange.AddListener(OnDeath);
        _animator = GetComponentInChildren<Animator>();
        _collider = GetComponent<Collider2D>();
    }

    public void OnDeath(float currHealth, float maxHealth)
    {
        if (currHealth <= 0)
        {
            _collider.enabled = false;
            AudioManager.Instance.PlayEnemyDies_SFX();
            _animator.Play("Dead");
            PlayerData.UpdateScore(score);
            Destroy(gameObject, deathDelay);
        }
    }
}