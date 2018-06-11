using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StoreView : BaseView,IPopUps,IListener {

    Button coin1Button;
    Button watchAdButton;
    Button likeButton;
    Button closeButton;

    GameObject watchAd;
    Text timerText;
    Text coin1Price;

    Vector3 watchAdButtonTrans;
    Vector3 watchAdButtonTrans2;

    private void Awake()
    {
        coin1Button = transform.Find("coin1Button").GetComponent<Button>();
        likeButton = transform.Find("likeButton").GetComponent<Button>();
        watchAdButton = transform.Find("watchAdButton").GetComponent<Button>();
        closeButton = transform.Find("closeButton").GetComponent<Button>();
        watchAd = transform.Find("watchAdButton/watchAd").gameObject;
        timerText = transform.Find("watchAdButton/timerText").GetComponent<Text>();
        coin1Price = transform.Find("coin1Button/price").GetComponent<Text>();

        coin1Button.onClick.AddListener(OnCoin1ButtonClick);
        likeButton.onClick.AddListener(OnLikeButtonClick);
        watchAdButton.onClick.AddListener(OnWatchAdButtonClick);
        closeButton.onClick.AddListener(OnCloseButtonClick);

        watchAdButtonTrans = watchAdButton.transform.localPosition;
        watchAdButtonTrans2 = new Vector3(0, watchAdButtonTrans.y, 0);

        Mediator.AddListener(this, "onReviewedUpdate","watchAdTimeOut","onRewardAdRewarded");
    }

    public void OnNotify(string notify,object args){
        switch (notify){
            case "onReviewedUpdate":
                if (args.ToString() == "True"){
                    likeButton.gameObject.SetActive(false);
                    watchAdButton.transform.localPosition = watchAdButtonTrans2;
                }
                else{
				likeButton.gameObject.SetActive(true);
                    watchAdButton.transform.localPosition = watchAdButtonTrans;
                }
                    
                break;
            case "watchAdTimeOut":
                watchAd.gameObject.SetActive(true);
                timerText.gameObject.SetActive(false);
                break;
            case "onRewardAdRewarded":
                Mediator.SendMassage("addCoin", 1);
                watchAd.gameObject.SetActive(false);
                timerText.gameObject.SetActive(true);
                Mediator.SendMassage("startWatchAdTimer", timerText);
                break;
        }
    }

    public override void OnOpened()
    {
        Mediator.SendMassage("inStore", true);
        //更新watchAdButton显示
        if (Mediator.GetValue("watchAdTimeOut").ToString() == "True")
        {
            watchAd.gameObject.SetActive(true);
            timerText.gameObject.SetActive(false);
        }
        else
        {
            watchAd.gameObject.SetActive(false);
            timerText.gameObject.SetActive(true);
            Mediator.SendMassage("updateWatchAdTimer", timerText);
        }

        //更新likeButton显示

        if (Mediator.GetValue("reviewed").ToString() == "False" &&
           Mediator.GetValue("showInters").ToString() == "True")
        {
			likeButton.gameObject.SetActive(true);
        }
        else
        {
            likeButton.gameObject.SetActive(false);
        }

        //更新价格显示
        string price1 = Mediator.GetValue("coin1Price").ToString();
        if (price1.Length >= 7)
        {
            coin1Price.text = price1.Substring(2);
        }
        else
        {
            coin1Price.text = price1;
        }
    }

    public override void OnClosed()
    {
        Mediator.SendMassage("inStore", false);
    }

    void OnCoin1ButtonClick(){
        Mediator.SendMassage("buyProduct", (Mediator.GetValue("productList")as List<Product>)[0].id);
    }

    void OnLikeButtonClick(){
        Mediator.SendMassage("openLikeUrl");
        Mediator.SendMassage("addCoin", 2);
    }

    void OnWatchAdButtonClick(){
        if (Mediator.GetValue("watchAdTimeOut").ToString() == "False")
        {//时间还没到
            Mediator.SendMassage("openView", "gettingVideoView");
        }
        else if ((int)Mediator.GetValue("rewardNumber") >= 20)//一天最多可以看20个
            PopUpsManager.ShowPopUps(LanguageManager.GetInstance().GetString("getVideoFailed"),
                                     LanguageManager.GetInstance().GetString("adReached"),
                                     PopUpsManager.HidePopUps,
                                     LanguageManager.GetInstance().GetString("cancel")
                                    );
        else if(Mediator.GetValue("rewardAdIsReady").ToString()=="True"){//时间到了但是视频没有准备好
            Mediator.SendMassage("showRewardAd");
			Mediator.SendMassage ("startWatchAdTimer");
        }
        else{
            Mediator.SendMassage("openView", "gettingVideoView");
        }
    }

    void OnCloseButtonClick(){
        Close();
    }
}
