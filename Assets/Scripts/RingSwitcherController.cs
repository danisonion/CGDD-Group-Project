using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RingSwitcherController : MonoBehaviour
{

    public GameObject player;
    public Vector2 offset;

    private Camera _camera;

    Material _material;
    Image _image;
    bool cool = false;
    bool moving = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _image = GetComponent<Image>();
        _material = _image.material;
        _image.enabled = false;
        _camera = Camera.allCameras[0];
    }

    // Update is called once per frame
    void Update()
    {
        movingLoop();
        ((RectTransform)transform).anchoredPosition = (Vector2)_camera.WorldToScreenPoint(player.transform.position) + offset;
    }

    private void OnApplicationQuit()
    {
        _material.SetFloat("_percent", 1);
    }

    private void movingLoop()
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
    private void hideImage()
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
