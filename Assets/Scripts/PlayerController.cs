using System;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Component Refrences")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D collider2d;

    [Header("Player Settings")]
    [SerializeField] private float baseSpeed;
    private float speed;
    [SerializeField] private float jumpingPower;
    [SerializeField] private float descensionMultiplier;
    [Range(0, 1)]
    [SerializeField] private float crouchingHeight = 0.6f;

    [Header("Grounding")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;

    private float inputX, inputY;
    private float baseGravityScale;

    private void Awake() {
        baseGravityScale = rb.gravityScale;
    }



    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(IsGrounded() ? inputX * baseSpeed :
        Mathf.LerpUnclamped(rb.linearVelocity.x, inputX * baseSpeed, 0.2f), rb.linearVelocity.y);

        if (inputY <= -0.5)
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
    public void Move(InputAction.CallbackContext context)
    {
        inputX = context.ReadValue<Vector2>().x;
        inputY = context.ReadValue<Vector2>().y;
    }

    private void Crouch(bool sneak) {
        if (sneak)
        {
            collider2d.size = new Vector2(1, crouchingHeight);
            collider2d.offset = new Vector2(0, crouchingHeight/2 -.5f);
            speed = baseSpeed / 2;
        }
        else
        {
            collider2d.size = new Vector2(1, 1);
            collider2d.offset = new Vector2(0, 0);
            speed = baseSpeed;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            rb.linearVelocityY = jumpingPower;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1f, 0.1f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }


    #endregion
}
