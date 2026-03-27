using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorInteraction : MonoBehaviour
{
    // ensure these are the VALUE actions not the ACTION itself
    public InputActionProperty grabLeft;
    public InputActionProperty grabRight;

    [SerializeField]
    private GameObject leftController;

    [SerializeField]
    private GameObject rightController;


    bool bothGrabbing = false;
    bool lGrabbing = false;
    bool rGrabbing = false;

    void Update()
    {
        // Reading a float (0.0 to 1.0) for the trigger
        float grabLeftValue = grabLeft.action.ReadValue<float>();
        float grabRightValue = grabRight.action.ReadValue<float>();


        if (grabLeftValue > 0.1f && grabRightValue > 0.1f)
        {
            rGrabbing = true;
            lGrabbing = true;
            //Debug.Log("both grabbing");
        }
        else if (grabRightValue > 0.1f)
        {
            rGrabbing = true;
            lGrabbing = false;
        }
        else if (grabLeftValue > 0.1f)
        {
            rGrabbing = false;
            lGrabbing = true;
        }
        else
        {
            rGrabbing = false;
            lGrabbing = false;
        }

        bothGrabbing = rGrabbing && lGrabbing;

        //Debug.Log(Vector3.Distance(transform.position, leftController.transform.position) <= 1);

        CollisionCheck();

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision");
        if (!collision.gameObject.CompareTag("LController") || collision.gameObject.CompareTag("RController")) return;
        float signedAngle; 
        if (lGrabbing && collision.gameObject.CompareTag("LController"))
        {
            signedAngle = Vector3.SignedAngle(transform.forward, leftController.transform.position, Vector3.up);
        }
        else if (lGrabbing && collision.gameObject.CompareTag("RController"))
        {
            signedAngle = Vector3.SignedAngle(transform.forward, rightController.transform.position, Vector3.up);
        }
        else
        {
            return;

        }

        transform.rotation = Quaternion.Euler(0, signedAngle, 0);

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.gameObject.CompareTag("LController") || collision.gameObject.CompareTag("RController")) return;
        float signedAngle;
        if (lGrabbing && collision.gameObject.CompareTag("LController"))
        {
            signedAngle = Vector3.SignedAngle(transform.forward, leftController.transform.position, Vector3.up);
        }
        else if (lGrabbing && collision.gameObject.CompareTag("RController"))
        {
            signedAngle = Vector3.SignedAngle(transform.forward, rightController.transform.position, Vector3.up);
        }
        else
        {
            return;

        }

        transform.rotation = Quaternion.Euler(0, signedAngle, 0);
    }


    void CollisionCheck ()
    {

        GameObject toLookAt;
        if (lGrabbing && Vector3.Distance(transform.position, leftController.transform.position) <= 10) 
        {
            toLookAt = leftController;
        }
        else if (rGrabbing && Vector3.Distance(transform.position, rightController.transform.position) <= 10)
        {
            toLookAt = rightController;
        }
        else
        {
            return;
        }

        // look at object, clamp within door range, freeze x and z rotation
        transform.LookAt(toLookAt.transform.position);
        Vector3 euler = transform.eulerAngles;
        float yRange = Mathf.Clamp(euler.y, 0, 90);
        transform.eulerAngles = new Vector3(0, yRange, 0);

    }

}
    