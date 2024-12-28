using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGameExitManager : MonoBehaviour
{
    [SerializeField] private GameObject gamePointObj;
    [SerializeField] private Vector3 DefaultPosition;
    [SerializeField] private Vector3 ChangePosition;

    private void OnEnable()
    {
        foreach (var item in puzzleManager.instance.box)
        {
            item.gameObject.GetComponent<Button>().enabled = true;
            item.GetComponent<Button>().transition = Selectable.Transition.ColorTint;
        }                

        puzzleManager.instance.isEnterGame = true;
        if (GameManager.instance.isPraticeMode)
        {
            gamePointObj.transform.localScale = Vector3.one * 1.6f;
            gamePointObj.GetComponent<RectTransform>().anchoredPosition = ChangePosition;
        }
        else
        {
            gamePointObj.transform.localScale = Vector3.one * 1.2f;
            gamePointObj.GetComponent<RectTransform>().anchoredPosition = DefaultPosition;
        }
        puzzleManager.instance.BgaudioSource.enabled = false;
    }
    private void OnDisable()
    {
        if (GameManager.instance.time >= 1 && !GameManager.instance.isPraticeMode)
        {
            //GameManager.instance.isJoinBefore = false;
        }
        puzzleManager.instance.BgaudioSource.enabled = true;
        uimanager.instance.home.SetActive(true);
        puzzleManager.instance.isEnterGame = false;

        foreach (var item in puzzleManager.instance.box)
        {
            item.gameObject.GetComponent<Button>().enabled = true;
            item.GetComponent<Button>().transition = Selectable.Transition.ColorTint;
        }
    }
}
