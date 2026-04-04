using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockerBehaviour : MonoBehaviour
{
    [SerializeField] LockerTrigger trigger;
    [SerializeField] GameObject door;

    GameObject Player;
    PlayerAttributes attributes;

    public bool isPlayerHidden = false;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        attributes = Player.GetComponent<PlayerAttributes>();
    }

    // Update is called once per frame
    void Update()
    {
        // 92 represents closed, and it subtracts towards 0 for fully open
        attributes.isHidden = trigger.playerInArea && door.transform.rotation.eulerAngles.y > 75;
    }
}
