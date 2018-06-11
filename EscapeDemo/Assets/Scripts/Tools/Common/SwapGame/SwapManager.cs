using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwapManager : MonoBehaviour {

    public List<SwapButton> allButton = new List<SwapButton>();
    public string needNumberOder;//用，分割
    List<SwapButton> buttonList = new List<SwapButton>();
    List<string> needNumberList;

    public UnityEvent OnComplete;

    SwapButton button1;
    SwapButton button2;

    private void Awake()
    {
        foreach(var button in allButton){
            button.onButtonClick += OnButtonClick;
        }

        foreach(var button in allButton){
            buttonList.Add(button);
        }

        needNumberList = new List<string>(needNumberOder.Split(','));
    }

    void OnButtonClick(SwapButton button){
        if (button1 == null)
            button1 = button;
        else if(button==button1){
            button1 = null;
            button.Default();
        }
        else if(button2==null){
            button2 = button;
            Swap();
        }
    }

    void Swap(){

        Vector3 button2Pos = button2.transform.localPosition;
        int button2ChildIndex = button2.transform.GetSiblingIndex();
        button2.transform.SetSiblingIndex(button1.transform.GetSiblingIndex());
        button1.transform.SetSiblingIndex(button2ChildIndex);
        button2.transform.localPosition = button1.transform.localPosition;
        button1.transform.localPosition = button2Pos;

        button1.Default();
        button2.Default();

        if(button1.link!=null&&button2.link!=null){
            Vector3 button2LinkPos = button2.link.transform.localPosition;
            int button2LinkChildIndex = button2.link.transform.GetSiblingIndex();
            button2.link.transform.SetSiblingIndex(button1.link.transform.GetSiblingIndex());
            button1.link.transform.SetSiblingIndex(button2LinkChildIndex);
            button2.link.transform.localPosition = button1.link.transform.localPosition;
            button1.link.transform.localPosition = button2LinkPos;

        }

        int button1Index = buttonList.FindIndex((obj) => obj == button1);
        int button2Index = buttonList.FindIndex((obj) => obj == button2);
        buttonList.Insert(button1Index, button2);
        buttonList.RemoveAt(button1Index + 1);
        buttonList.Insert(button2Index, button1);
        buttonList.RemoveAt(button2Index + 1);

        button1 = null;
        button2 = null;

        Complete();
    }

    void Complete(){
        if (needNumberList.Count != buttonList.Count)
            return;
        for (int i = 1; i < needNumberList.Count;i++){
            if (int.Parse(needNumberList[i]) != buttonList[i].number)
            {
                Debug.Log(needNumberList[i] + "  !=  " + buttonList[i].number);
                return;
            }
        }
        OnComplete.Invoke();
    }
}
