using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [Header("References")]
    public GameObject pauseMenuUI;
    public Transform headTransform;
    public GameObject locomotionObject;
    public InputActionReference pauseAction;

    [Header("Placement")]
    public float distanceFromPlayer = 1.8f;
    public float heightOffset = -0.15f;
    public float downwardTilt = 8f;

    private bool isPaused = false;

    private void OnEnable()
    {
        if (pauseAction != null)
            pauseAction.action.Enable();
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
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;

        PositionMenu();
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;

        if (locomotionObject != null)
            locomotionObject.SetActive(false);
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;

        if (locomotionObject != null)
            locomotionObject.SetActive(true);
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
        {
            Quaternion lookRotation = Quaternion.LookRotation(toPlayer.normalized, Vector3.up);
            pauseMenuUI.transform.rotation = lookRotation;
        }

        pauseMenuUI.transform.Rotate(downwardTilt, 0f, 0f);
    }
}