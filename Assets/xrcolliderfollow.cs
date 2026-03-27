using UnityEngine;
using UnityEngine.XR;

public class xrcolliderfollow : MonoBehaviour
{
    public Transform head;
    public CharacterController controller;

    void Update()
    {
        controller.height = head.localPosition.y;

        Vector3 center = Vector3.zero;
        center.y = controller.height / 2;
        center.x = head.localPosition.x;
        center.z = head.localPosition.z;

        controller.center = center;
    }
}