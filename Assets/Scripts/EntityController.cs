using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class EntityController : MonoBehaviour
{
    [Header("Controller Settings")]
    public ControllerData controllerData;
    public float sizeX, sizeY;
    // [SerializeField] private float baseSpeed;
    // [SerializeField] private float jumpingPower;


    [Header("Grounding")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;

    // [Header("Crouching")]
    // [SerializeField] private float descensionMultiplier;
    // [Range(0, 1)]
    // [SerializeField] private float crouchingHeight = 0.6f;

    [Header("Entity Component Refrences")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D collider2d;

    //private float inputX, inputY;
    private float baseGravityScale;
    private EntityManager entityManager;

    private int airJumpCount;

    private void Awake()
    {
        baseGravityScale = rb.gravityScale;
        entityManager = GetComponent<EntityManager>();
        groundCheck.localPosition = new Vector3(0, -sizeY/2f);
    }



    private void FixedUpdate()
    {
        if (IsGrounded())
        {
            airJumpCount = 0;
        }

        rb.linearVelocity = new Vector2(IsGrounded() ? entityManager.inputX * controllerData.baseSpeed :
        Mathf.LerpUnclamped(rb.linearVelocity.x, entityManager.inputX * controllerData.baseSpeed, 0.2f), rb.linearVelocity.y);

        if (entityManager.inputY <= -0.5)
        {
            Crouch(IsGrounded());
            rb.gravityScale = baseGravityScale * controllerData.gravityMultiplier * controllerData.descensionMultiplier;

        }
        else
        {
            Crouch(false);
            rb.gravityScale = baseGravityScale * controllerData.gravityMultiplier;
        }
    }

    #region ENTITY_CONTROLS

    private void Crouch(bool sneak)
    {
        if (sneak)
        {
            collider2d.size = new Vector2(sizeX, sizeY*controllerData.crouchingHeight);
            collider2d.offset = new Vector2(0, -(1-controllerData.crouchingHeight)*sizeY/2f);
        }
        else
        {
            collider2d.size = new Vector2(sizeX, sizeY);
            collider2d.offset = new Vector2(0, 0);
        }
    }

    public void Jump(bool ignoreGrounded = false, float powerMultiplier = 1)
    {
        if (IsGrounded() || ignoreGrounded)
        {
            rb.linearVelocityY = controllerData.jumpingPower * powerMultiplier;
        }
        else if (airJumpCount < controllerData.maxAirJump)
        {
            rb.linearVelocityY = 0;
            rb.linearVelocityY += controllerData.jumpingPower * powerMultiplier;
            airJumpCount++;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1f, 0.1f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }


    #endregion
}
