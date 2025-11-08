using UnityEngine;

public class WindSlashProjectile : MonoBehaviour
{
    private Rigidbody2D rb;
    public float initialForceMagnitude;
    public float slowDownRate;

    public bool FacingRight {  get; set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (FacingRight)
        {
            rb.linearVelocityX = 1;
        }
        else
        {
            rb.linearVelocityX = -1;
            initialForceMagnitude *= -1;
            slowDownRate *= -1;
        }

        rb.AddForce(transform.right * initialForceMagnitude);
    }

    void Update()
    {
        if (FacingRight && rb.linearVelocityX <= 0)
            Destroy(gameObject);
        if (!FacingRight && rb.linearVelocityX >= 0)
            Destroy(gameObject);

        rb.AddForce(transform.right * slowDownRate);
    }
}
