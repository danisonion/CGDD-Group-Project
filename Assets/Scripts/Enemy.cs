using System;
using System.Collections.Generic;
using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.Tilemaps;

public class enemy : MonoBehaviour
{
    [Header("Enemy Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private List<GameObject> destinationPoints = new List<GameObject>();

    private Rigidbody2D rb;
    private Transform destination;
    private int destinationID = 0;

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