using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinHandler : MonoBehaviour
{
    public List<Sprite> userSprites;
    public List<string> userNames;
    public List<string> userIds;

    public List<WinObjHandler> winObjHandlers;
    public Text oneMinTxt, twoMinTxt, threeMinTxt;

    public WinRes winRes;

    private void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            this.gameObject.SetActive(false);
        }
    }

    public void WinningSet(WinRes winResData)
    {
        winRes = winResData;
        string first = "Complete 1 minute " + winRes.oneMinutePercentage + "% usrs complete fill color.";
        string second = "Complete 2 minute " + winRes.twoMinutePercentage + "% usrs complete fill color.";
        string third = "Complete 3 minute " + winRes.threeMinutePercentage + "% usrs complete fill color.";

        //for (int i = 0; i < winObjHandlers.Count; i++)
        //{
        //    string userName = "";
        //    string userId = "";
        //    int randomNum = Random.Range(0, userSprites.Count);
        //    winObjHandlers[i].WinDataSet(userSprites[randomNum], userName, userId);
        //}

        WinnerDataSet(winRes.topUsers.Count);

        oneMinTxt.text = first;
        twoMinTxt.text = second;
        threeMinTxt.text = third;
    }

    public void WinnerDataSet(int cnt)
    {
        if (cnt == 1)
        {
            int randomNum = Random.Range(0, userSprites.Count);
            winObjHandlers[0].WinDataSet(userSprites[randomNum], "Gaurav", winRes.topUsers[0].id);

            int randomNum2 = Random.Range(0, userSprites.Count);
            winObjHandlers[1].WinDataSet(userSprites[randomNum2], userNames[randomNum2], userIds[randomNum2]);
            int randomNum3 = Random.Range(0, userSprites.Count);
            winObjHandlers[2].WinDataSet(userSprites[randomNum3], userNames[randomNum3], userIds[randomNum3]);
        }
        else if (cnt == 2)
        {
            int randomNum = Random.Range(0, userSprites.Count);
            winObjHandlers[0].WinDataSet(userSprites[randomNum], "Gaurav", winRes.topUsers[0].id);
            int randomNum2 = Random.Range(0, userSprites.Count);
            winObjHandlers[1].WinDataSet(userSprites[randomNum2], "Gaurav", winRes.topUsers[1].id);

            int randomNum3 = Random.Range(0, userSprites.Count);
            winObjHandlers[2].WinDataSet(userSprites[randomNum3], userNames[randomNum3], userIds[randomNum3]);
        }
        else if (cnt == 3)
        {
            int randomNum = Random.Range(0, userSprites.Count);
            winObjHandlers[0].WinDataSet(userSprites[randomNum], "Gaurav", winRes.topUsers[0].id);
            int randomNum2 = Random.Range(0, userSprites.Count);
            winObjHandlers[1].WinDataSet(userSprites[randomNum2], "Gaurav", winRes.topUsers[1].id);
            int randomNum3 = Random.Range(0, userSprites.Count);
            winObjHandlers[2].WinDataSet(userSprites[randomNum3], "Gaurav", winRes.topUsers[2].id);
        }
    }

}

[System.Serializable]
public class WinRes
{
    public List<TopUser> topUsers;
    public int totalUsers;
    public int oneMinuteUsers;
    public int oneMinutePercentage;
    public int twoMinuteUsers;
    public int twoMinutePercentage;
    public int threeMinuteUsers;
    public int threeMinutePercentage;
}

[System.Serializable]
public class TopUser
{
    public string _id;
    public string userId;
    public int gameCompleteTime;
    public int entryFee;
    public string tableId;
    public System.DateTime createdAt;
    public System.DateTime updatedAt;
    public string id;
}

