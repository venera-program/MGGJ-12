using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public abstract class DealDamage : MonoBehaviour
{
    public float damage; 
    public string[] tagsToPayAttentionTo;
    void OnTriggerEnter2D(Collider2D other){
        for(int i = 0 ; i < tagsToPayAttentionTo.Length ; i++){
            if (other.tag == tagsToPayAttentionTo[i]){
                other.GetComponent<Health>()?.TakeDamage(damage);
                OnContact();
            }
        }
    }

    public abstract void OnContact();
}
