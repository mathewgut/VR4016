using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckyInteraction : MonoBehaviour
{
    [SerializeField] AudioClip quackSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CollectDuck()
    {
        GameManager.Instance.IncrementCollected();
        AudioSource.PlayClipAtPoint(quackSound, transform.position);
        Destroy(transform.gameObject);
    }
}
