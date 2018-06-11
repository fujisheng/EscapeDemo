using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackView : BaseView,IListener {

    Button newsButton;
    Button feedbackButton;
    Button submitButton;
    Button closeButton;
    InputField text;
    GameObject feedbackCallback;
    Text feedbackText;

    Image feedbackButtonImage;
    Image newsButtonImage;

    private void Awake()
    {
        newsButton = transform.Find("newsButton").GetComponent<Button>();
        feedbackButton = transform.Find("feedbackButton").GetComponent<Button>();
        submitButton = transform.Find("submitButton").GetComponent<Button>();
        closeButton = transform.Find("closeButton").GetComponent<Button>();
        text = transform.Find("text").GetComponent<InputField>();
        feedbackCallback = transform.Find("feedbackCallback").gameObject;
        feedbackText = feedbackCallback.transform.Find("Text").GetComponent<Text>();

        feedbackButtonImage = feedbackButton.GetComponent<Image>();
        newsButtonImage = newsButton.GetComponent<Image>();

        newsButton.onClick.AddListener(OnNewsButtonClick);
        feedbackButton.onClick.AddListener(OnFeedbackButtonClick);
        submitButton.onClick.AddListener(OnSubmitButtonClick);
        closeButton.onClick.AddListener(OnCloseButtonClick);

        Mediator.AddListener(this,"feedbackSucceed","feedbackFailed");
    }

    public override void OnOpened()
    {
        ShowNews();
    }

    public void OnNotify(string notify,object args){
        switch(notify){
            case "feedbackSucceed":
                StartCoroutine(ShowFeedbackCallback("反馈成功"));
                break;
            case "feedbackFailed":
                StartCoroutine(ShowFeedbackCallback("反馈失败"));
                break;
        }
    }

    void OnNewsButtonClick(){
        ShowNews();
    }

    void OnFeedbackButtonClick(){
        ShowFeedBack();
    }

    void OnSubmitButtonClick(){
        Mediator.SendMassage("feedback",text.text);
    }

    void OnCloseButtonClick(){
        Close();
    }

    void ShowNews(){
        feedbackButtonImage.color = Color.gray;
        newsButtonImage.color = Color.white;
        submitButton.gameObject.SetActive(false);
        text.readOnly = true;
        if (!string.IsNullOrEmpty(Mediator.GetValue("reply").ToString()))
            text.text = Mediator.GetValue("reply").ToString();
    }

    void ShowFeedBack(){
        feedbackButtonImage.color = Color.white;
        newsButtonImage.color = Color.gray;
        submitButton.gameObject.SetActive(true);
        text.readOnly = false;
        text.text = string.Empty;
    }

    IEnumerator ShowFeedbackCallback(string str){
        feedbackCallback.gameObject.SetActive(true);
        feedbackText.text = str;
        yield return new WaitForSeconds(1f);
        feedbackCallback.gameObject.SetActive(false);
    }
}
