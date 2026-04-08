using UnityEngine;
using TMPro;

public class TextBlink : MonoBehaviour
{
    public TMP_Text tmpText;
    public float flashInterval = 0.5f; 

    private void Start()
    {
        if (tmpText == null)
            tmpText = GetComponent<TMP_Text>();

        InvokeRepeating(nameof(ToggleText), flashInterval, flashInterval);
    }

    void ToggleText()
    {
        tmpText.enabled = !tmpText.enabled;
    }
}
