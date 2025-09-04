using UnityEngine;

public class BossEvents : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Health>().healthChange.AddListener(OnDeath);
        GetComponent<Health>().healthChange.AddListener(BossHealthBarUI.instance.UpdateBossHealthBar);
    }

    public void OnDeath(float currHealth, float maxHealth)
    {
        if (currHealth <= 0)
        {
            LevelManager.BossDefeated = true;
        }
    }
}