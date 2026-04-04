using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBehaviour : MonoBehaviour
{

    int tick;

    [SerializeField] AudioSource source;
    GameObject playerCollider;


    // Start is called before the first frame update
    void Start()
    {
        playerCollider = GameObject.FindGameObjectWithTag("PlayerCollider");
    }

    // Update is called once per frame
    void Update()
    {
        tick += 1;

        if(tick % 5 == 0)
        {
            if (Vector3.Distance(transform.position, playerCollider.transform.position) <= 15)
            {
                if(!source.isPlaying) source.Play();
            }
            else
            {
                source.Stop();
            }
        }
    }
}
