using UnityEngine;

public class BossEvents : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Health>().healthChange.AddListener(OnDeath);
        GetComponent<Health>().healthChange.AddListener(BossHealthBarUI.instance.UpdateBossHealthBar);
    }

    private void OnDisable()
    {
        GetComponent<Health>().healthChange.RemoveListener(OnDeath);
        GetComponent<Health>().healthChange.RemoveListener(BossHealthBarUI.instance.UpdateBossHealthBar);
    }

    public void OnDeath(float currHealth, float maxHealth)
    {
        if (currHealth <= 0)
        {
            LevelManager.BossDefeated = true;
        }
    }
}