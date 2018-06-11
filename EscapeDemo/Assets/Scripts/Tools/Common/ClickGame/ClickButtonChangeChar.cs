using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickButtonChangeChar : ClickButton {

    private Text text;
    private Text pText;
    private char defaultChar = 'A';

    void Awake(){
        Init();
        text = GetComponent<Text>();
        if (parentObj != null)
            pText = parentObj.GetComponent<Text>();
    }

    protected override void OnClick()
    {
        text.text = ((char)( defaultChar + number)).ToString();
        if (pText)
            pText.text = ((char)(defaultChar + number)).ToString(); 
    }
    protected override void _Reset()
    {
        text.text = defaultChar.ToString();
        if (pText)
            pText.text = defaultChar.ToString(); 
    }
    public override void OnAllComplete()
    {
        
    }
}
