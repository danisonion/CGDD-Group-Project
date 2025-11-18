using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerManager : EntityManager
{
    [Header("Components")]
    // I need this in an external script btw
    [SerializeField] public ControllerData wControllerData;
    [SerializeField] public ControllerData cControllerData;

    [Header("Ability Components")]
    [SerializeField] private GameObject windSlashProjectile;
    [SerializeField] private GameObject healingOrb;

    public enum PlayerForm
    {
        Warm,
        Cool
    }

    [Header("Current Form")]
    public PlayerForm currentForm;


    [System.Serializable]
    public struct Health
    {
        public float Hp;
        public float MaxHp;
    }

    [Header("Health & stats")]
    // Just putting this here for future reference (we still don't have a health mechanic lol)
    public Health health;


    [field: NonSerialized] public bool facingRight = true;

    private Animator animator;

    private void Awake()
    {
        // THIS IS JUST FOR THE DEMO, MAKE THIS BETTER EVENTUALLY!!!!
        health = new Health
        {
            Hp = 20,
            MaxHp = 20
        };

        cControllerData.abilities = new AbilityBase[] {
            new WindSlashAbility(gameObject, windSlashProjectile),
            new LowGravityAbility(gameObject),
        };
        wControllerData.abilities = new AbilityBase[] {
            new AstralShieldAbility(gameObject, new Timer(0)),
            new HealingOrangeAbility(gameObject, healingOrb),
        };
        if (currentForm == PlayerForm.Warm)
        {
            entityController.controllerData = wControllerData;
        }
        else
        {
            entityController.controllerData = cControllerData;
        }
    }

    public void Update()
    {
        foreach (var ability in wControllerData.abilities)
        {
            ability.AbilityOnFrame();
        }

        if (inputX == 0)
        {
            animator.SetBool("isWalking", false);
        }
    }

    void OnDestroy()
    {
        cControllerData.gravityMultiplier = 1;
    }

    //Input System --> Player --> SwapSides --> Keyboard F
    public void SwapSides(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ToggleForm();
            Debug.Log("Form Switched");
        }
    }

    public void ToggleForm()
    {
        if (currentForm == PlayerForm.Warm)
        {
            currentForm = PlayerForm.Cool;
            entityController.controllerData = cControllerData;
            animator.SetBool("isLight", false);
        }
        else
        {
            currentForm = PlayerForm.Warm;
            entityController.controllerData = wControllerData;
            animator.SetBool("isLight", true);
        }
        Debug.Log("Current Form: " + currentForm);

    }

    // General function to use different abilities defined in the controllerData variable
    // WARNING: Please, do not under any circumstances change the name of the inputAction or I will cry
    // Matching it by strings probably isn't the best idea, but it's the only one I have
    public void UseAbility(InputAction.CallbackContext context)
    {
        animator.SetTrigger("UseAbility");
        switch (context.action.name)
        {
            case "Ability1":
                entityController.controllerData.abilities[0].Ability();
                break;
            case "Ability2":
                entityController.controllerData.abilities[1].Ability();
                break;
            case "Ability3":
                entityController.controllerData.abilities[2].Ability();
                break;
        }
    }

    //Implemented a central move function that only this script reads the inputs for
    //If both scripts have a Move() method, then an issue arises when the player switches whilst moving
    public void Move(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        inputX = value.x;
        inputY = value.y;

        // Intentionally doesn't cover all cases. Player needs to stay the direction they are facing when
        // they come to rest
        if (inputX < 0)
        {
            facingRight = false;
        }
        else if (inputX > 0)
        {
            facingRight = true;
        }

        animator.SetBool("isWalking", true);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed) entityController.Jump();
    }

}
