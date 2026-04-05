using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [Header("References")]
    public GameObject pauseMenuUI;
    public Transform headTransform;
    public MonoBehaviour locomotionScript;
    public InputActionReference pauseAction;

    [Header("Placement")]
    public float distanceFromPlayer = 1.8f;
    public float heightOffset = -0.15f;
    public float downwardTilt = 8f;

    private bool isPaused = false;

    private void OnEnable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.Enable();
            Debug.Log("Pause action enabled: " + pauseAction.action.name);
        }
    }

    private void OnDisable()
    {
        if (pauseAction != null)
            pauseAction.action.Disable();
    }

    private void Update()
    {
        if (pauseAction != null && pauseAction.action.WasPressedThisFrame())
        {
            Debug.Log("Pause button pressed");

            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }

        // editor test key
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("Escape pressed");

            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        Debug.Log("PauseGame called");
        isPaused = true;

        PositionMenu();

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);

        Time.timeScale = 0f;

        if (locomotionScript != null)
            locomotionScript.enabled = false;
    }

    public void ResumeGame()
    {
        Debug.Log("ResumeGame called");
        isPaused = false;

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;

        if (locomotionScript != null)
            locomotionScript.enabled = true;
    }

    private void PositionMenu()
    {
        if (pauseMenuUI == null || headTransform == null)
            return;

        Vector3 flatForward = headTransform.forward;
        flatForward.y = 0f;
        flatForward.Normalize();

        if (flatForward.sqrMagnitude < 0.01f)
            flatForward = Vector3.forward;

        Vector3 targetPosition = headTransform.position + flatForward * distanceFromPlayer;
        targetPosition.y = headTransform.position.y + heightOffset;

        pauseMenuUI.transform.position = targetPosition;

        Vector3 toPlayer = headTransform.position - pauseMenuUI.transform.position;
        toPlayer.y = 0f;

        if (toPlayer.sqrMagnitude > 0.01f)
            pauseMenuUI.transform.rotation = Quaternion.LookRotation(toPlayer.normalized, Vector3.up);

        pauseMenuUI.transform.Rotate(downwardTilt, 0f, 0f);
    }
}