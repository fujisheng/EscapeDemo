using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ClickDisappear : MonoBehaviour,IPointerClickHandler {

    public UnityEvent finishEvents;

    public void OnPointerClick(PointerEventData eventData){
        finishEvents.Invoke();
        Destroy(this.gameObject);
    }
}
