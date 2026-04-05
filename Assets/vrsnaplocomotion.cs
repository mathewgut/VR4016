using UnityEngine;
using UnityEngine.InputSystem;
using Unity.XR.CoreUtils;

public class VRSnapLocomotion : MonoBehaviour
{
    [Header("XR References")]
    public XROrigin xrOrigin;
    public Transform headTransform;

    [Header("Input")]
    public InputActionReference moveAction;

    [Header("Step Settings")]
    public float stepDistance = 1.0f;
    public float inputThreshold = 0.8f;
    public float resetThreshold = 0.2f;

    [Header("Collision")]
    public bool useCollisionCheck = true;
    public LayerMask blockingLayers;
    public float checkRadius = 0.3f;

    private bool stickEngaged = false;

    private void OnEnable()
    {
        if (moveAction != null)
            moveAction.action.Enable();
    }

    private void OnDisable()
    {
        if (moveAction != null)
            moveAction.action.Disable();
    }

    private void Update()
    {
        if (xrOrigin == null || headTransform == null || moveAction == null)
            return;

        Vector2 input = moveAction.action.ReadValue<Vector2>();

        if (!stickEngaged && input.magnitude >= inputThreshold)
        {
            StepMove(input);
            stickEngaged = true;
        }
        else if (stickEngaged && input.magnitude <= resetThreshold)
        {
            stickEngaged = false;
        }
    }

    private void StepMove(Vector2 input)
    {
        Vector3 forward = headTransform.forward;
        Vector3 right = headTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * input.y + right * input.x).normalized;

        if (moveDirection.sqrMagnitude < 0.01f)
            return;

        Vector3 startPosition = xrOrigin.transform.position;
        Vector3 targetPosition = startPosition + moveDirection * stepDistance;

        if (useCollisionCheck)
        {
            if (!Physics.CheckSphere(targetPosition, checkRadius, blockingLayers))
            {
                xrOrigin.transform.position = targetPosition;
            }
        }
        else
        {
            xrOrigin.transform.position = targetPosition;
        }
    }
}