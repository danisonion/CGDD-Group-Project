using System;
using System.Collections.Generic;
using Unity.Hierarchy;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private List<GameObject> destinationPoints = new List<GameObject>();

    [SerializeField] private EnemyData enemyData;
    private Rigidbody2D rb;
    private Transform destination;
    private int destinationID = 0;

    public enum MovementMode
    {
        None,
        Horizontal,
        Vertical,
        Omnidirectional
    }
    public enum Aggression
    {
        No,
        Partial,
        Full
    }
    [Flags]
    public enum AttackMode
    {
        None=0,
        Melee=1,
        Ranged=2,
        Magic=3
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        destination = destinationPoints[destinationID].transform;
    }

    private void FixedUpdate()
    {
        Vector2 point = destination.position - transform.position;
        if (destinationID == 0)
        {
            rb.linearVelocity = new Vector2(speed, 0);
        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, 0);
        }
        if (Vector2.Distance(transform.position, destination.position) < 0.5f)
        {
            if (destinationID == destinationPoints.Count - 1)
            {
                destinationID = 0;
            }
            else
            {
                destinationID += 1;
            }
            destination = destinationPoints[destinationID].transform;
        }
    }
}