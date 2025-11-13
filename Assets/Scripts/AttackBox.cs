using System;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
    [HideInInspector] public float damage;
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if colliding object has an Enemy tag
        if(collision.tag == "Enemy")
        {
            try
            {
                collision.gameObject.GetComponent<EntityManager>().health.Hp -= damage;
                Debug.Log("HIT");
            } catch (NullReferenceException)
            {
                Debug.LogWarning("HIT, target does not have entityManager script.");
            }
        }
    }
}
