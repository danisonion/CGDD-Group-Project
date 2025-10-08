using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class RingSwitcherController : MonoBehaviour
{

    public GameObject player;
    Material _material;
    Boolean cool = false;
    Boolean moving = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _material = GetComponent<Material>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            float percent = _material.GetFloat("Percent");

            if (cool && percent < 1)
            {
                _material.SetFloat("Percent", percent + 0.01f);
            }
            else if (!cool && percent > 0)
            {
                _material.SetFloat("Percent", percent - 0.01f);
            }
            else
            {
                moving = false;
            }
        }
    }

    public void SwapSides(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            cool = !cool;
            moving = true;
        }
    }
}
