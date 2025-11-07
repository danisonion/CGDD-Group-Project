using UnityEngine;

[CreateAssetMenu(fileName = "ControllerData", menuName = "Scriptable Objects/ControllerData")]
public class ControllerData : ScriptableObject
{
    [Header("Player Settings")]
    public float baseSpeed;
    public float jumpingPower;
    // How many times the character can double jump, 0 means disabled
    public int maxAirJump; 
    
    [Header("Crouching")]
    public float descensionMultiplier;
    [Range(0, 1)]
    public float crouchingHeight = 0.6f;

    public float gravityMultiplier = 1;

    [Header("Abilities")]
    public AbilityBase[] abilities;

}
