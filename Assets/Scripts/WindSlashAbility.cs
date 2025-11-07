using System.Collections.Generic;
using System.Linq;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class WindSlashAbility : AbilityBase
{
    public const float initialForce = 4;
    public const float slowDownRate = -0.2f;

    public GameObject windSlashProjectile;
    private List<GameObject> projectileInstances;

    public WindSlashAbility(GameObject player) : base(player, new Timer(0.75f))
    {
        projectileInstances = new List<GameObject>();
    }

    public override void Ability()
    {
        if (CanUseAbility())
        {
            GameObject projectile = Instantiate(windSlashProjectile, player.transform);
            Rigidbody2D proj_RB = projectile.GetComponent<Rigidbody2D>();
            proj_RB.gravityScale = 0;
            proj_RB.AddForceX(initialForce);
            projectileInstances.Add(projectile);

            abilityCooldown.start();
        }
    }

    public new void AbilityOnFrame()
    {
        // Not super efficient, but it'll get the work done.
        projectileInstances = projectileInstances.Where(x => x.GetComponent<Rigidbody2D>().linearVelocityX >= 0).ToList();
        foreach (var instance in projectileInstances)
        {
            instance.GetComponent<Rigidbody2D>().AddForceX(slowDownRate);
        }
    }

}
