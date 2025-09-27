using UnityEngine;
using UnityEngine.Events;

public class PauseManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pausePanel;

    [Header("Events")]
    public UnityEvent OnPaused;
    public UnityEvent OnResumed;

    public bool IsPaused { get; private set; }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (IsPaused) 
            Resume();
        else 
            Pause();
    }

    public void Pause()
    {
        if (IsPaused) return;
        IsPaused = true;

        Time.timeScale = 0f;

        if (pausePanel) 
            pausePanel.SetActive(true);
        OnPaused?.Invoke();
    }

    public void Resume()
    {
        if (!IsPaused) return;
        IsPaused = false;

        Time.timeScale = 1f;

        if (pausePanel) pausePanel.SetActive(false);
        OnResumed?.Invoke();
    }
}
