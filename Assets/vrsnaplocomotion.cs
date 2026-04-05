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
    public float collisionRadius = 0.3f;
    public float playerHeight = 1.7f;
    public float skinWidth = 0.05f;

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

        Vector3 currentBodyPosition = GetBodyPosition();
        Vector3 targetBodyPosition = currentBodyPosition + moveDirection * stepDistance;

        if (useCollisionCheck)
        {
            if (!CanMoveTo(currentBodyPosition, targetBodyPosition, moveDirection, stepDistance))
                return;
        }

        Vector3 rigOffset = targetBodyPosition - currentBodyPosition;
        xrOrigin.transform.position += rigOffset;
    }

    private Vector3 GetBodyPosition()
    {
        Vector3 bodyPosition = headTransform.position;
        bodyPosition.y = xrOrigin.transform.position.y;
        return bodyPosition;
    }

    private bool CanMoveTo(Vector3 startBodyPosition, Vector3 targetBodyPosition, Vector3 moveDirection, float distance)
    {
        Vector3 startBottom = startBodyPosition + Vector3.up * collisionRadius;
        Vector3 startTop = startBodyPosition + Vector3.up * (playerHeight - collisionRadius);

        float castDistance = distance + skinWidth;

        bool hitOnPath = Physics.CapsuleCast(
            startBottom,
            startTop,
            collisionRadius,
            moveDirection,
            castDistance,
            blockingLayers,
            QueryTriggerInteraction.Ignore
        );

        if (hitOnPath)
            return false;

        Vector3 targetBottom = targetBodyPosition + Vector3.up * collisionRadius;
        Vector3 targetTop = targetBodyPosition + Vector3.up * (playerHeight - collisionRadius);

        bool blockedAtDestination = Physics.CheckCapsule(
            targetBottom,
            targetTop,
            collisionRadius,
            blockingLayers,
            QueryTriggerInteraction.Ignore
        );

        return !blockedAtDestination;
    }

    private void OnDrawGizmosSelected()
    {
        if (xrOrigin == null || headTransform == null)
            return;

        Gizmos.color = Color.green;

        Vector3 bodyPosition = headTransform.position;
        bodyPosition.y = xrOrigin.transform.position.y;

        Vector3 bottom = bodyPosition + Vector3.up * collisionRadius;
        Vector3 top = bodyPosition + Vector3.up * (playerHeight - collisionRadius);

        Gizmos.DrawWireSphere(bottom, collisionRadius);
        Gizmos.DrawWireSphere(top, collisionRadius);
    }
}