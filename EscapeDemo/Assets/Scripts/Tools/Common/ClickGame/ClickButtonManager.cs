using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickButtonManager : MonoBehaviour {

    public bool OnCompleteDisableClick=true;
    public bool canFinish = true;
    public List<ClickButton> clickButtonList=new List<ClickButton> ();
    public UnityEvent finishEvents;

    protected virtual void OnComplete(){
        
    }

    public bool Complete(){
        foreach (var button in clickButtonList)
        {
            if (button.Complete() == false)
                return false;
        }
        Debug.Log("all Complete");
        if (canFinish == false)
            return false;
        if (OnCompleteDisableClick == true)
            DisableAllButton();
        finishEvents.Invoke();
        foreach (var button in clickButtonList)
        {
            button.OnAllComplete();
        }
        OnComplete();
        return true;
    }
    public void DisableAllButton(){
        foreach (var button in clickButtonList)
        {
            button.DisableButton();
        }
    }

    public void EnableAllButton(){
        foreach (var button in clickButtonList)
        {
            button.EnableButton();
        }
    }
    public void Reset(){
        foreach (var button in clickButtonList)
        {
            button.Reset();
        }
    }

    public void CanFinish(){
        canFinish = true;
    }
}
