using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickButtonChangeNumber : ClickButton {

    private Text text;
    private Text Ptext;

    void Awake(){
        Init();
        if (GetComponent<Text>())
            text = GetComponent<Text>();
        else
            Debug.LogWarning("Dont have Text Component");
        if (parentObj == null)
            return;
        if (parentObj.GetComponent<Text>())
            Ptext = parentObj.GetComponent<Text>();
    }

    protected override void OnClick()
    {
        text.text = number.ToString();
        if (parentObj == null)
            return;
        Ptext.text = number.ToString();
    }
    protected override void _Reset()
    {
        text.text = 0.ToString();
        if (parentObj == null)
            return;
        Ptext.text = 0.ToString();
    }
    public override void OnAllComplete()
    {
        
    }
}
