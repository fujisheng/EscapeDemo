using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ColliderChain : MonoBehaviour,IPointerClickHandler {

    public List<UnityEvent> eventList = new List<UnityEvent>();
    public bool goNext;
    int index = 0;


    public void OnPointerClick(PointerEventData eventData){
        if (eventList.Count <= index)
            return;
        eventList[index].Invoke();
        if (goNext == true)
            index++;
    }

    public void GoNext(){
        index++;
    }

    public void GoTo(int index){
        this.index = index;
    }
}
