using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFormManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private WarmPlayerController wPlayerController;
    [SerializeField] private CoolPlayerController cPlayerController;
    [SerializeField] private PlayerInput playerInput;

    public float inputX { get; set; }
    public float inputY { get; set; }
    public enum PlayerForm
    {
        Warm,
        Cool
    }

    [Header("Current Form")]
    [SerializeField] PlayerForm currentForm;

    private void Awake()
    {
        wPlayerController = GetComponent<WarmPlayerController>();
        cPlayerController = GetComponent<CoolPlayerController>();
        playerInput = GetComponent<PlayerInput>();

        if (currentForm == PlayerForm.Warm)
        {
            wPlayerController.enabled = true;
            cPlayerController.enabled = false;
            SubscribeWarm();
        }
        else
        {
            cPlayerController.enabled = true;
            wPlayerController.enabled = false;
            SubscribeCool();
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
            if (wPlayerController != null && cPlayerController != null)
            {
                UnsubscribeWarm();
                wPlayerController.enabled = false;

                cPlayerController.enabled = true;
                SubscribeCool();
            }
        }
        else
        {
            if (wPlayerController != null && cPlayerController != null)
            {
                UnsubscribeCool();
                cPlayerController.enabled = false;

                wPlayerController.enabled = true;
                SubscribeWarm();
            }
        }
        currentForm = (currentForm == PlayerForm.Warm) ? PlayerForm.Cool : PlayerForm.Warm;
        Debug.Log("Current Form: " + currentForm);

    }

    //Implemented a central move function that only this script reads the inputs for
    //If both scripts have a Move() method, then an issue arises when the player switches whilst moving
    public void Move(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        inputX = value.x;
        inputY = value.y;

    }

    #region Subscriptions
    //Alters the Input System --> Events --> Player , and ensures that the correct Jump() method are being called during runtime.
    //Doing this there isn't a need to drag and drop for Jump(), and this better allows for two seperate control schemes
    private void SubscribeWarm()
    {
        if (playerInput != null)
            playerInput.actions["Jump"].performed += wPlayerController.Jump;
    }
    private void UnsubscribeWarm()
    {
        if (playerInput != null)
        playerInput.actions["Jump"].performed -= wPlayerController.Jump;
    }
    private void SubscribeCool()
    {
        if (playerInput != null)
        playerInput.actions["Jump"].performed += cPlayerController.Jump;
    }
    private void UnsubscribeCool()
    {
        if (playerInput != null)
        playerInput.actions["Jump"].performed -= cPlayerController.Jump;
    }
    #endregion
}
