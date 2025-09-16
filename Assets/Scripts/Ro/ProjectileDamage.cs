using UnityEngine;

[RequireComponent(typeof(Projectile))]
public class ProjectileDamage : DealDamage
{
    private Projectile script;

    private void Awake()
    {
        script = GetComponent<Projectile>();
    }

    public override void OnContact()
    {
        if (CompareTag("EnemyProjectile"))
        {
            Graze.instance.RemoveGrazeCount(gameObject.GetInstanceID());
        }
        ProjectilePool.instance.DeactivateProjectile(script);
    }
}
