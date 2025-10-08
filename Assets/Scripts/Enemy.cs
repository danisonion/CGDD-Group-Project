using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.Tilemaps;

public class enemy : MonoBehaviour
{
    [Header("Enemy Attributes")]
    [SerializeField] private float speed;
    private int direction = -1;
    private Transform en_transform;

    private void Start() => en_transform = transform;

    private void FixedUpdate()
    {
        en_transform.position = new Vector2(en_transform.position.x + speed * direction *Time.deltaTime, en_transform.position.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<TilemapCollider2D>() != null)
        {
            direction *= -1;
        }
    }
}