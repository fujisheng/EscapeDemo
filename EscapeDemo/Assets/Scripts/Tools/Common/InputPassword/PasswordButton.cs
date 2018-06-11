using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PasswordButton : MonoBehaviour,IPointerClickHandler ,IPointerDownHandler,IPointerUpHandler{

    public string str;
    public Action<PasswordButton> onButtonClick;
    Text text;
    Color defaultColor;
    bool canClick = true;

    private void Awake()
    {
        text = GetComponent<Text>();
        if (text != null)
            defaultColor = text.color;
    }

    public void OnPointerClick(PointerEventData eventData){
        if (canClick == false)
            return;
        if (onButtonClick != null)
            onButtonClick.Invoke(this);
    }

    public void OnPointerDown(PointerEventData eventData){
        if (text != null)
            text.color = Color.white;
    }

    public void OnPointerUp(PointerEventData eventData){
        if (text != null)
            text.color = defaultColor;
    }

    public void CanClick(bool b){
        canClick = b;
    }
}
