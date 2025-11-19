using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Hierarchy;
using UnityEngine;

public class Enemy : MonoBehaviour
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
        rb.gravityScale = 0f;
        coroutineHandler = StartCoroutine(GreenAI());
    }

    private IEnumerator GreenAI()
    {
        while (enemyData.health > 0)
        {
            do
            {
                rb.linearVelocityY = enemyData.moveSpeed * direction;
                yield return null;
            } while (transform.localPosition.y <= patrolRadius*direction);
            direction *= -1;
            rb.linearVelocityY = 0;
            yield return new WaitForSeconds(0.7f);
            do
            {
                rb.linearVelocityY = enemyData.moveSpeed * direction;
                yield return null;
            } while (transform.localPosition.y >= patrolRadius * direction);
            direction *= -1;
            rb.linearVelocityY = 0;
            yield return new WaitForSeconds(0.7f);
            yield return null;
        }
        yield break;
    }
}