using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuView : BaseView {

    Button startButton;
    Button settingButton;

    private void Awake()
    {
        startButton = transform.Find("startButton").GetComponent<Button>();
        settingButton = transform.Find("settingButton").GetComponent<Button>();

        startButton.onClick.AddListener(OnStartButtonClick);
        settingButton.onClick.AddListener(OnSettingButtonClick);
    }

    public override void OnOpened()
    {
        Mediator.SendMassage("hideBanner");
    }

    void OnStartButtonClick(){
        Mediator.SendMassage("openView", "levelSelectPannal");
    }

    void OnSettingButtonClick(){
        Mediator.SendMassage("openView","settingView");
    }
}
