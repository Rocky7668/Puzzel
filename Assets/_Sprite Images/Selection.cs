using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selection : MonoBehaviour
{

    public Image[] AllButtos;
    public Sprite[] AllImg;
    public Sprite[] SliceImg;

    private void OnEnable()
    {

    }

    public void SelectImage(int idx)
    {
        uimanager.instance.imgIdx = idx;
        GameManager.instance.gamePlaySprite = AllImg[idx];
        GameManager.instance.ButtonsOnOff(true);
        GameManager.instance.isPraticeMode = true;

        
        uimanager.instance.practiceModeTxt.SetActive(true);
        uimanager.instance.top.SetActive(false);
        uimanager.instance.backBtn.SetActive(true);
        uimanager.instance.time.SetActive(false);

        puzzleManager.instance.EnablePlayImage();
        NewUIManager.instance.OpenPanel(Panel.PracticeMode);

    }

}
