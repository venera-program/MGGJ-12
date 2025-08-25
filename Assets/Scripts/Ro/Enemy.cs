using UnityEngine;
using MGGJ25.Shared;

[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    public float score;

    void Awake()
    {
        GetComponent<Health>().healthChange.AddListener(OnDeath);
    }

    public void OnDeath(float currHealth, float maxHealth)
    {
        if (currHealth <= 0)
        {
            AudioManager.Instance.PlayEnemyDies_SFX();
            PlayerData.UpdateScore(score);
            Destroy(gameObject);
        }
    }
}