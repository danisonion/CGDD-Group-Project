using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlueEnemy : MonoBehaviour
{
    [SerializeField] EnemyData enemyData;
    [SerializeField] private float detectionRadius, attackRange;
    [SerializeField] private Transform target;

    private bool touchingTerrain, attacking;
    private Rigidbody2D rb;
    private int xDirection, yDirection;
    private float attackCooldown = 2f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        StartCoroutine(BlueAI());
    }

    private IEnumerator BlueAI()
    {
        float xStep = 0;
        float yStep = 0;
        float acceleration = enemyData.moveSpeed;
        int changeXPoint;
        yield return new WaitUntil(() => Vector2.Distance(transform.position, target.position) < detectionRadius * Mathf.Sqrt(2));
        Debug.Log("Here I come!");
        yield return new WaitForSeconds(0.7f);
        while (true)
        {
            if (transform.position.x < target.position.x) { changeXPoint = -1; } else { changeXPoint = 1; }
            Vector3 attackPoint = target.position + new Vector3(attackRange * Mathf.Sqrt(2) * changeXPoint, attackRange * Mathf.Sqrt(2), target.position.z);
            RaycastHit2D isInRay = Physics2D.Raycast(transform.position, 
                new Vector2(changeXPoint * -1, -1f), attackRange * Mathf.Sqrt(2), LayerMask.GetMask("Player"));
            if (isInRay)
            {
                attacking = true;
                Debug.Log(attacking);
                StartCoroutine(Attack());
                yield return new WaitUntil(() => attacking == false);
            }
            if (transform.position.x < attackPoint.x) { xDirection = 1; } else { xDirection = -1; }
            if (transform.position.y < attackPoint.y) { yDirection = 1; } else { yDirection = -1; }
            if ((xStep >= enemyData.moveSpeed && xDirection == 1) || (xStep <= enemyData.moveSpeed * -1 && xDirection == -1))
            { xStep = enemyData.moveSpeed * xDirection; } else { xStep += acceleration * Time.deltaTime * xDirection; }
            if ((yStep >= enemyData.moveSpeed && yDirection == 1) || (yStep <= enemyData.moveSpeed * -1 && yDirection == -1))
            { yStep = enemyData.moveSpeed * yDirection; } else { yStep += acceleration * Time.deltaTime * yDirection; }
            rb.linearVelocity = new Vector2(xStep, yStep);
            yield return null;
        }
    }
    private IEnumerator Attack()
    {
        rb.linearVelocity = new Vector2(0f,0f);
        Vector3 targetLastPosition = target.position;
        yield return new WaitForSeconds(0.75f);
        rb.linearVelocity = new Vector2((targetLastPosition.x - transform.position.x) / 0.25f,
    (targetLastPosition.y - transform.position.y) / 0.25f);
        do
        {
            yield return null;
        } while (Vector2.Distance(transform.position, targetLastPosition) > 0.5*Mathf.Sqrt(2));
        while (Mathf.Round(rb.linearVelocity.x) != 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x / 1.03f, rb.linearVelocity.y / 1.03f);
            if (touchingTerrain) { Debug.Log("Oof!");  break; }
            yield return null;
        }
        rb.linearVelocity = new Vector2(0f, 0.25f);
        yield return new WaitForSeconds(attackCooldown);
        attacking = false;
        yield break;
    }

    #region Collision Check
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            touchingTerrain = true;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.layer);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            touchingTerrain = false;
        }
    }
    #endregion
}
