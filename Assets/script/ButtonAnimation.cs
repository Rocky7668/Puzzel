using UnityEngine;
using DG.Tweening;

public class ButtonAnimation : MonoBehaviour
{
    public static ButtonAnimation instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = this;
    }

    public void ButtonPressed()
    {
        gameObject.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.1f).OnComplete(() => gameObject.transform.DOScale(Vector3.one, 0.1f));
    }
}
