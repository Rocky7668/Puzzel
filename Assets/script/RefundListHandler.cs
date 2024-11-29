using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RefundListHandler : MonoBehaviour
{
    public MainRefundData MainRefund;

    public RefundUsernamePrefab refundUsernamePrefab;
    public RefundAmountPrefab refundAmountPrefab;

    public Transform Parent1, Parent2;

    public void OnEnable()
    {
        MainRefund = new();
        StartCoroutine(GetRefundList());
    }

    IEnumerator GetRefundList()
    {
        WWWForm form = new WWWForm();

        using (var RefundListApi = UnityWebRequest.Post(StaticData.baseURL + StaticData.GetRefundUserList, form))
        {
            RefundListApi.SetRequestHeader("Authorization", GameManager.instance.token);

            yield return RefundListApi.SendWebRequest();
            if (RefundListApi.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Data Not Found : " + RefundListApi.error);
            }
            else
            {
                Debug.Log(RefundListApi.downloadHandler.text);
                MainRefund = JsonUtility.FromJson<MainRefundData>(RefundListApi.downloadHandler.text);
                GenerateUsername();
            }
        }
    }

    List<GameObject> UsernameList = new();
    List<GameObject> AmountList = new();

    public void GenerateUsername()
    {
        ClearAllObjects();
        for (int i = 0; i < MainRefund.data.Count; i++)
        {
            RefundUsernamePrefab refundUsernamePrefabClone = Instantiate(refundUsernamePrefab, Parent1);
            refundUsernamePrefabClone.UsernameTxt.text = MainRefund.data[i].phoneNumber;
            UsernameList.Add(refundUsernamePrefabClone.gameObject);
            GenerateAmountPrefab(i);
        }
    }

    public void GenerateAmountPrefab(int Index)
    {
        RefundAmountPrefab refundAmountPrefabClone = Instantiate(refundAmountPrefab, Parent2);
        refundAmountPrefabClone.AmountTxt.text = MainRefund.data[Index].amount.ToString();
        AmountList.Add(refundAmountPrefabClone.gameObject);
    }

    public void ClearAllObjects()
    {
        for (int i = 0; i < UsernameList.Count; i++)
        {
            Destroy(UsernameList[i].gameObject);
            Destroy(AmountList[i].gameObject);
        }
        UsernameList.Clear();
        AmountList.Clear();
    }

}

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
[System.Serializable]
public class RefundUserData
{
    public string userId;
    public string phoneNumber;
    public int amount;
}

[System.Serializable]
public class MainRefundData
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public List<RefundUserData> data;
}


