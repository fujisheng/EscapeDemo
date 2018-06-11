using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class PuzzlesButton : MonoBehaviour ,IPointerClickHandler{

    public PuzzlesManager manager;
    public GameObject parentObj;
    private bool active = false;

    bool canClick = true;

    void Awake(){
        manager.OnDisableButton += () =>
        {
            canClick = false;
        };
        manager.OnReset += () =>
        {
                active=false;
                OnReset();
        };
        manager.OnComplete += OnAllComplete;
    }

    public void OnPointerClick(PointerEventData eventData){
        if (canClick == false)
            return;
        if (active == false)
        {
            active = true;
            OnActive();
        }
        else
        {
            active = false;
            OnDefault();
        }
    }

    public void CanClick(bool can){
        canClick = can;
    }
    protected abstract void OnActive();
    protected abstract void OnDefault();
    public virtual void OnReset(){}
    protected virtual void OnAllComplete(){}
}
