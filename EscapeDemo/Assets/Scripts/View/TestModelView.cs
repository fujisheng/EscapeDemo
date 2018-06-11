using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TestModelView : BaseView {

    Button deleteDataButton;
    Button unlockAllLevelButton;
    Button get1000CoinButton;
    Button getAllTipsButton;
    Button closeButton;
    Button menuButton;

    Transform bg;

    bool isOpen;

    private void Awake()
    {
        deleteDataButton = transform.Find("bg/deleteDataButton").GetComponent<Button>();
        unlockAllLevelButton = transform.Find("bg/unlockAllLevelButton").GetComponent<Button>();
        get1000CoinButton = transform.Find("bg/get1000CoinButton").GetComponent<Button>();
        getAllTipsButton = transform.Find("bg/getAllTipsButton").GetComponent<Button>();
        closeButton = transform.Find("bg/closeButton").GetComponent<Button>();
        menuButton = transform.Find("menuButton").GetComponent<Button>();
        bg = transform.Find("bg");

        deleteDataButton.onClick.AddListener(OnDeleteButtonClick);
        unlockAllLevelButton.onClick.AddListener(OnUnlockAllLevelButtonClick);
        get1000CoinButton.onClick.AddListener(OnGet1000CoinButtonClick);
        getAllTipsButton.onClick.AddListener(OnGetAllTipsButtonClick);
        closeButton.onClick.AddListener(Close);
        menuButton.onClick.AddListener(OnMeunButtonClick);
        bg.DOScale(Vector3.zero, 0f);
    }

    void OnDeleteButtonClick(){
        Mediator.SendMassage("deleteData");
    }

    void OnUnlockAllLevelButtonClick(){
        Mediator.SendMassage("unlockAllLevel");
    }

    void OnGet1000CoinButtonClick(){
        Mediator.SendMassage("addCoin", 1000);
    }

    void OnGetAllTipsButtonClick(){
        Mediator.SendMassage("getAllTips");
    }

    void OnMeunButtonClick(){
        if (isOpen == true){
            bg.DOScale(Vector3.zero, 0f);
            isOpen = false;
        }
        else
        {
            bg.DOScale(Vector3.one, 0f);
            isOpen = true;
        }    
    }
}
