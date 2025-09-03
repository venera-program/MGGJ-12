using UnityEngine;

public class BossEvents : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Health>().healthChange.AddListener(OnDeath);
    }

    public void OnDeath(float currHealth, float maxHealth)
    {
        if (currHealth <= 0)
        {
            LevelManager.BossDefeated = true;
        }
    }
}