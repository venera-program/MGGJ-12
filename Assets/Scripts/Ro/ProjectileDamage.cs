using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ProjectileDamage : DealDamage
{
        public override void OnContact(){
            Destroy(gameObject);
        }
}
