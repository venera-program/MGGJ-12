using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private string[] affectedTags;
    [SerializeField] private bool isOnCollision;
    [SerializeField] private bool isOnTrigger;

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (isOnCollision)
        {
            for (int i = 0; i < affectedTags.Length; i++)
            {
                if (other.gameObject.CompareTag(affectedTags[i]))
                {
                    if (other.gameObject.TryGetComponent<Health>(out var health))
                    {
                        health.TakeDamage(damage);
                    }
                    OnContact();
                }
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (isOnTrigger)
        {
            for (int i = 0; i < affectedTags.Length; i++)
            {
                if (other.CompareTag(affectedTags[i]))
                {
                    if (other.TryGetComponent<Health>(out var health))
                    {
                        health.TakeDamage(damage);
                    }
                    OnContact();
                }
            }
        }
    }

    public virtual void OnContact() { }
}
