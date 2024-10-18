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
    public GameObject play;
    public List<Puzzle> puzzles;
    public List<Image> box;
    public List<int> couter;
    public List<Sprite> temp;
    public GameObject cols, rows;
    public int cout, time;
    public TextMeshProUGUI timmerTxt;




    public TimerHandler timerHandler;

    private void OnEnable()
    {
        uimanager.instance.top.SetActive(false);
        time = 180;
        temp.Clear();
        cout = 0;
        couter.Clear();
        temp.AddRange(puzzles[0].sprites);
        for (int i = 0; i < box.Count; i++)
        {
            int num = i;
            box[i].GetComponent<Button>().onClick.AddListener(() => puzzlbox(num));
            temp[i].name = i.ToString();
        }
        SwapImagesRandomly();

        for (int i = 0; i < temp.Count; i++)
        {
            box[i].sprite = temp[i];
        }
        //InvokeRepeating(nameof(timmer), 1, 1
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

        if (cout == 30)
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
        if (couter.Count != 2)
        {
            couter.Add(i);
            if (couter.Count == 2)
            {
                SwapImagesAtIndices(couter[0], couter[1]);
            }
        }
        else
        {
            couter.Clear();
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


    internal IEnumerator GetPuzzelSprites()
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture("url");
        yield return request.SendWebRequest();

        // Check if the request has an error
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            // Get the downloaded texture

            string s = request.downloadHandler.text;

            JObject json = JObject.Parse(s);
            JArray array = (JArray)json["sprites"];

            int count = array.Count;
            puzzles[0].sprites.Clear();

            foreach (var sprites in array)
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

                // Convert the texture to a sprite
                Sprite sprite = ConvertTextureToSprite(texture);

                puzzles[0].sprites.Add(sprite); 
            }
        }
    }

    private Sprite ConvertTextureToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

}
[System.Serializable]
public class Puzzle
{
    public List<Sprite> sprites;
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
