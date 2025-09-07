using UnityEngine;
public class ProjectileDamage : DealDamage
{
    [SerializeField] private Projectile script;
    public override void OnContact()
    {
        if (tag == "EnemyProjectile")
        {
            Graze.instance.RemoveGrazeCount(gameObject.GetInstanceID());
        }
        ProjectilePool.instance.DeactivateProjectile(script);
    }
}
