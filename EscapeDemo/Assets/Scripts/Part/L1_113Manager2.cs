using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class L1_113Manager2 : MonoBehaviour {

    public static L1_113Manager2 instance;

    public List<L1_113Button2> allButton = new List<L1_113Button2>();
    public UnityEvent OnComplete;

    private void Awake()
    {
        instance = this;
    }

    public void Complete(){
        foreach(var button in allButton){
            if (button.number != 2 && button.number != 6)
                return;
        }
        OnComplete.Invoke();
    }

    public void EnableAllButton(){
        foreach(var button in allButton)
        {
            button.canClick = true;
        }
    }
}
