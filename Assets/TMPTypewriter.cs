using System.Collections;
using TMPro;
using UnityEngine;

public class TMPTypewriter : MonoBehaviour
{
    public TMP_Text textComponent;
    [TextArea] public string fullText;
    public float delay = 0.05f;

    private void Start()
    {
        StartCoroutine(RevealText());
    }

    IEnumerator RevealText()
    {
        textComponent.text = fullText;
        textComponent.maxVisibleCharacters = 0;

        for (int i = 0; i <= fullText.Length; i++)
        {
            textComponent.maxVisibleCharacters = i;
            yield return new WaitForSeconds(delay);
        }
    }
}