using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class uimanager : MonoBehaviour
{
    public static uimanager instance;
    public GameObject home, play, magic, profile, notification, menu, win, notChipsObj, top, time, winBtnObj, practiceModeTxt, transactionPanel, topPanel, backBtn;
    public static int a;
    public List<GameObject> panels;
    public List<GameObject> mainpanels;

    public List<GameObject> _newPanels;

    public Text usernameTxt, periodTxt;
    public TextMeshProUGUI amountTxt;

    public LoginResponse loginResponse;
    public EntryFeeResponse entryFeeResponse;
    public TextMeshProUGUI GamePointTxt;

    internal int imgIdx = 0;


    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        loginResponse = LoginHandler.instance.loginData;
        usernameTxt.text = loginResponse.data.user.email.Split('@')[0];
        amountTxt.text = loginResponse.data.user.amount.ToString();
        //myUserId = loginResponse.data.user._id;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isMenuopen)
            {
                menu.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-990, 0), 0.5f);
                isMenuopen = false;
            }
        }
    }
    public void HomeButtonBack()
    {
        Application.Quit();
    }
    public void NotificationButton()
    {
        profile.transform.DOScale(Vector3.zero, 0.5f);
        menu.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-990, 0), 0.5f); isMenuopen = false;
        //notification.transform.DOLocalMove(new Vector2(0, 0), 0.5f);
        notification.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
    }
    public bool isMenuopen;
    public void MenuButton()
    {
        isMenuopen = true;
        //menu.transform.DOLocalMove(new Vector2(-615, 0), 0.5f);
        menu.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-190, 0), 0.5f);
        notification.GetComponent<RectTransform>().DOAnchorPos(new Vector2(910, 0), 0.5f);
    }

    public void PlayBtnClick()
    {
        SocketConnection.instance.SendDataToServer(StaticData.PuzzleEvent.ENTRYFEE.ToString(), "");
    }

    public void NotChips()
    {
        notChipsObj.SetActive(true);
        DOVirtual.DelayedCall(2f, () => notChipsObj.SetActive(false));
    }

    public void HomeButtonClick()
    {
        home.SetActive(false);
        play.SetActive(true);
    }
    public void MagicBoxClick()
    {
        magic.SetActive(true);
        home.SetActive(false);
        menu.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-990, 0), 0.5f);
    }
    public void profileButClick()
    {
        //profile.transform.DOScale(Vector3.one, 0.5f);
        profile.SetActive(true);
        //home.SetActive(false);
        play.SetActive(false);
        magic.SetActive(false);
        notification.GetComponent<RectTransform>().DOAnchorPos(new Vector2(910, 0), 0.5f);
        menu.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-990, 0), 0.5f);
        isMenuopen = false;
    }
    public void ProfileBack()
    {
        profile.transform.DOScale(Vector3.zero, 0.5f);
        home.SetActive(true);
    }

    public void WinButtonClick()
    {
        play.SetActive(false);
        win.SetActive(true);
    }
    public void BackButton()
    {
        home.SetActive(true);
        play.SetActive(false);
        magic.SetActive(false);
        //notification.transform.DOLocalMove(new Vector2(1550, 0), 0.5f);
        notification.GetComponent<RectTransform>().DOAnchorPos(new Vector2(910, 0), 0.5f);
        //menu.transform.DOLocalMove(new Vector2(-1550, 0), 0.5f); 
        menu.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-990, 0), 0.5f);
        isMenuopen = false;
        profile.transform.DOScale(Vector3.zero, 0.5f);
        win.SetActive(false);
    }

    public void onChangePanel(int index)
    {
        foreach (GameObject g in panels)
        {   
            g.SetActive(false);
        }
        menu.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-990, 0), 0.5f);
        panels[index].SetActive(true);
    }
    public void onclicmainpanel(int index)
    {
        foreach (GameObject g in mainpanels)
        {
            g.SetActive(false);
        }
    }

    public void SendBtnClick()
    {
        //string whatsappUrl = "https://wa.me/" + phoneNumber + "?text=" + urlEncodedMessage;
        string whatsappUrl = "https://wa.me/";
        Application.OpenURL(whatsappUrl);
    }

    public void OpenNewPanels(int number)
    {
        foreach (var panels in _newPanels)
        {
            panels.SetActive(false);
        }
        menu.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-990, 0), 0.5f);
        _newPanels[number].SetActive(true);
        Screen.orientation = ScreenOrientation.Portrait;
    }
    public void OnScreenResolutionLandScape()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
}

#region ModelClass
[System.Serializable]
public class JoinTableRes
{
    public bool success;
    public string status;
    public string message;
    public int amount;
    public string tableId;
}

[System.Serializable]
public class EntryFeeResponse
{
    public int entryFee;
    public double gstAmount;
    public double totalEntryFee;
}
#endregion
