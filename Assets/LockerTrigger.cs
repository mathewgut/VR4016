using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockerTrigger : MonoBehaviour
{

    [SerializeField] GameObject rootObject;

    public bool playerInArea = false;
   

    Camera mainCamera;
    BoxCollider selfCollider;

    int tick = 0;

    void Start()
    {
        mainCamera = Camera.main;
        selfCollider = transform.GetComponent<BoxCollider>();
    }

    private void Update()
    {
        tick += 1;

        // performance measure (multiple lockers would mean this check is multiplied by n, which is not great)
        if (tick % 4 == 0 && Vector3.Distance(rootObject.transform.position, mainCamera.transform.position) < 3) { 
            playerInArea = IsPointInsideBox(mainCamera.transform.position, selfCollider);
        }
    }


    private bool IsPointInsideBox(Vector3 worldPoint, BoxCollider box)
    {
        Vector3 localPoint = box.transform.InverseTransformPoint(worldPoint) - box.center;

        Vector3 halfSize = box.size * 0.5f;

        return Mathf.Abs(localPoint.x) <= halfSize.x &&
               Mathf.Abs(localPoint.y) <= halfSize.y &&
               Mathf.Abs(localPoint.z) <= halfSize.z;
    }

}

