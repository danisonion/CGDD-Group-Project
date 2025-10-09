using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RingSwitcherController : MonoBehaviour
{

    public GameObject player;
    Material _material;
    Image _image;
    Boolean cool = false;
    Boolean moving = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _image = GetComponent<Image>();
        _material = _image.material;
        _image.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            float percent = _material.GetFloat("_Percent");

            if (cool && percent < 1)
            {
                _material.SetFloat("_Percent", percent + Time.deltaTime * 3);
            }
            else if (!cool && percent > 0)
            {
                _material.SetFloat("_Percent", percent - Time.deltaTime * 3);
            }
            else
            {
                moving = false;
                Invoke(nameof(hideImage), 0.5f);
            }
        }
    }

    void hideImage()
    {
        _image.enabled = false;
    }

    public void SwapSides(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            cool = !cool;
            moving = true;
            _image.enabled = true;
            CancelInvoke(nameof(hideImage));
        }
    }
}
