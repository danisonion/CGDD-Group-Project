using System;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
    [HideInInspector] public float damage;
    public PlayerManager playerManager;

    void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
    }
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
            if(playerManager.PogoSurfBox.enabled) playerManager.Jump(true, 2f);
        }
    }
}
