using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OpenOverall : MonoBehaviour,IPointerClickHandler {

    public int id;
    public UnityEvent OnOpened;

    public void OnPointerClick(PointerEventData eventData){
        Mediator.SendMassage("openOverall", id);
        OnOpened.Invoke();
    }
}
