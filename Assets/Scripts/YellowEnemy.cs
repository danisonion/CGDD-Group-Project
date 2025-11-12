using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowEnemy : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private float chaseSpeedMultiplier;
    [SerializeField] private Transform target, center;
    [SerializeField] private float patrolRange, detectionRadius, attackRange;
    [SerializeField] private List<Transform> breakChasePoints = new List<Transform>();

    //base statistics that are privated are listed below
    private bool inDetectionRange, inAttackRange, attacking;
    private Vector2 destination;
    private Rigidbody2D rb;
    private float attackCooldownDuration = 1f;
    [SerializeField][Range(-1f,1f)] private static float direction;
    private enum State
    {
        Patrolling,
        Aggressive
    } private State state = State.Patrolling;
    private Coroutine patrolling, aggro;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        patrolling = StartCoroutine(Patrolling());
        aggro = null;
    }

    private void FixedUpdate()
    {
        inDetectionRange = Vector2.Distance(transform.position, target.position) <= detectionRadius;
        switch (state)
        {
            case State.Patrolling:
                if (inDetectionRange)
                {
                    SwitchStates();
                }
                break;
            case State.Aggressive:
                inAttackRange = Vector2.Distance(transform.position, target.position) <= attackRange;
                break;
        }
        
    }

    #region "State Coroutines"
    private IEnumerator Patrolling()
    {
        if (transform.position.x - destination.x <= 0f)
        {
            direction = 1;
            destination = new Vector2(center.position.x + Random.Range(0.5f, patrolRange), center.position.y);
        }
        else
        {
            direction = -1;
            destination = new Vector2(center.position.x + Random.Range(-patrolRange, 0.5f), center.position.y);
        }
        while (!(Vector2.Distance(transform.position, destination) < 0.05f))
        {
            rb.linearVelocityX = enemyData.moveSpeed * direction;
            yield return null;
        }
        yield return new WaitForSeconds(3f);
        patrolling = StartCoroutine(Patrolling());
        yield break;
    }

    private IEnumerator Aggro()
    {
        Transform rightBreakPoint = breakChasePoints[0];
        Transform leftBreakPoint = breakChasePoints[1];
        while (true)
        {
            direction = Mathf.Min(Mathf.Max(-1f, target.transform.position.x - transform.position.x), 1f);
            rb.linearVelocityX = enemyData.moveSpeed * chaseSpeedMultiplier * direction;
            if ((Vector2.Distance(transform.position, rightBreakPoint.position) < 0.2 && 
                target.transform.position.x - rightBreakPoint.position.x > 0f) ||
                (Vector2.Distance(transform.position, leftBreakPoint.position) < 0.2 &&
                leftBreakPoint.position.x - target.transform.position.x > 0f))
            {
                Debug.Log("Rats!");
                break;
            }
            if (inAttackRange)
            {
                attacking = true;
                StartCoroutine(Attacking(leftBreakPoint, rightBreakPoint));
                yield return new WaitUntil(() => attacking == false);
                Debug.Log(leftBreakPoint.position.x - target.transform.position.x);
                
            }
            
            yield return null;
        }
        rb.linearVelocityX = 0;
        float timeSet = Time.time;
        while (Time.time - timeSet < 1f)
        {
            bool stillInRange = Vector2.Distance(transform.position, leftBreakPoint.position) < 0.2 && transform.position.x - target.transform.position.x < 0f ||
                Vector2.Distance(transform.position, rightBreakPoint.position) < 0.2 && target.transform.position.x - transform.position.x < 0f;
            if (stillInRange)
            {
                do
                {
                    direction = Mathf.Min(Mathf.Max(-1f, target.transform.position.x - transform.position.x), 1f);
                    rb.linearVelocityX = enemyData.moveSpeed * chaseSpeedMultiplier * direction;
                    yield return null;
                } while (stillInRange);
                aggro = StartCoroutine(Aggro());
                yield break;
            }
            if (inDetectionRange)
            {
                timeSet = Time.time;
            }
            yield return null;
        }
        SwitchStates();
        yield break;
    }

    public IEnumerator Attacking(Transform leftPoint, Transform rightPoint)
    {
        rb.linearVelocityX = 0;
        yield return new WaitForSeconds(0.3f);
        for (float dash = 0f; dash < Mathf.PI/2; dash += Mathf.PI/720)
        {
            bool stopped = Vector2.Distance(transform.position, rightPoint.position) < 0.2 ||
                Vector2.Distance(transform.position, leftPoint.position) < 0.2;
            if (stopped)
            {
                rb.linearVelocityX = 0f;
            }
            else
            {
                rb.linearVelocityX = Mathf.Cos(dash) * 10 * direction;
            }
            yield return null;
        }
        yield return new WaitForSeconds(attackCooldownDuration);
        attacking = false;
        yield break;
    }
    #endregion

    private void SwitchStates()
    {
        switch (state)
        {
            case State.Patrolling:
                StopCoroutine(patrolling);
                patrolling = null;
                state = State.Aggressive;
                aggro = StartCoroutine(Aggro());
                break;
            case State.Aggressive:
                StopCoroutine(aggro);
                aggro = null;
                state = State.Patrolling;
                patrolling = StartCoroutine(Patrolling());
                break;
            default:
                Debug.Log("There is no such state that exists");
                break;
        }
    }
}
