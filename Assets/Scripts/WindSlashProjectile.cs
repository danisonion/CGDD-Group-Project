using UnityEngine;

public class WindSlashProjectile : MonoBehaviour
{
    private Rigidbody2D rb;
    public float initialForce = 300f;
    public float slowDownRate = -2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForceX(initialForce);
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.linearVelocityX < 0)
            Destroy(gameObject);
        rb.AddForceX(slowDownRate);
    }
}
