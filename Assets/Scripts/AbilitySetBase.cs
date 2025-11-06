using UnityEngine;

abstract public class AbilitySetBase : MonoBehaviour
{
    public bool usingAbility1;
    public bool usingAbility2;
    public bool usingAbility3;

    //region We probably don't need this, but I'm keeping it here just in case
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //endregion

    abstract public void ability1(GameObject player);

    abstract public void ability2(GameObject player);

    abstract public void ability3(GameObject player);
}
