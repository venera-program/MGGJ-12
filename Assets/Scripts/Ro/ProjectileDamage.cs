using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ProjectileDamage : DealDamage
{
    [SerializeField] private Projectile script;
        public override void OnContact(){
             ProjectilePool.instance.DeactivateProjectile(script);
        }
}
