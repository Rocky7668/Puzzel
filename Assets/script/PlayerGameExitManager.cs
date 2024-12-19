using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameExitManager : MonoBehaviour
{
    [SerializeField] private GameObject gamePointObj;
    [SerializeField] private Vector3 DefaultPosition;
    [SerializeField] private Vector3 ChangePosition;

    private void OnEnable()
    {
        if(GameManager.instance.isPraticeMode)
        {
            gamePointObj.transform.localScale = Vector3.one * 1.6f;
            gamePointObj.GetComponent<RectTransform>().anchoredPosition = ChangePosition;
        }
        else
        {
            gamePointObj.transform.localScale = Vector3.one * 1.2f;
            gamePointObj.GetComponent<RectTransform>().anchoredPosition = DefaultPosition;
        }
    }
    private void Update()
    {
        
    }

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
