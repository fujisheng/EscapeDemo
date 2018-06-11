using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeUpView : BaseView {

    Button closeButton;

    private void Awake()
    {
        closeButton = transform.Find("closeButton").GetComponent<Button>();

        closeButton.onClick.AddListener(OnCloseButtonClick);
    }

    void OnCloseButtonClick(){
        Mediator.SendMassage("closeMakeUp");
    }
}
