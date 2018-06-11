using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PauseView : BaseView ,IPopUps{

    Button startButton;

    private void Awake()
    {
        startButton = transform.Find("startButton").GetComponent<Button>();

        startButton.onClick.AddListener(OnStartButtonClick);
    }

    void OnStartButtonClick(){
        Mediator.SendMassage("showTopBanner");
        Close();
    }
}
