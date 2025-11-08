using UnityEngine;

[System.Serializable]
public class WindSlashAbility : AbilityBase
{
    [SerializeField] public GameObject windSlashProjectile;

    public WindSlashAbility(GameObject player, GameObject windSlashProjectile) : base(player, new Timer(0.75f))
    {
        this.windSlashProjectile = windSlashProjectile;
    }

    public override void Ability()
    {
        if (CanUseAbility())
        {
            GameObject projectile = Instantiate(windSlashProjectile);
            projectile.transform.position = player.transform.position;
            projectile.GetComponent<WindSlashProjectile>().FacingRight = player.GetComponent<PlayerManager>().facingRight;

            abilityCooldown.start();
        }
    }

}
