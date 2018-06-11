using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickButtonChangeNumbers : ClickButton {

	public List<int> numberList=new List<int>();
    private Text text;
    private Text parentText;
    private int index = 0;
	void Awake()
	{
        Init();
		needNumber=numberList[0];
        text = GetComponent<Text>();
        if (parentObj == null)
            return;
        parentText = parentObj.GetComponent<Text>();
	}
	protected override void OnClick(){
        text.text = number.ToString();
        if (parentObj == null)
            return;
        parentText.text = number.ToString();
	}

	protected override void _Reset(){
        index = 0;
        needNumber = numberList[0];
        text.text = numberList[0].ToString();
        if (parentObj == null)
            return;
        parentText.text = numberList[0].ToString();
    }

    public override void OnAllComplete(){
        StartCoroutine(AllComplete());
    }
    IEnumerator AllComplete(){
        yield return new WaitForSeconds(0.2f);
        index++;
        if (index <= numberList.Count - 1)
            needNumber = numberList[index];
        else
            DisableButton();
        number = 0;
        text.text = number.ToString();
        if (parentObj == null)
            yield return 0;
        parentText.text = number.ToString();
        EnableButton();
        StopCoroutine(AllComplete());
    }
}
