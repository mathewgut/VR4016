using UnityEngine;

public class creditscroll : MonoBehaviour
{
    public float scrollSpeed = 20f;
    public float delay = 5f;
    public GameObject textObject;

    private void Start()
    {
        textObject.SetActive(false);

        StartCoroutine(ShowTextAfterDelay());
    }

    private System.Collections.IEnumerator ShowTextAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        textObject.SetActive(true);
    }

    private void Update()
    {
        if (textObject.activeSelf)
        {
            transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
        }
    }
}
