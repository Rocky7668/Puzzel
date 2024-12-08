using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;


public class puzzleManager : MonoBehaviour
{
    public static puzzleManager instance;

    public GameObject play;

    public List<Puzzle> puzzles; // list of sprite
    public List<Image> box; // box images
    public List<int> couter;
    public List<Sprite> temp;
    public GameObject cols, rows, mainimageObj;
    public int cout, time;
    public TextMeshProUGUI timmerTxt;
    public TextMeshProUGUI pointsTxt;


    public TimerHandler timerHandler;
    internal bool isEnterGame;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
       
    }

    public void OnStartGame()
    {
        if (isEnterGame)
            uimanager.instance.top.SetActive(false);

        cols.SetActive(false);
        rows.SetActive(false);
        mainimageObj.SetActive(false);
        GameManager.instance.HintButton.SetActive(false);
        time = 180;
        temp.Clear();
        cout = 0;
        couter.Clear();
        gamePoints = 0;
    }


    public void EnablePlayImage()
    {
        OnStartGame();
        UpdatePoints();

        cols.SetActive(true);
        rows.SetActive(true);
        mainimageObj.SetActive(true);
        pointsTxt.text = "0";
        GameManager.instance.HintButton.SetActive(true);
        temp.Clear();
        temp.AddRange(puzzles[uimanager.instance.imgIdx].sprites);
       
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
        GameManager.instance.isPraticeMode = false;


    }

    public void SetPuzzelImageForPracticePanel(int Index)
    {
        UpdatePoints();

        cols.SetActive(true);
        rows.SetActive(true);
        mainimageObj.SetActive(true);
        GameManager.instance.HintButton.SetActive(true);
        temp.Clear();
        temp.AddRange(puzzles[uimanager.instance.imgIdx].sprites);
        Debug.Log($"puzzles[uimanager.instance.imgIdx].sprites{uimanager.instance.imgIdx}");
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
    }

    private void OnDisable()
    {
        time = 180;
        timmerTxt.text = "03" + ":" + "00";
        CancelInvoke(nameof(timmer));
        uimanager.instance.top.SetActive(false);

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
            SocketConnection.instance.SendDataToServer(StaticData.PuzzleEvent.SUBMIT_TIMER.ToString(), SubmitTimerReq(timerHandler.gamePlaySecond));
            cols.SetActive(false);
            rows.SetActive(false);
            DOTween.Sequence().AppendInterval(0.5f).OnComplete(() =>
            {
                //uimanager.instance.WinButtonClick();
            });
        }
    }

    public void Click()
    {
        SocketConnection.instance.SendDataToServer(StaticData.PuzzleEvent.SUBMIT_TIMER.ToString(), SubmitTimerReq(100));
    }

    void puzzlbox(int i)
    {
        Debug.Log("hello");
        Debug.Log("Count ----111 ----- " + couter.Count);
        if (couter.Count != 2)
        {
            couter.Add(i);
            if (couter.Count == 2)
            {
                SwapImagesAtIndices(couter[0], couter[1]);
                CheckNumberForPoints(couter[1]);
                Debug.Log("Count ----222 ----- " + couter.Count);
            }
        }
        else
        {
            couter.Clear();
            couter.Add(i);
        }
        Debug.Log("Count ----333 ----- " + couter.Count);
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
                gamePoints += addPoints;
            }
            else
            {
                Debug.Log("Else  ----------- 11");
                gamePoints -= addPoints;
            }
        }
        else
        {
            if (temp[index].name != index.ToString())
            {
                Debug.Log("Else  ----------- 22");
                gamePoints -= addPoints;
            }
        }

        UpdatePoints();
    }

    public void UpdatePoints()
    {
        pointsTxt.text = gamePoints.ToString("F0");
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
