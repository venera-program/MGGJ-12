using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ProjectileDamage : DealDamage
{
    [SerializeField] private Projectile script;
        public override void OnContact(){
             if(tag == "EnemyProjectile"){
                Graze.instance.RemoveGrazeCount(gameObject.GetInstanceID());
            }
             ProjectilePool.instance.DeactivateProjectile(script);
        }
}
