using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OpenPart : MonoBehaviour ,IPointerClickHandler{

    public int id;
    public bool clickToTrigger = true;
    public UnityEvent OnOpened;


    public void OnPointerClick(PointerEventData eventData){
        if (clickToTrigger == false)
            return;
        Mediator.SendMassage("playAudio","openPart");
        Mediator.SendMassage("openPart", id);
        OnOpened.Invoke();
        Mediator.SendMassage("showTimerIntersAd");
    }

    public void Open(){
        Mediator.SendMassage("playAudio", "openPart");
        Mediator.SendMassage("openPart", id);
        OnOpened.Invoke();
        Mediator.SendMassage("showTimerIntersAd");
    }
}
