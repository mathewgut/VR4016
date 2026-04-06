using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject mainPage;
    public GameObject controlsPage;
    public Transform headTransform;
    public MonoBehaviour[] playerScriptsToDisable;
    public MonoBehaviour[] aiScriptsToDisable;
    public NavMeshAgent[] agentsToStop;
    public Animator[] animatorsToPause;
    public InputActionReference pauseAction;

    public float distanceFromPlayer = 1.8f;
    public float heightOffset = -0.15f;

    private bool isPaused = false;

    private void OnEnable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.Enable();
            pauseAction.action.performed += OnPausePerformed;
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

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        isPaused = true;

        ShowMenu();

        if (mainPage != null) mainPage.SetActive(true);
        if (controlsPage != null) controlsPage.SetActive(false);

        foreach (MonoBehaviour script in playerScriptsToDisable)
        {
            if (script != null)
                script.enabled = false;
        }

        foreach (MonoBehaviour script in aiScriptsToDisable)
        {
            if (script != null)
                script.enabled = false;
        }

        foreach (NavMeshAgent agent in agentsToStop)
        {
            if (agent != null)
                agent.isStopped = true;
        }

        foreach (Animator anim in animatorsToPause)
        {
            if (anim != null)
                anim.speed = 0f;
        }

        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    public void ResumeGame()
    {
        isPaused = false;

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        foreach (MonoBehaviour script in playerScriptsToDisable)
        {
            if (script != null)
                script.enabled = true;
        }

        foreach (MonoBehaviour script in aiScriptsToDisable)
        {
            if (script != null)
                script.enabled = true;
        }

        foreach (NavMeshAgent agent in agentsToStop)
        {
            if (agent != null)
                agent.isStopped = false;
        }

        foreach (Animator anim in animatorsToPause)
        {
            if (anim != null)
                anim.speed = 1f;
        }

        AudioListener.pause = false;
        Time.timeScale = 1f;
    }

    public void OpenControlsPage()
    {
        if (mainPage != null) mainPage.SetActive(false);
        if (controlsPage != null) controlsPage.SetActive(true);
    }

    public void BackToMainPage()
    {
        if (controlsPage != null) controlsPage.SetActive(false);
        if (mainPage != null) mainPage.SetActive(true);
    }

    public void QuitGame()
    {
        AudioListener.pause = false;
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void QuitToMainMenu(string sceneName)
    {
        AudioListener.pause = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    private void ShowMenu()
    {
        if (pauseMenuUI == null || headTransform == null)
            return;

        pauseMenuUI.SetActive(true);

        Vector3 menuPos = headTransform.position + headTransform.forward * distanceFromPlayer;
        menuPos.y = headTransform.position.y + heightOffset;
        pauseMenuUI.transform.position = menuPos;

        pauseMenuUI.transform.rotation = Quaternion.LookRotation(
            headTransform.position - pauseMenuUI.transform.position,
            Vector3.up
        );

        pauseMenuUI.transform.Rotate(0f, 180f, 0f);

    }
}