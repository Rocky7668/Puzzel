using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selection : MonoBehaviour
{

    public Image[] AllButtos;
    public Sprite[] AllImg;
    public Sprite[] SliceImg;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectImage(int idx)
    {
        Debug.Log(idx);
        uimanager.instance.imgIdx = idx;
        uimanager.instance.play.SetActive(true);
        uimanager.instance.practiceModeTxt.SetActive(true);
        uimanager.instance.top.SetActive(false);
        uimanager.instance.home.SetActive(false);
        //uimanager.instance.time.SetActive(false);
        uimanager.instance.winBtnObj.SetActive(false);
        uimanager.instance.backBtn.SetActive(true);

        GameManager.instance.ButtonsOnOff(true);
        this.gameObject.SetActive(false);
    }

}
