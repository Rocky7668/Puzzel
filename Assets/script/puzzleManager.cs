using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CandyCoded.HapticFeedback;


public class puzzleManager : MonoBehaviour
{
    public static puzzleManager instance;

    public GameObject play;

    public List<Outline> hilightBorder;
    public List<Canvas> hilightBorderLayer;
    public Color hilightColor;
    public List<Puzzle> puzzles; // list of sprite
    public List<Image> box; // box images
    public List<int> couter;
    public List<Sprite> temp;
    public GameObject cols, rows, mainimageObj;
    public int cout, time;
    public TextMeshProUGUI timmerTxt;
    public TextMeshProUGUI pointsTxt;


    public TimerHandler timerHandler;
    public bool isEnterGame;

    public GameObject WinObjectAnimation;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        if (!PlayerPrefs.HasKey("Sound"))
        {
            PlayerPrefs.SetInt("Sound", 1);
            PlayerPrefs.SetInt("Music", 1);
        }

        foreach(var line in hilightBorderLayer)
        {
            line.sortingOrder = 1;
        }
        foreach (var line in hilightBorder)
        {
            line.enabled = false;
            line.effectColor = hilightColor;
        }

        IsSound = PlayerPrefs.GetInt("Sound") == 1 ? true : false;
        IsMusic = PlayerPrefs.GetInt("Music") == 1 ? true : false;

