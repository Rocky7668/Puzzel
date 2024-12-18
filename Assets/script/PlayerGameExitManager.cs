using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameExitManager : MonoBehaviour
{
    private void OnDisable()
    {
        if (GameManager.instance.time >= 1 && !GameManager.instance.isPraticeMode)
        {
            //GameManager.instance.isJoinBefore = false;
        }
        uimanager.instance.home.SetActive(true);
        puzzleManager.instance.isEnterGame = false;
    }
}
