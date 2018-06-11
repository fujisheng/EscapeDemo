using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SwapButton : MonoBehaviour,IPointerClickHandler {

    Outline outline;

    public Action<SwapButton> onButtonClick;
    public GameObject link;
    public int number;

    bool active;

    private void Awake()
    {
        if (transform.GetComponent<Outline>())
            outline = transform.GetComponent<Outline>();
    }

    public void OnPointerClick(PointerEventData data){
        if (active == false){
            if (outline != null)
                outline.enabled = true;
            onButtonClick.Invoke(this);
            active = true;
        }
        else{
            if (outline != null)
                outline.enabled = false;
            onButtonClick.Invoke(this);
            active = false;
        }
    }

    public void Default(){
        active = false;
        if (outline != null)
            outline.enabled = false;
    }
}
