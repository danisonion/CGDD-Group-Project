using UnityEngine;

[RequireComponent(typeof(EntityController))]
public class EntityManager : MonoBehaviour
{
    [SerializeField] private protected EntityController entityController;
    public float inputX { get; set; }
    public float inputY { get; set; }

    private protected float attackDuration, attackCooldown;

    [System.Serializable]
    public struct Health
    {
        public float Hp;
        public float MaxHp;
    }

    [Header("Health & stats")]
    // Just putting this here for future reference (we still don't have a health mechanic lol)
    public Health health;

    void Awake()
    {
        entityController = GetComponent<EntityController>();
    }
}