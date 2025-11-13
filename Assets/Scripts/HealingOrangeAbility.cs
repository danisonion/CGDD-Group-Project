using UnityEngine;

public class HealingOrangeAbility : AbilityBase
{
    public GameObject healingOrb;

    public HealingOrangeAbility(GameObject player, GameObject healingOrb) : base(player, new Timer(0.75f))
    {
        this.healingOrb = healingOrb;
    }

    public override void Ability()
    {
        if (CanUseAbility())
        {
            GameObject orb = Instantiate(healingOrb);
            orb.transform.position = player.transform.position + new Vector3(3, 3, 0);
            orb.GetComponent<HealingOrbController>().player = player;

            abilityCooldown.start();
        }
    }

}
