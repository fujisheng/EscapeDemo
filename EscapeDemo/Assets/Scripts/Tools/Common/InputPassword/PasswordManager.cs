using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class PasswordManager : MonoBehaviour {

    List<string> stringList = new List<string>();
    public Text displayText;
    public string needString;
    public int length;
    public bool autoComplete;
    public List<Text> displayTexts;
    public UnityEvent completeEvents;
    public List<PasswordButton> buttonList = new List<PasswordButton>();

    Color textDefaultColor;

    private void Awake()
    {
        foreach (var button in buttonList)
        {
            button.onButtonClick += OnButtonClick;
        }
        length = needString.Length;
        if (displayText != null)
            textDefaultColor = displayText.color;
        else if (displayTexts.Count != 0)
            textDefaultColor = displayTexts[0].color;
    }

    public void OnButtonClick(PasswordButton button){
        if (displayText != null)
            displayText.DOFade(1f, 0f);
        else if (displayTexts.Count != 0)
            displayTexts.ForEach((obj) => obj.DOFade(1f, 0f));
        switch(button.str){
            case "backspace":
                if(stringList.Count!=0)
                    stringList.RemoveAt(stringList.Count - 1);
                break;
            case "delete":
                stringList.Clear();
                break;
            case "enter":
                Complete();
                break;
            default:
                stringList.Add(button.str);
                break;
        }
        if(length>0&&stringList.Count>=length){
            //stringList.Clear();
            if (IsComplete() == true)
            {
                UpdataShow();
                if (autoComplete == true)
                    Complete();
            }
            else if(displayText != null){
                buttonList.ForEach((_button)=>_button.CanClick(false));
                displayText.color = Color.red;
                displayText.DOFade(0.5f, 0.2f).SetLoops(3).OnComplete(() => { 
                    stringList.Clear(); 
                    UpdataShow(); 
                    buttonList.ForEach((_button) => _button.CanClick(true));
                    displayText.color = textDefaultColor;
                }); 
            }
            else{
                buttonList.ForEach((_button) => _button.CanClick(false));
				displayTexts[0].color = Color.red;
				displayTexts[0].DOFade(0.5f, 0.2f).SetLoops(3).OnComplete(() =>
				{
					stringList.Clear();
					UpdataShow();
					buttonList.ForEach((_button) => _button.CanClick(true));
					displayTexts[0].color = textDefaultColor;
				});

                displayTexts.ForEach((obj)=> { 
                    if(obj!=displayTexts[0]){
                        obj.color = Color.red;
                        obj.DOFade(0.5f, 0.2f).SetLoops(3).OnComplete(()=>obj.color=textDefaultColor);
                    }});
			}
                
        }
        UpdataShow();
        if(autoComplete==true)
            Complete();
    }

    void UpdataShow(){
        string str = string.Empty;
        foreach(var st in stringList){
            str += st;
        }
        if (displayText != null)
            displayText.text = str;

        if (displayTexts.Count == 0)
            return;
        for (int i = 0; i < displayTexts.Count;i++){
            if (stringList.Count > i)
                displayTexts[i].text = stringList[i];
            else
                displayTexts[i].text = string.Empty;
        }
    }

    void Complete(){
        string str = string.Empty;
        foreach (var st in stringList)
        {
            str += st;
        }
        if (str != needString)
            return;
        Debug.Log("allComplete");
        completeEvents.Invoke();
    }

    bool IsComplete(){
        string str = string.Empty;
        foreach (var st in stringList)
        {
            str += st;
        }
        if (str != needString)
            return false;
        return true;
    }

    public void Reset()
    {
        stringList.Clear();
    }
}
