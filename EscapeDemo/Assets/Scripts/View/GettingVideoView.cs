using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GettingVideoView : BaseView ,IPopUps{

    Image image;
    Text text;

    private void Awake()
    {
        image = GetComponent<Image>();
        text = transform.Find("Text").GetComponent<Text>();
    }

    public override void OnOpened()
    {
        base.OnOpened();
        image.DOFade(1, 0f);
        text.DOFade(1, 0f);
        text.DOFade(0, 1.5f);
        image.DOFade(0, 1.5f).OnComplete(()=>Close());
    }
}
