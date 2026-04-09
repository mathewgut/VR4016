using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockerBehaviour : MonoBehaviour
{
    [SerializeField] LockerTrigger trigger;
    [SerializeField] GameObject door;
    [SerializeField] GameObject buttonUI;

    Camera mainCam;
    GameObject Player;
    PlayerAttributes attributes;

    public bool isPlayerHidden = false;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        attributes = Player.GetComponent<PlayerAttributes>();
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // 92 represents closed, and it subtracts towards 0 for fully open
        attributes.isHidden = trigger.playerInArea && door.transform.rotation.eulerAngles.y > 75;

        if (Vector3.Distance(Camera.main.transform.position, transform.position) <= 5f)
        {
            buttonUI.SetActive(true);
        }
        else
        {
            buttonUI.SetActive(false);
        }

    }
}
