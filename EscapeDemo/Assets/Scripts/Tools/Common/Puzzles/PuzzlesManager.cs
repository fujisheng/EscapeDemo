using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Reflection;

public abstract class PuzzlesManager : MonoBehaviour {

    public bool canFinish = true;
    public bool completeDisableButton = true;

    public List<PuzzlesButton> allButtonList = new List<PuzzlesButton>(); 
    public List<PuzzlesButton> needButtonList = new List<PuzzlesButton> ();
    protected List<PuzzlesButton> buttonList = new List<PuzzlesButton> ();

    public List<PuzzlesButton> ButtonList{
        get{ return buttonList;}
        set{ buttonList = value;}
    }

    public UnityEvent finishEvents;
    public Action OnDisableButton;
    public Action OnComplete;
    public Action OnReset;

    public void CanFinish(){
        canFinish = true;
    }

    protected abstract bool CanComplete();
    protected virtual void AddToButtonListAction(){}
    protected virtual void MidFromButtonListAction(){}
    protected virtual void ResetAction(){}

    void Complete(){
        if (canFinish == false)
            return;
        if (CanComplete() == false)
            return;
        if (completeDisableButton == true)
        {
            if(OnDisableButton!=null)
                OnDisableButton.Invoke();
        }
        if(OnComplete != null)
            OnComplete.Invoke();
        Debug.Log("All  Complete");
        finishEvents.Invoke();
    }

    public void Reset(){
        buttonList.Clear();
        if(OnReset != null)
            OnReset.Invoke();
        ResetAction();
    }

    public void AddToButtonList(PuzzlesButton button){
        buttonList.Add(button);
        AddToButtonListAction();
        Complete();
    }
    public void MidFormButtonList(PuzzlesButton button){
        buttonList.Remove(button);
        MidFromButtonListAction();
        Complete();
    }


}
