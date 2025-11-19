using System.Collections;
using UnityEngine;

public class OrangeEnemy : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private float patrolRadius;
    [SerializeField] private Transform center;

    private Rigidbody2D rb;
    private int direction = 1;
    private Coroutine coroutineHandler;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coroutineHandler = StartCoroutine(OrangeAI());
    }

    private IEnumerator OrangeAI()
    {
        while (enemyData.health > 0)
        {
            do
            {
                rb.linearVelocityX += enemyData.moveSpeed * direction;
                yield return null;
            } while (transform.localPosition.x <= patrolRadius * direction);
            direction *= -1;
            rb.linearVelocityX = 0;
            do
            {
                rb.linearVelocityX += enemyData.moveSpeed * direction;
                yield return null;
            } while (transform.localPosition.x >= patrolRadius * direction);
            direction *= -1;
            rb.linearVelocityX = 0;
            yield return null;
        }
        yield break;
    }
}
