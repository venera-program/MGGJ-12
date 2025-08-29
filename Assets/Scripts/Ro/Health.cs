using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float currHealth;
    public UnityEvent<float, float> healthChange = new UnityEvent<float, float>();

    private Coroutine _IFrameRoutine;

    [Header("For Debugging Purposes")]
    [SerializeField] private bool isInvincible;

    void Awake()
    {
        currHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (_IFrameRoutine != null)
        {
            Debug.Log("trying to take damage when immune", this);
            return;
        }
        if(isInvincible) return; 

        currHealth = Mathf.Clamp(currHealth - damage, 0f, maxHealth);
        Debug.Log($"{transform.name} took {damage} damage");
        healthChange.Invoke(currHealth, maxHealth);
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

    public void TriggerIFrames(int frameCount)
    {
        if (_IFrameRoutine != null)
        {
            StopCoroutine(_IFrameRoutine);
        }

        _IFrameRoutine = StartCoroutine(CountIFrames(frameCount));
    }

    public IEnumerator CountIFrames(int _TargetIFrame)
    {
        for (int i = 0; i < _TargetIFrame; i++)
        {
            // TODO: graphic indicating iframes active
            yield return null;
        }

        _IFrameRoutine = null;
    }
}