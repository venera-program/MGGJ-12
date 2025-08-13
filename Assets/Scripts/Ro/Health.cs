using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour{
    public float maxHealth;
    public float currHealth;
    public UnityEvent healthChange = new UnityEvent();

    void Awake(){
        currHealth = maxHealth;
    }
    public void TakeDamage(float damage){
        currHealth = Mathf.Clamp(currHealth - damage, 0f, maxHealth);
        Debug.Log($"{transform.name} took {damage} damage");
    }

    public void Heal(float health){
        currHealth = Mathf.Clamp(currHealth + health, 0f, maxHealth);
    }

    public void FullHeal(){
        currHealth = maxHealth;
    }

}
