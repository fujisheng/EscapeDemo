using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickButtonChangeStrings : ClickButton {
    public string strings;
    List<string> stringList = new List<string>();
    Text text;
    Text parentText;

    private void Awake()
    {
        Init();
        text = GetComponent<Text>();
        if(parentObj != null)
            parentText = parentObj.GetComponent<Text>();
        stringList = new List<string>(strings.Split(','));
        text.text = stringList[0];
        if(parentObj != null)
            parentText.text = stringList[0];
    }
    protected override void OnClick() {
        text.text = stringList[number];
        if(parentObj != null)
            parentText.text = stringList[number];
    }
    public override void OnAllComplete() {
        text.raycastTarget = false;
    }
}
