using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

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

    public ImageData imageData;

    public GameObject TimerObject;

    [Header("===== Running Game =========")]

    public Image RunningGameImage;
    public Image QuestiomarkImage;
    public Text RunningGamePeriodNumber;
    public Text RunningGameTxt;
    public bool isStarGame;
    public bool isWingame;
    public int periodnumber;
    internal bool Onetime;
    internal bool isPraticeMode;
    public int time;
    public Sprite QuestionMark;

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

        if (isWingame) isStarGame = false;

        if (isStarGame && !Onetime)
        {
            StartCoroutine(GetPuzzelTexture());
            if (uimanager.instance.JoinGamePopup.activeInHierarchy)
                uimanager.instance.JoinGamePopup.SetActive(false);
            Onetime = true;
        }
        if (isWingame)
        {
            Onetime = false;
        }
        if (time == 0)
        {
            QuestiomarkImage.sprite = QuestionMark;
            //StartCoroutine(GetPuzzelTexture());
        }
    }

    public void PracticeMode()
    {
        SelectObject.gameObject.SetActive(true);
        TimerObject.SetActive(false);
        isPraticeMode = true;
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
        TimerObject.SetActive(true);
        puzzleManager.instance.isEnterGame = true;
        StartCoroutine(GetPuzzelTexture());
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



    private Sprite ConvertTextureToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    // Get Images From Api

    internal IEnumerator GetPuzzelTexture()
    {
        WWWForm form = new WWWForm();


        using (var api = UnityWebRequest.Post(StaticData.baseURL + StaticData.GetImage, form))
        {
            Debug.Log("<color=white><b>Image Api Calles</b></color>");
            api.SetRequestHeader("Authorization", token);
            yield return api.SendWebRequest();
            if (api.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Data Not Found");
            }
            else
            {
                Debug.Log(api.downloadHandler.text);
                imageData = JsonUtility.FromJson<ImageData>(api.downloadHandler.text);
                StartCoroutine(GenerateTexture());

            }
        }
    }


    public IEnumerator GenerateTexture()
    {
        string url = imageData.data;
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            // Check for errors
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error downloading texture: {request.error}");
            }
            else
            {
                Debug.Log("Texture downloaded successfully!");
                Texture2D texture = DownloadHandlerTexture.GetContent(request);

                // Assign to SpriteCutter or use the texture as needed
                SpriteCutter.instance.spriteToCut = texture;
                SpriteCutter.instance.GenerateAndDisplaySprites(5, 4);
                gamePlaySprite = ConvertTextureToSprite(texture);

                yield return new WaitUntil(() => isStarGame);
                SetImageAfterStartGame();
            }
        }
    }


    public void SetImageAfterStartGame()
    {
        RunningGameImage.sprite = gamePlaySprite;
        QuestiomarkImage.sprite = gamePlaySprite;
        RunningGamePeriodNumber.text = periodnumber.ToString();
        GameManager.instance.RunningGameTxt.text = "Running Game";
    }
}

[System.Serializable]
// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class ImageData
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public string data;
}