        ChangeSound(true);
        ChangeMusic(true);
    }

    public void OnStartGame()
    {
        if (isEnterGame)
            uimanager.instance.top.SetActive(false);

        tempPointCheck.Clear();
        WinObjectAnimation.SetActive(false);
        cols.SetActive(false);
        rows.SetActive(false);
        mainimageObj.SetActive(false);
        GameManager.instance.HintButton.SetActive(false);
        time = 180;
        temp.Clear();
        cout = 0;
        couter.Clear();
        gamePoints = 0;
        pointsTxt.text = "0";
    }


    public void EnablePlayImage()
    {
        OnStartGame();
        UpdatePoints();

        tempPointCheck.Clear();
        cols.SetActive(true);
        rows.SetActive(true);
        mainimageObj.SetActive(true);
        pointsTxt.text = "0";
        GameManager.instance.HintButton.SetActive(true);
        temp.Clear();
        temp.AddRange(puzzles[uimanager.instance.imgIdx].sprites);

        WinObjectAnimation.SetActive(false);

        for (int i = 0; i < box.Count; i++)
        {
            int num = i;
            box[i].GetComponent<Button>().onClick.RemoveAllListeners();

            box[i].GetComponent<Button>().onClick.AddListener(() => puzzlbox(num));
            temp[i].name = i.ToString();
        }

        SwapImagesRandomly();

        for (int i = 0; i < temp.Count; i++)
        {
            box[i].sprite = temp[i];
        }
        //GameManager.instance.isPraticeMode = false;
    }

    public void SetPuzzelImageForPracticePanel(int Index)
    {
        UpdatePoints();


        tempPointCheck.Clear();
        cols.SetActive(true);
        rows.SetActive(true);
        mainimageObj.SetActive(true);
        GameManager.instance.HintButton.SetActive(true);
        temp.Clear();
        temp.AddRange(puzzles[uimanager.instance.imgIdx].sprites);
        Debug.Log($"puzzles[uimanager.instance.imgIdx].sprites{uimanager.instance.imgIdx}");
        
        WinObjectAnimation.SetActive(false);

        for (int i = 0; i < box.Count; i++)
        {
            int num = i;
            box[i].GetComponent<Button>().onClick.RemoveAllListeners();
            box[i].GetComponent<Button>().onClick.AddListener(() => puzzlbox(num));
            temp[i].name = i.ToString();
        }
        pointsTxt.text = "0";
        SwapImagesRandomly();

        for (int i = 0; i < temp.Count; i++)
        {
            box[i].sprite = temp[i];
        }
    }

    private void OnDisable()
    {
        time = 180;
        timmerTxt.text = "03" + ":" + "00";
        CancelInvoke(nameof(timmer));
        uimanager.instance.top.SetActive(false);
        pointsTxt.text = "0";
    }

    void SwapImagesRandomly()
    {
        for (int i = 0; i < temp.Count; i++)
        {
            int randomIndex = Random.Range(0, temp.Count);
            Sprite t = temp[i];
            temp[i] = temp[randomIndex];
            temp[randomIndex] = t;
        }

    }

    void SwapImagesAtIndices(int index1, int index2)
    {
        if (index1 >= 0 && index1 < box.Count && index2 >= 0 && index2 < box.Count)
        {
            Sprite t = box[index1].sprite;
            box[index1].sprite = box[index2].sprite;
            box[index2].sprite = t;

            Sprite s = temp[index1];
            temp[index1] = temp[index2];
            temp[index2] = t;
            checkWin();
        }
        else
        {
            Debug.LogError("Index out of range");
        }
    }

    void checkWin()
    {
        cout = 0;
        for (int i = 0; i < temp.Count; i++)
        {
            if (temp[i].name == i.ToString())
            {
                cout++;
                //Debug.Log(cout);
            }
        }

        if (cout == 20)
        {
            Debug.Log("You are win");
            trueObjectaudioSource.PlayOneShot(WinClip);
            SocketConnection.instance.SendDataToServer(StaticData.PuzzleEvent.SUBMIT_TIMER.ToString(), SubmitTimerReq(timerHandler.gamePlaySecond));
            WinObjectAnimation.SetActive(true);
            cols.SetActive(false);
            rows.SetActive(false);
        }
    }

    public void Click()
    {
        SocketConnection.instance.SendDataToServer(StaticData.PuzzleEvent.SUBMIT_TIMER.ToString(), SubmitTimerReq(100));
    }

    void puzzlbox(int i)
    {

        if (couter.Count != 2)
        {
            couter.Add(i);
            if (couter.Count == 2)
            {
                foreach (var n in couter)
                {
                    hilightBorder[n].enabled = false;
                    hilightBorderLayer[n].sortingOrder = 1;
                }
                SwapImagesAtIndices(couter[0], couter[1]);
                CheckNumberForPoints(couter[1]);
            }
        }
        else
        {
            couter.Clear();
            //here a yeelo outline  
            hilightBorder[i].enabled = true;
            hilightBorderLayer[i].sortingOrder = 5;
            couter.Add(i);
        }
    }
    void timmer()
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        timmerTxt.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        if (time == 0)
        {
            time = 180;
            play.SetActive(false);
            play.SetActive(true);
        }
        time--;
    }

    string SubmitTimerReq(int sec)
    {
        SubmitTimer submitTimer = new SubmitTimer();
        SubmitTimerData submitTimerData = new SubmitTimerData();
        submitTimerData.completeTime = sec;

        submitTimerData = submitTimer.data;

        return JsonUtility.ToJson(submitTimer);
    }
    public List<int> tempPointCheck = new List<int>();

    public float gamePoints;
    public float addPoints = 1100;

    internal void CheckNumberForPoints(int index)
    {

        if (!tempPointCheck.Contains(index))
        {
            if (temp[index].name == index.ToString())
            {
                tempPointCheck.Add(index);
                Debug.Log("Index ------ " + index + "   ------- Name ---- " + box[index].name);
                box[index].gameObject.GetComponent<Button>().enabled = false;
                gamePoints += addPoints;
                trueObjectaudioSource.PlayOneShot(trueObjectClipp);
            }
            else
            {

                gamePoints -= addPoints;
                HapticFeedback.MediumFeedback();
            }
        }
        else
        {
            if (temp[index].name != index.ToString())
            {
                gamePoints -= addPoints;
                HapticFeedback.MediumFeedback();
            }
        }

        UpdatePoints();
    }

    public void UpdatePoints()
    {
        pointsTxt.text = gamePoints.ToString("F0");
    }

    public AudioSource trueObjectaudioSource;
    public AudioSource BgaudioSource;
    public AudioSource buttonAudioSource;
    public AudioClip trueObjectClipp;
    public AudioClip WinClip;

    public bool _isSound;
    public bool _isMusic;

    public Image SoundImage;
    public Image MusicImage;

    public Sprite OnSound;
    public Sprite OffSound;
    public Sprite OnMusic;
    public Sprite OffMusic;

    public bool IsSound
    {
        get
        {
            return PlayerPrefs.GetInt("Sound") == 1;
        }
        set
        {
            _isSound = value;
            PlayerPrefs.SetInt("Sound", value == true ? 1 : 0);
        }
    }

    public bool IsMusic
    {
        get
        {
            return PlayerPrefs.GetInt("Music") == 1;
        }
        set
        {
            _isMusic = value;
            PlayerPrefs.SetInt("Music", value == true ? 1 : 0);
        }
    }

    public void ChangeSound(bool isOn = false)
    {
        if (!isOn)
            IsSound = !IsSound;

        trueObjectaudioSource.mute = !IsSound;
        buttonAudioSource.mute = !IsSound;
        SoundImage.sprite = IsSound ? OnSound : OffSound;

        Debug.Log("IsSound ---- " + IsSound);
    }

    public void ChangeMusic(bool isOn = false)
    {
        if (!isOn)
            IsMusic = !IsMusic;

        BgaudioSource.mute = !IsMusic;
        MusicImage.sprite = IsMusic ? OnMusic : OffMusic;

        Debug.Log("IsMusic ---- " + IsMusic);
    }
}

[System.Serializable]
public class Puzzle
{
    public List<Sprite> sprites = new List<Sprite>();
}

#region Model Class
[System.Serializable]
public class SubmitTimer
{
    public SubmitTimerData data;
}

public class SubmitTimerData
{
    public int completeTime;
}

#endregion
