using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Selection SelectObject;

    public List<Button> playObjBtns;

    public string myUserId;
    public string token;

    public Image gamePlayImg;
    public Sprite gamePlaySprite;


    public GameObject HintButton;

    public bool isButtonPressed;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        token = PlayerPrefs.GetString("token");
        Debug.Log("Token ------ " + token);
    }

    public void ButtonsOnOff(bool isOpen)
    {
        foreach (var button in playObjBtns)
        {
            button.interactable = isOpen;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUIElement())
            {
                isButtonPressed = true;
                gamePlayImg.sprite = gamePlaySprite;
                gamePlayImg.gameObject.SetActive(true);
                Debug.Log("UI Button IBtn is clicked down");
            }
        }

        // Check if the mouse button is released
        if (Input.GetMouseButtonUp(0) && isButtonPressed)
        {
            isButtonPressed = false;
            gamePlayImg.gameObject.SetActive(false);
            Debug.Log("UI Button IBtn is released");
        }
    }

    public void PracticeMode()
    {
        
        SelectObject.gameObject.SetActive(true);
    }

    public void IBtn()
    {
        gamePlayImg.sprite = gamePlaySprite;
        gamePlayImg.gameObject.SetActive(true);
        Invoke(nameof(GamePlayImgClose), 2f);
    }

    void GamePlayImgClose()
    {
        gamePlayImg.gameObject.SetActive(false);
    }

    public void JoinTableGame()
    {
        SocketConnection.instance.SendDataToServer(StaticData.PuzzleEvent.JOIN_TABLE.ToString(), "");
    }


    private bool IsPointerOverUIElement()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject == HintButton)
            {
                return true;
            }
        }

        return false;
    }
}
