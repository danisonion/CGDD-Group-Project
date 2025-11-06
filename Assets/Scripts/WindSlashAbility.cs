using UnityEngine;

public class NewMonoBehaviourScript : AbilityBase
{
    public GameObject windSlashProjectile;

    public override void ability(GameObject player)
    {
        if (canUseAbility())
        {
            GameObject projectile = Instantiate(windSlashProjectile, player.transform);
            abilityCooldown.start();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        abilityCooldown = new Timer(0.75f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
