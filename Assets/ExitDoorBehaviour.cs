using UnityEngine;

public class ExitDoorBehaviour : MonoBehaviour
{
    int tick = 0;
    [SerializeField] GameObject child;
 

    // Start is called before the first frame update
    void Start()
    {
        child.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        tick += 1;

        if (tick % 10 == 0)
        {
            if (GameManager.Instance.GetGameState() == GameManager.GameState.Collected) child.gameObject.SetActive(true);
            else child.gameObject.SetActive(false);
        }
    }
}
