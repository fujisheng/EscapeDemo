using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainView : BaseView,IListener {

    Button coinButton;
    Button likeButton;
    Button feedbackButton;
    Button shareButton;
    Button settingButton;
    GameObject isReply;
    Text coinText;

    private void Awake()
    {
        coinButton = transform.Find("coinButton").GetComponent<Button>();
        coinText = transform.Find("coinButton/Text").GetComponent<Text>();
        likeButton = transform.Find("functionBar/likeButton").GetComponent<Button>();
        feedbackButton = transform.Find("functionBar/feedbackButton").GetComponent<Button>();
        shareButton = transform.Find("functionBar/shareButton").GetComponent<Button>();
        settingButton = transform.Find("functionBar/settingButton").GetComponent<Button>();
        isReply = feedbackButton.transform.Find("isReply").gameObject;

        coinButton.onClick.AddListener(OnCoinButtonClick);
        likeButton.onClick.AddListener(OnLikeButtonClick);
        feedbackButton.onClick.AddListener(OnFeedbackButtonClick);
        shareButton.onClick.AddListener(OnShareButtonClick);
        settingButton.onClick.AddListener(OnSettingButtonClick);

        Mediator.AddListener(this, "onCoinUpdate");

        coinText.text = Mediator.GetValue("coin").ToString();
    }

    public void OnNotify(string notify,object args){
        switch(notify){
            case "onCoinUpdate":
                coinText.text = args.ToString();
                break;
        }
    }

    public override void OnOpened(){
        string reply = Mediator.GetValue("reply").ToString();
        if (string.IsNullOrEmpty(reply))
        {
            isReply.SetActive(false);
        }
        else
            isReply.SetActive(true);
    }    

    void OnCoinButtonClick(){
        Mediator.SendMassage("openView", "storeView");
    }

    void OnLikeButtonClick(){
        Mediator.SendMassage("openLikeUrl");
    }

    void OnFeedbackButtonClick(){
        Mediator.SendMassage("openView", "feedbackView");
    }

    void OnShareButtonClick(){
        Mediator.SendMassage("share");
    }

    void OnSettingButtonClick(){
        Mediator.SendMassage("openView", "settingView");
    }
}
