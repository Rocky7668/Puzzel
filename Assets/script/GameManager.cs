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
    public bool isPraticeMode;
    public int time;
    public Sprite QuestionMark;
    public GameObject QuestionBG;

    public GameObject Image1Border;

    public Animator TextColorAnimation;

    [Space(5f)]

    [SerializeField] internal int currentTime;


    [SerializeField] private bool _isJoinedBefore;



    public AudioSource buttonAudioSource;
    public AudioClip ButtonClick;

    public bool isJoinBefore
    {
        get
        {
            return PlayerPrefs.GetInt("isJoinBefore") == 1;
        }
        set
        {
            int n = value ? 1 : 0;
            PlayerPrefs.SetInt("isJoinBefore", n);
            _isJoinedBefore = value;
        }
    }



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
            {
                NewUIManager.instance.OpenPanel(Panel.Home);
                //uimanager.instance.JoinGamePopup.SetActive(false);
            }
            Onetime = true;
            HintButton.SetActive(true);
        }
        if (isWingame)
        {
            Onetime = false;
        }
        if (time == 0)
        {
            QuestiomarkImage.sprite = null;
            TextColorAnimation.enabled = true;
            QuestionBG.SetActive(true);
            Image1Border.SetActive(false);
            //StartCoroutine(GetPuzzelTexture());
        }
    }

    public void PracticeMode()
    {
        SelectObject.gameObject.SetActive(true);
        TimerObject.SetActive(false);
        //isPraticeMode = true;
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
        isPraticeMode = false;
        uimanager.instance.top.SetActive(false);
        HintButton.SetActive(false);

        //StartCoroutine(GetPuzzelTexture());
        foreach (var item in puzzleManager.instance.box)
        {
            item.sprite = null;
        }



        if (isJoinBefore)
        {
            NewUIManager.instance.OpenPanel(Panel.Play);
            return;
        }
        SocketConnection.instance.SendDataToServer(StaticData.PuzzleEvent.JOIN_TABLE.ToString(), "");
        isJoinBefore = true;
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
    Sprite tempSprite;

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
                if (!isPraticeMode)
                    gamePlaySprite = ConvertTextureToSprite(texture);
                else
                    tempSprite = ConvertTextureToSprite(texture);

                yield return new WaitUntil(() => isStarGame);
                SetImageAfterStartGame();
            }
        }
    }

    public Color DefaultTextColor;
    public void SetImageAfterStartGame()
    {
        if (isPraticeMode)
        {
            RunningGameImage.sprite = tempSprite;
            QuestiomarkImage.sprite = tempSprite;
        }
        else
        {
            RunningGameImage.sprite = gamePlaySprite;
            QuestiomarkImage.sprite = gamePlaySprite;
        }

        QuestionBG.SetActive(false);
        TextColorAnimation.enabled = false;
        TextColorAnimation.GetComponent<Text>().color = DefaultTextColor;

        Vector2 NewSizeDelta = QuestiomarkImage.gameObject.GetComponent<RectTransform>().sizeDelta;
        Image1Border.GetComponent<RectTransform>().sizeDelta = new Vector2(NewSizeDelta.x + 13, NewSizeDelta.y - 27);

        Image1Border.SetActive(true);
        QuestiomarkImage.color = Color.white;
        RunningGamePeriodNumber.text = FormatLastThreeAbove(periodnumber.ToString());
        GameManager.instance.RunningGameTxt.text = "Running Game";
    }

    string FormatLastThreeAbove(string input)
    {
        if (input.Length < 3)
        {
            return input; // Jo string 3 karta ochhi hoy to original return karva
        }

        // Last 3 characters replace karva
        string lastThree = input.Substring(input.Length - 3);
        string remaining = input.Substring(0, input.Length - 3);

        // Output ne line format ma mukva
        string output = "*****" + lastThree;
        return output;
    }

    public void ButtonClickSound()
    {
        buttonAudioSource.PlayOneShot(ButtonClick);
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


