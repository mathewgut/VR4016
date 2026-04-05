using UnityEngine;
using System.Collections;
using TMPro;

public class creditscroll : MonoBehaviour
{
    public float scrollSpeed = 20f;
    public float delay = 5f;
    public TMP_Text textComponent;

    private bool canScroll = false;

    private void Start()
    {
        StartCoroutine(StartScroll());
    }

    private IEnumerator StartScroll()
    {
        textComponent.enabled = false;

        yield return new WaitForSeconds(delay);

        textComponent.enabled = true;
        canScroll = true;
    }

    private void Update()
    {
        if (canScroll)
        {
            transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
        }
    }
}