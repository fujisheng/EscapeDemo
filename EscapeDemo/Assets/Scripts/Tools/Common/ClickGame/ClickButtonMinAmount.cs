using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ClickButtonMinAmount : ClickButton {

    public float minAmount = 0.35f;
    private Image image;
    private Image parentImage;

    private void Awake()
    {
        Init();
        image = GetComponent<Image>();
        if (parentObj != null)
            parentImage = parentObj.GetComponent<Image>();
    }

    protected override void OnClick()
    {
        image.fillAmount -= minAmount;
        if (number == 0)
            image.fillAmount = 1;
        if (parentObj == null)
            return;
        parentImage.fillAmount -= minAmount;
        if (number == 0)
            parentImage.fillAmount = 1;     
    }
}
