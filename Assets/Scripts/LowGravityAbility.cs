using UnityEngine;

public class LowGravityAbility : AbilityBase
{
    [SerializeField] public GameObject windSlashProjectile;

    private bool gravityToggle;

    public float gravityChange = 2;
    private float initialGravityScale;
    private Rigidbody2D rb;

    public LowGravityAbility(GameObject player) : base(player, new Timer(0.75f))
    {
        gravityToggle = false;
        rb = player.GetComponent<Rigidbody2D>();
        initialGravityScale = rb.gravityScale;
    }

    public override void Ability()
    {
        if (CanUseAbility())
        {
            if (!gravityToggle)
            {
                rb.gravityScale = gravityChange;
                gravityToggle = true;
            }
            else
            {
                rb.gravityScale = initialGravityScale;

                abilityCooldown.start();
            }
        }
    }

}
