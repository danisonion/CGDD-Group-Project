using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public InputActionMap UIActions;

    public bool paused;

    public GameObject pauseMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIActions = InputSystem.actions.actionMaps[1];
        UIActions.FindAction("Pause").performed += OnPause;
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (paused)
        {
            paused = false;
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
        else
        {
            paused = true;
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
    }
}
