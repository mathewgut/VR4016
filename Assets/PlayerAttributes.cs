using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    public bool isHidden = false;
    GameObject playerCollider;
    Camera mainCam;

    float colliderYOffset = 0;


    void Start()
    {
        playerCollider = GameObject.FindGameObjectWithTag("PlayerCollider");
        mainCam = Camera.main;
    }

    void Update()
    {
        playerCollider.transform.position = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y + colliderYOffset, mainCam.transform.position.z);
    }


}
