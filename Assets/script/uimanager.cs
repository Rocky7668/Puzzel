using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class uimanager : MonoBehaviour
{
    public static uimanager instance;
    public GameObject home, play, magic, profile, notification, menu, win, notChipsObj, top, time, winBtnObj, practiceModeTxt, transactionPanel, topPanel, backBtn, JoinGamePopup;
    public static int a;
    public List<GameObject> panels;
    public List<GameObject> mainpanels;

    public List<GameObject> _newPanels;

    public Text usernameTxt, periodTxt;
    public TextMeshProUGUI amountTxt;

    public LoginResponse loginResponse;
    public EntryFeeResponse entryFeeResponse;
    public TextMeshProUGUI GamePointTxt;

    public GameObject fullscreenNotificationCloseButton;
    public GameObject fullscreenMenuCloseButton;

    public List<GameObject> MenuButtonList;

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
        amountTxt.text = loginResponse.data.user.amount.ToString("F2");
        //myUserId = loginResponse.data.user._id;
    }

    private void Update()
    {
        /*#region Device Back

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isMenuopen)
            {
                menu.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-990, 0), 0.5f);
                isMenuopen = false;
                fullscreenMenuCloseButton.SetActive(false);
                return;
            }
            else if (isNotification)
            {
                notification.GetComponent<RectTransform>().DOAnchorPos(new Vector2(910, 0), 0.5f);
                isNotification = false;
                return;
            }

            if (magic.activeSelf)
            {
                magic.SetActive(false);
                home.SetActive(true);
                return;
            }
            if (profile.activeSelf)
            {
                profile.SetActive(false);
                home.SetActive(true);
                return;
            }
            if (play.activeSelf)
            {
                play.SetActive(false);
                home.SetActive(true);
                top.SetActive(true);
                GameManager.instance.isPraticeMode = false;
                puzzleManager.instance.isEnterGame = false;
                return;
            }
            if (JoinGamePopup.activeSelf)
            {
                JoinGamePopup.SetActive(false);
                return;
            }
            for (int i = 0; i < panels.Count; i++)
            {
                if (panels[i].activeSelf)
                {
                    panels[i].SetActive(false);
                    return;
                }

            }

            for (int i = 0; i < _newPanels.Count; i++)
            {
                if (_newPanels[i].activeSelf)
                {
                    _newPanels[i].SetActive(false);
                    return;
                }

            }

            if (topPanel.activeSelf)
            {
                topPanel.SetActive(false);
                home.SetActive(true);
                return;
            }

            if (transactionPanel.activeSelf)
            {
                transactionPanel.SetActive(false);
                home.SetActive(true);
                return;
            }

            if (win.activeSelf)
            {
                win.SetActive(false);
                home.SetActive(true);
                top.SetActive(true);
                return;
            }

            if (home.activeSelf)
            {
                Application.Quit();
            }
        }
            #endregion*/
    }
    public void HomeButtonBack()
    {
        Application.Quit();
    }
    public bool isNotification;
    public void NotificationButton()
    {
        isNotification = true;
        profile.SetActive(false);
        menu.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-990, 0), 0.5f); isMenuopen = false; fullscreenMenuCloseButton.SetActive(false);
        notification.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
        fullscreenNotificationCloseButton.SetActive(true);
    }
    public bool isMenuopen;
    public void MenuButton()
    {
        isMenuopen = true;
        isNotification = false;
        menu.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-190, 0), 0.5f);
        notification.GetComponent<RectTransform>().DOAnchorPos(new Vector2(910, 0), 0.5f);
        fullscreenMenuCloseButton.SetActive(true);
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
        fullscreenMenuCloseButton.SetActive(false);
        isMenuopen = false;

    }
    public void profileButClick()
    {
        //profile.transform.DOScale(Vector3.one, 0.5f);
        profile.SetActive(true);
        //home.SetActive(false);
        play.SetActive(false);
        magic.SetActive(false);
        home.SetActive(true);
        notification.GetComponent<RectTransform>().DOAnchorPos(new Vector2(910, 0), 0.5f);
        fullscreenMenuCloseButton.SetActive(false);
        //menu.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-990, 0), 0.5f);
        //fullscreenNotificationCloseButton.SetActive(false);
        isMenuopen = false;
    }
    public void ProfileBack()
    {
        //profile.transform.DOScale(Vector3.zero, 0.5f);
        profile.SetActive(false);
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
        
        notification.GetComponent<RectTransform>().DOAnchorPos(new Vector2(910, 0), 0.5f);
      
        menu.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-990, 0), 0.5f);
        isMenuopen = false;
        fullscreenMenuCloseButton.SetActive(false);
        fullscreenNotificationCloseButton.SetActive(false);
        isNotification = false;
        profile.SetActive(false);
        win.SetActive(false);
    }

    public void onChangePanel(int index)
    {
        foreach (GameObject g in panels)
        {
            g.SetActive(false);
        }
        //menu.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-990, 0), 0.5f);
        panels[index].SetActive(true);
        Screen.orientation = ScreenOrientation.Portrait;
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
        //menu.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-990, 0), 0.5f);
        //fullscreenMenuCloseButton.SetActive(false);
        _newPanels[number].SetActive(true);
        isMenuopen = false;
        isNotification = false;
        Screen.orientation = ScreenOrientation.Portrait;
    }

    public void OnclickHomeInMenu()
    {
        fullscreenMenuCloseButton.SetActive(false);
        menu.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-990, 0), 0.5f);
        isMenuopen = false;
    }

    public void CloseNotification()
    {
        fullscreenNotificationCloseButton.SetActive(false);
        notification.GetComponent<RectTransform>().DOAnchorPos(new Vector2(910, 0), 0.5f);
        isNotification = false;
    }
    public Color32 homeColor;
    public Color32 highLightColor;

    public void MenuButtonsHighLight(int Index)
    {
        foreach (var item in MenuButtonList)
        {
            item.transform.GetChild(0).GetComponent<Image>().color = Color.white;
            item.transform.GetChild(1).GetComponent<Text>().color = Color.white;
        }
       
        if(Index == 0)
        {
            MenuButtonList[Index].transform.GetChild(0).GetComponent<Image>().color = homeColor;
            MenuButtonList[Index].transform.GetChild(1).GetComponent<Text>().color = Color.white;

        }

        MenuButtonList[Index].transform.GetChild(0).GetComponent<Image>().color = highLightColor;
        MenuButtonList[Index].transform.GetChild(1).GetComponent<Text>().color = highLightColor;

    }

    public void OnScreenResolutionLandScape()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    public void LogOut()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
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
