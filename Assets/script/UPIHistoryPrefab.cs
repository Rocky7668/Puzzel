using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp;

public class UPIHistoryPrefab : MonoBehaviour
{
    public TextMeshProUGUI UPiID;
    internal int Index;

    public void SetData(string upi, int index)
    {
        UPiID.text = upi;
        Index = index;
    }
}
