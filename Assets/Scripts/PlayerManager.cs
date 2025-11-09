using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerManager : EntityManager
{
    [Header("Components")]
    [SerializeField] private ControllerData wControllerData;
    [SerializeField] private ControllerData cControllerData;

    [Header("Ability Components")]
    [SerializeField] private GameObject windSlashProjectile;
    

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

    private void Awake()
    {
        wControllerData.abilities = new AbilityBase[] { new WindSlashAbility(gameObject, windSlashProjectile) };
        cControllerData.abilities = new AbilityBase[] {};
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
        }
        else
        {
            currentForm = PlayerForm.Warm;
            entityController.controllerData = wControllerData;
        }
        Debug.Log("Current Form: " + currentForm);

    }

    // General function to use different abilities defined in the controllerData variable
    // WARNING: Please, do not under any circumstances change the name of the inputAction or I will cry
    // Matching it by strings probably isn't the best idea, but it's the only one I have
    public void UseAbility(InputAction.CallbackContext context)
    {
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

        // Intentionally doesn't cover all cases. Player needs to stay the direction they are facing
        if (inputX < 0)
        {
            facingRight = false;
        }
        else if (inputX > 0)
        {
            facingRight = true;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed) entityController.Jump();
    }

}
