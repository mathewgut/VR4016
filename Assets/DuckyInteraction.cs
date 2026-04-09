using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckyInteraction : MonoBehaviour
{
    [SerializeField] AudioClip quackSound;
    [SerializeField] Canvas countUI;
    [SerializeField] TMPro.TMP_Text countText;

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
        countUI.transform.position = transform.position;

        if(GameManager.Instance.GetCurrCollected() >= 5)
        {
            countText.text = GameManager.Instance.GetCurrCollected() + "/" + GameManager.Instance.GetToCollect() + "\n" + "Find the exit!";
        }
        else
        {
            countText.text = GameManager.Instance.GetCurrCollected() + "/" + GameManager.Instance.GetToCollect();
        }
           

        countUI.gameObject.SetActive(true);

        Destroy(transform.gameObject);
    }
}
