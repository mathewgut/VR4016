using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckUIBehaviour : MonoBehaviour
{
    // Start is called before the first frame update

    bool alive = false;
    float timeStart = -1;
    [SerializeField] float timeTarget = 5f;
    [SerializeField] TMPro.TMP_Text text;


    private void OnEnable()
    {
        alive = true;    
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            text.transform.LookAt(Camera.main.transform.position);
            text.transform.Rotate(0, 180, 0);

            if (timeStart == -1) timeStart = Time.time;
            if(Time.time - timeStart >= timeTarget)
            {
                alive = false;
                transform.gameObject.SetActive(false);
            }
        }
    }
}
