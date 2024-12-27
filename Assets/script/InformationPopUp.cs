using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationPopUp : MonoBehaviour
{
    public TextMeshProUGUI NoticeText;

    private void OnEnable()
    {
        transform.localScale = Vector3.one * 0.3f;
        transform.localPosition = Vector3.zero;
        transform.DOScale(1,0.6f);
        transform.DOLocalMoveY(650, 0.8f).OnComplete(delegate
        {
            DOVirtual.DelayedCall(2f, delegate
            {
                gameObject.SetActive(false);
            });
        });
    }
}
