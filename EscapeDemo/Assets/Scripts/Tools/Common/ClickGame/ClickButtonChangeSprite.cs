using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickButtonChangeSprite :ClickButton {

    public List<Sprite> spriteList = new List<Sprite>();
    private Image image;
    private Image Pimage;

    void Awake(){
        Init();
        image = transform.GetComponent<Image>();
        if(parentObj!=null)
            Pimage = parentObj.GetComponent<Image>();

        image.sprite = spriteList[defaultNumber];
        if (parentObj == null)
            return;
        Pimage.sprite = spriteList[defaultNumber];
    }

    protected override void OnClick()
    {
        image.sprite = spriteList[number];
        if (parentObj == null)
            return;
        Pimage.sprite = spriteList[number];
    }
    protected override void _Reset()
    {
        image.sprite = spriteList[0];
        if (parentObj == null)
            return;
        Pimage.sprite = spriteList[0];
    }
    public override void OnAllComplete()
    {
        
    }
}
