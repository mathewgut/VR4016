using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseTools : MonoBehaviour
{
    // Start is called before the first frame update
    public NoiseController controller;

    public Material material;
    public GameObject noiseObject;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 spawnPos = new Vector3(hit.point.x, 1f, hit.point.z);
                GameObject spawned = Instantiate(noiseObject, spawnPos, Quaternion.identity);

                if (controller != null)
                {
                    controller.noiseEvent = spawned;
                }
            }
        }
    }
}
