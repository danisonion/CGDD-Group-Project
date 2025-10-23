using UnityEngine;

[RequireComponent(typeof(EntityController))]
public class EntityManager : MonoBehaviour
{
    [SerializeField] private protected EntityController entityController;
    public float inputX { get; set; }
    public float inputY { get; set; }

    void Awake()
    {
        entityController = GetComponent<EntityController>();
    }
}