using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RotationTest : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject toRotate;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 targetPos = (transform.position - toRotate.transform.position);
        float angle = - Mathf.Atan2(targetPos.x, targetPos.z) * Mathf.Rad2Deg;
        Debug.Log(angle);

        transform.eulerAngles  = new Vector3(0,angle,0);
        transform.LookAt(toRotate.transform.position);
        Vector3 euler = transform.eulerAngles;
        float yRange = Mathf.Clamp(euler.y, 0, 90);

        transform.eulerAngles = new Vector3(0, yRange, 0);
        Debug.Log(euler.y);
    }
}
