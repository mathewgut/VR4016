using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RetryButton : MonoBehaviour
{

    public void RetryGame()
    {
        SceneTransitionManager.singleton.GoToSceneAsync(0);
    }
}
