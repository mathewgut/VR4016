using UnityEngine;
using UnityEngine.Events;
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private bool initiallyPaused;
    public bool IsPaused => pauseMenu.activeSelf;
    public UnityEvent onEnterPaused;
    public UnityEvent onExitPaused;
    private void Awake()
    {
        if (!pauseMenu) pauseMenu = gameObject;
        SetPauseMode(initiallyPaused);
    }
    public void TogglePause()
    {
        SetPauseMode(!IsPaused);
    }
    private void SetPauseMode(bool pause)
    {
        pauseMenu.SetActive(pause);
        Time.timeScale = pause ? 0f : 1f; 
        if (pause) onEnterPaused.Invoke();
        else onExitPaused.Invoke();
    }
}