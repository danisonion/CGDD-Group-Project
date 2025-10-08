using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class CoolPlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float baseSpeed;
    [SerializeField] private float jumpingPower;

    [Header("Grounding")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;

    private bool canDoubleJump;

    [Header("Crouching")]
    [SerializeField] private float descensionMultiplier;
    [Range(0, 1)]
    [SerializeField] private float crouchingHeight = 0.6f;

    [Header("Player Component Refrences")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D collider2d;

    private float baseGravityScale;
    private PlayerFormManager formManager;

    private void Awake()
    {
        baseGravityScale = rb.gravityScale;
        formManager = GetComponent<PlayerFormManager>();
    }



    private void FixedUpdate()
    {
        if (IsGrounded())
        {
            canDoubleJump = true;
        }

        //Calls for the input from formManager rather being hosted in own script
        rb.linearVelocity = new Vector2(IsGrounded() ? formManager.inputX * baseSpeed :
        Mathf.LerpUnclamped(rb.linearVelocityX, formManager.inputX * baseSpeed, 0.2f), rb.linearVelocityY);

        if (formManager.inputY <= -0.5)
        {
            Crouch(IsGrounded());
            rb.gravityScale = baseGravityScale * descensionMultiplier;

        }
        else
        {
            Crouch(false);
            rb.gravityScale = baseGravityScale;
        }
    }

    #region PLAYER_CONTROLS
    private void Crouch(bool sneak)
    {
        if (sneak)
        {
            collider2d.size = new Vector2(1, crouchingHeight);
            collider2d.offset = new Vector2(0, crouchingHeight / 2 - .5f);
        }
        else
        {
            collider2d.size = new Vector2(1, 1);
            collider2d.offset = new Vector2(0, 0);
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            rb.linearVelocityY = jumpingPower;
        }
        else if (context.performed && canDoubleJump)
        {
            rb.linearVelocityY = 0;
            rb.linearVelocityY += jumpingPower;
            canDoubleJump = false;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1f, 0.1f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }
    #endregion
}
