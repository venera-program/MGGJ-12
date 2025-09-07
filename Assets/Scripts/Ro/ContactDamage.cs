using UnityEngine;

public class ContactDamage : DealDamage
{


    void OnCollisionEnter2D(Collision2D other)
    {
        if (isOnCollision)
        {
            for (int i = 0; i < tagsToPayAttentionTo.Length; i++)
            {
                if (other.gameObject.tag == tagsToPayAttentionTo[i])
                {
                    other.gameObject.GetComponent<Health>()?.TakeDamage(damage);
                    OnContact();
                }
            }
        }
    }
    public override void OnContact()
    {

    }


}