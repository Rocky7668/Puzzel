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
        Debug.Log(idx);
        uimanager.instance.imgIdx = idx;
        Debug.Log("from Here");

        GameManager.instance.gamePlaySprite = AllImg[idx];
        uimanager.instance.play.SetActive(true);
        uimanager.instance.practiceModeTxt.SetActive(true);
        uimanager.instance.top.SetActive(false);
        uimanager.instance.home.SetActive(false);
     
        uimanager.instance.winBtnObj.SetActive(false);
        uimanager.instance.backBtn.SetActive(true);

        GameManager.instance.ButtonsOnOff(true);
        puzzleManager.instance.EnablePlayImage();
        GameManager.instance.isPraticeMode = true;
        this.gameObject.SetActive(false);
        
    }

}
