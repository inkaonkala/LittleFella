using UnityEngine;
using UnityEngine.UI;              // <— needed for Button
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private string openingSceneName = "Opening";
    [SerializeField] private Button buttonClose;
    [SerializeField] private Button buttonExit;

    [Header("Input")]
    [SerializeField] private InputActionReference pauseAction;

    private bool isPaused;

    private void Start()
    {
        Resume();
            
        if (buttonClose) buttonClose.onClick.AddListener(Resume);
        if (buttonExit)  buttonExit.onClick.AddListener(ExitToOpening);
    }

    private void OnEnable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.performed += OnPausePerformed;
            pauseAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.performed -= OnPausePerformed;
            pauseAction.action.Disable();
        }
    }

    private void OnPausePerformed(InputAction.CallbackContext ctx) => TogglePause();

    public void TogglePause()
    {
        if (isPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        if (isPaused) return;
        isPaused = true;

        if (pauseCanvas) pauseCanvas.SetActive(true);

        Time.timeScale = 0f;
        AudioListener.pause = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        if (!isPaused) return;
        isPaused = false;

        if (pauseCanvas) pauseCanvas.SetActive(false);

        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    public void ExitToOpening()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene(openingSceneName);
    }
}
