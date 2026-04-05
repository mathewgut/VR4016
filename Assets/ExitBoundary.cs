using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitBoundary : MonoBehaviour
{
    BoxCollider coll;


    // Start is called before the first frame update
    void Start()
    {
        coll = transform.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {

        // idk this was supposed top be a performance measure then i got lazy. it works okay
       if(GameManager.Instance.GetGameState() == GameManager.GameState.Collected)
        {
            if(Vector3.Distance(transform.position, Camera.main.transform.position) <= 10)
            {
                if(IsPointInsideBox(Camera.main.transform.position, coll))
                {
                    GameManager.Instance.SetGameState(GameManager.GameState.Won);
                }
            }
        }
    }

    private bool IsPointInsideBox(Vector3 worldPoint, BoxCollider box)
    {
        Vector3 localPoint = box.transform.InverseTransformPoint(worldPoint) - box.center;

        Vector3 halfSize = box.size * 0.5f;

        return Mathf.Abs(localPoint.x) <= halfSize.x &&
               Mathf.Abs(localPoint.y) <= halfSize.y &&
               Mathf.Abs(localPoint.z) <= halfSize.z;
    }
}
