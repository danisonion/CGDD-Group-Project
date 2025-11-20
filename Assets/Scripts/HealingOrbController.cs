using Unity.VisualScripting;
using UnityEngine;

public class HealingOrbController : MonoBehaviour
{
    public GameObject player;

    public float healing_amount = 5;
    public float chase_amount = 50;

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.Normalize();
        rb.AddForce(chase_amount * Time.deltaTime * direction);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            Debug.Log("Hit!");
            PlayerManager man = player.GetComponent<PlayerManager>();

            float missingHealth = man.health.MaxHp - man.health.Hp;

            if (healing_amount > missingHealth)
            {
                man.health.Hp = man.health.MaxHp;
            }
            else
            {
                man.health.Hp += healing_amount;
            }
            Destroy(gameObject);
        }
    }
}
