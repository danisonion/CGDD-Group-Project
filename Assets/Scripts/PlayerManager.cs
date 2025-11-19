using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerManager : EntityManager
{
    [Header("Components")]
    // I need this in an external script btw
    [SerializeField] public ControllerData wControllerData;
    [SerializeField] public ControllerData cControllerData;

    [SerializeField] private BoxCollider2D AttackHurtBox;
    public BoxCollider2D PogoSurfBox;

    [Header("Ability Components")]
    public float pogoMultiplier;
    [SerializeField] private GameObject windSlashProjectile;
    [SerializeField] private GameObject healingOrb;

    public enum PlayerForm
    {
        Warm,
        Cool
    }

    [Header("Current Form")]
    public PlayerForm currentForm;

    private bool isAttacking = false;
    private bool pogoSurfable;

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

        animator = GetComponent<Animator>();

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
        AttackHurtBox.GetComponent<AttackBox>().damage = entityController.controllerData.baseAttackDamage;
        PogoSurfBox.size = new Vector2(entityController.sizeX, PogoSurfBox.size.y);
        //PogoSurfBox.transform.localPosition = new Vector3(0, -(entityController.sizeY/2),4498);
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
        if (AttackHurtBox.enabled && attackDuration > entityController.controllerData.baseAttackDuration && !(pogoSurfable && inputY < -0.1f))
        {
            AttackHurtBox.enabled = false;
        }
        else if (attackDuration < entityController.controllerData.baseAttackDuration)
        {
            attackDuration += Time.deltaTime;
        }
        
        if(attackCooldown < entityController.controllerData.baseAttackCooldown)
        {
            attackCooldown += Time.deltaTime;
        }

        
        PogoSurfBox.enabled = pogoSurfable && inputY < -0.1f;
        if(pogoSurfable && inputY < -0.1f && isAttacking) AttackHurtBox.enabled = true;
        
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
            AttackHurtBox.GetComponent<AttackBox>().damage = entityController.controllerData.baseAttackDamage;
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

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isAttacking = true;
            if (attackCooldown < entityController.controllerData.baseAttackCooldown) return;
            Debug.Log("ATTACK!");
            attackDuration = 0;
            attackCooldown = 0;
            AttackHurtBox.enabled = true;
            pogoSurfable = true;
            
        }
        else if (context.canceled)
        {
            pogoSurfable = false;
            isAttacking = false;
        }
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
        
        
        // Intentionally doesn't cover all cases. Player needs to stay the direction they are facing
        
        if (inputX < 0)
        {
            facingRight = false;
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (inputX > 0)
        {
            facingRight = true;
            GetComponent<SpriteRenderer>().flipX = false;
        }

        animator.SetBool("isWalking", true);
        
        if(inputY > 0.1f)
        {
            AttackHurtBox.transform.localPosition = new Vector3(0,1f,0);
        }
        else if(inputY < -0.1f)
        {
            AttackHurtBox.transform.localPosition = new Vector3(0,-1f,0);
        } else if (!isAttacking)
        {
            AttackHurtBox.transform.localPosition = new Vector3(facingRight?1.5f:-1.5f,0,0);
        }

    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed) entityController.Jump();
    }
    public void Jump(bool ignoreGrounded = false, float powerMultiplier = 1)
    {
        entityController.Jump(ignoreGrounded, powerMultiplier);
    }

}
