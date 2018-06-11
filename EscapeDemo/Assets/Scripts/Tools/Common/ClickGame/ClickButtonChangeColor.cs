using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickButtonChangeColor : ClickButton {
    public List<Color> colorList = new List<Color>();
	private Image image;
    private Image PImage;
	void Awake()
	{
        Init();
		if(colorList.Count<totel)
		    Debug.Log("colorList  dont have so much color");
		image=transform.GetComponent<Image>();
        if (parentObj == null)
            return;
        PImage = parentObj.GetComponent<Image>();

        image.color = colorList[0];
        if (parentObj == null)
            return;
        PImage.color = colorList[0];

	}
    protected override void OnClick(){
        image.color=colorList[number];
        if (parentObj == null)
            return;
        PImage.color = colorList[number];
	}
    protected override void _Reset()
    {
        image.color = colorList[0];
        if (parentObj == null)
            return;
        PImage.color = colorList[0];
    }
    public override void OnAllComplete()
    {
        
    }
}
