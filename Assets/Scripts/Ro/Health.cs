using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using MGGJ25.Shared;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float currHealth;
    public UnityEvent<float, float> healthChange = new UnityEvent<float, float>();
    [SerializeField] private AudioClip hurtSFX;

    private Coroutine _IFrameRoutine;

    [Header("For Debugging Purposes")]
    [SerializeField] private bool isInvincible;

    void Awake()
    {
        FullHeal();
    }

    public void TakeDamage(float damage)
    {
        if (_IFrameRoutine != null)
        {
            Debug.Log("trying to take damage when immune", this);
            return;
        }
        if (isInvincible) return;

        currHealth = Mathf.Clamp(currHealth - damage, 0f, maxHealth);
        Debug.Log($"{transform.name} took {damage} damage");
        healthChange.Invoke(currHealth, maxHealth);
        // AudioManager.Instance.PlaySfx(hurtSFX);
    }

    public void Heal(float health)
    {
        currHealth = Mathf.Clamp(currHealth + health, 0f, maxHealth);
        healthChange.Invoke(currHealth, maxHealth);
    }

    public void FullHeal()
    {
        currHealth = maxHealth;
        healthChange.Invoke(currHealth, maxHealth);
    }

    [ContextMenu("FullHarm")]
    public void FullHarm()
    {
        currHealth = 0;
        healthChange.Invoke(currHealth, maxHealth);
    }

    public float GetCurrHealth(){
        return currHealth;
    }

    public float GetMaxHealth(){
        return maxHealth;
    }

    public void TriggerIFrames(float ISeconds)
    {
        StopIFrames();
        _IFrameRoutine = StartCoroutine(CountIFrames(ISeconds));
    }

    public void StopIFrames(){
       if (_IFrameRoutine != null)
        {
            StopCoroutine(_IFrameRoutine);
            _IFrameRoutine = null;
        }
    }

    public IEnumerator CountIFrames(float _TargetISeconds)
    {
        yield return new WaitForSeconds(_TargetISeconds);
        _IFrameRoutine = null;
    }
}