using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
using System;

public class NewUIManager : MonoBehaviour
{
    public static NewUIManager instance;
    public Panel panelType;

    public List<GameObject> Panels;

    GameObject MiniPanel;

    public GameObject Top;
    public GameObject Bottom;

    public List<Panel> ExitPanelsList;

    public GameObject SolveTimeObjet;
    public GameObject SolveTimeObjectBig;

    public InformationPopUp InformationPopUp;

    internal bool isOtp;

    public ProfileOtpVerification ProfileOtpVerification;

    public GameObject NoInteretPopUp;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isOtp)
        {
            if (panelType == Panel.Home)
            {
                if (uimanager.instance.isMenuopen)
                {
                    uimanager.instance.menu.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-990, 0), 0.5f);
                    uimanager.instance.fullscreenMenuCloseButton.SetActive(false);
                    uimanager.instance.isMenuopen = false;
                    return;
                }
                else if (uimanager.instance.isNotification)
                {
                    uimanager.instance.notification.GetComponent<RectTransform>().DOAnchorPos(new Vector2(910, 0), 0.5f);
                    uimanager.instance.fullscreenNotificationCloseButton.SetActive(false);
                    uimanager.instance.isNotification = false;
                    return;
                }
                else
                {
                    Application.Quit();
                }
            }
            else
            {
                OpenPreviosPanel();
            }
        }
    }

    public void OpenPanel(int Index)
    {
        panelType = (Panel)Index;

        if (panelType == Panel.Home)
        {
            ExitPanelsList.Clear();
            ExitPanelsList.Add(Panel.Home);
        }
        else if (ExitPanelsList.Contains(panelType))
        {
            ExitPanelsList.Remove(panelType);
            ExitPanelsList.Add(panelType);
        }
        else
        {
            if (panelType != Panel.JoinGamePopUp && panelType != Panel.Play && panelType != Panel.PracticeMode)
                ExitPanelsList.Add(panelType);
            else
                ExitPanelsList.Add(Panel.HelperObject);
        }

        foreach (GameObject panel in Panels)
        {
            panel.SetActive(false);
        }
        Panels[Index].SetActive(true);

        SetOriation(Index <= 6 || Index == 21 || Index == 17 || Index == 30 ? 1 : 0);
        TopBottomONOFF(panelType);

        if (panelType == Panel.AddBank || panelType == Panel.AddUPI || panelType == Panel.ListBank || panelType == Panel.ListUPI)
        {
            Panels[(int)Panel.Withdrwal].SetActive(true);      // For Mix Script 
        }
        else if (panelType == Panel.QrPanel)
            Panels[(int)Panel.Deposite].SetActive(true);       // For Mix Script

        if (panelType == Panel.Play)
            uimanager.instance.practiceModeTxt.SetActive(false);
        else
            uimanager.instance.practiceModeTxt.SetActive(true);


        if (panelType == Panel.PracticeMode)
            GameManager.instance.isPraticeMode = true;
        else
            GameManager.instance.isPraticeMode = false;

    }

    void SetOriation(int number)
    {
        switch (number)
        {
            case 0:
                Screen.orientation = ScreenOrientation.Portrait;
                break;
            case 1:
                Screen.orientation = ScreenOrientation.LandscapeLeft;
                break;
        }
    }

    void TopBottomONOFF(Panel panelType)
    {
        if (panelType == Panel.Home)
        {
            Top.SetActive(true);
            Bottom.SetActive(true);
        }
        else if (panelType == Panel.Magic)
        {
            Top.SetActive(true);
            Bottom.SetActive(false);
        }
        else
        {
            Top.SetActive(false);
            Bottom.SetActive(false);
        }
    }

    public void OnOpenHome()
    {
        OpenPanel(0);
    }
    public void OpenPanel(Panel paneltype)
    {
        panelType = paneltype;
        if (panelType == Panel.Home)
        {
            ExitPanelsList.Clear();
            ExitPanelsList.Add(Panel.Home);
        }
        else if (ExitPanelsList.Contains(panelType))
        {
            ExitPanelsList.Remove(panelType);
            ExitPanelsList.Add(panelType);
        }
        else
        {
            if (panelType != Panel.JoinGamePopUp && panelType != Panel.Play && panelType != Panel.PracticeMode)
                ExitPanelsList.Add(panelType);
            else
                ExitPanelsList.Add(Panel.HelperObject);
        }


        foreach (GameObject panel in Panels)
        {
            panel.SetActive(false);
        }
        Panels[(int)paneltype].SetActive(true);

        SetOriation((int)paneltype <= 6 || (int)paneltype == 21 || (int)paneltype == 17 || (int)paneltype == 30 ? 1 : 0);
        TopBottomONOFF(panelType);
        if (panelType == Panel.AddBank || panelType == Panel.AddUPI || panelType == Panel.ListBank || panelType == Panel.ListUPI)
        {
            Panels[(int)Panel.Withdrwal].SetActive(true);      // For Mix Script 
        }
        else if (panelType == Panel.QrPanel)
            Panels[(int)Panel.Deposite].SetActive(true);       // For Mix Script

        if (panelType == Panel.Play)
            uimanager.instance.practiceModeTxt.SetActive(false);
        else
            uimanager.instance.practiceModeTxt.SetActive(true);

        if (panelType == Panel.PracticeMode)
            GameManager.instance.isPraticeMode = true;
        else
            GameManager.instance.isPraticeMode = false;


    }

    public void OpenPreviosPanel()
    {
        if (panelType == Panel.JoinGamePopUp || panelType == Panel.Play)
        {
            uimanager.instance.top.SetActive(true);
            OpenPanel(Panel.Home);
            return;
        }


        ExitPanelsList.Remove(ExitPanelsList[ExitPanelsList.Count - 1]);
        OpenPanel(ExitPanelsList[ExitPanelsList.Count - 1]);
    }

    public void OpenWhatsAppChat(string phoneNumber)
    {
        string whatsappURL = "https://wa.me/" + phoneNumber;

        // Open the URL
        Application.OpenURL(whatsappURL);
    }

    public void Perform(float delay, Action DoWork)
    {
        DOVirtual.DelayedCall(delay, delegate { DoWork?.Invoke(); });
    }

    public void OpenWebsite(string WebLink)
    {
        Application.OpenURL(WebLink);
    }



}

public enum Panel
{
    Home,
    Play,
    PracticeMode,
    Magic,
    PaymentHistory,
    TopPlayer,
    JoinGamePopUp,
    Deposite,
    Withdrwal,
    Wallet,
    Commission,
    Invite,
    TermsAndCondition,
    Setting,
    TdsList,
    Profile,
    OverView,
    Selection,
    Prefrences,
    MyAccount,
    HelperObject,
    Win,
    QrPanel,
    AddBank,
    AddUPI,
    ListBank,
    ListUPI,
    privacyPolicy,
    GSTPolicy,
    Legality,
    GameHistory
}
