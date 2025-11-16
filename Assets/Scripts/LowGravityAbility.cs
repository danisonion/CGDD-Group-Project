using UnityEngine;

public class LowGravityAbility : AbilityBase
{
    private bool gravityToggle;

    public float gravityChange = 0.5f;
    private float initialGravityScale;
    private ControllerData coldController;

    public LowGravityAbility(GameObject player) : base(player, new Timer(0.75f))
    {
        gravityToggle = false;
        coldController = player.GetComponent<PlayerManager>().cControllerData;
        initialGravityScale = coldController.gravityMultiplier;
    }

    public override void Ability()
    {
        if (CanUseAbility())
        {
            if (!gravityToggle)
            {
                coldController.gravityMultiplier = gravityChange;
                gravityToggle = true;

                abilityCooldown.start();
            }
            else
            {
                coldController.gravityMultiplier = initialGravityScale;
                gravityToggle = false;

                abilityCooldown.start();
            }
        }
    }
}
