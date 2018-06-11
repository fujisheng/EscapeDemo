using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class PopUpView : BaseView,IListener,IPopUps {

    Text title;
    Text content;
    Button sureButton;
    Button cancelButton;
    Text sureButtonTitle;
    Text cancelButtonTitle;

    Vector3 sureButtonPos;
    Vector3 cancelButtonPos;

    private void Awake()
    {
        title = transform.Find("title").GetComponent<Text>();
        content = transform.Find("content").GetComponent<Text>();
        sureButton = transform.Find("sureButton").GetComponent<Button>();
        cancelButton = transform.Find("cancelButton").GetComponent<Button>();
        sureButtonTitle = transform.Find("sureButton/Text").GetComponent<Text>();
        cancelButtonTitle = transform.Find("cancelButton/Text").GetComponent<Text>();

        sureButtonPos = sureButton.gameObject.transform.localPosition;
        cancelButtonPos = cancelButton.gameObject.transform.localPosition;

        Mediator.AddListener(this, "showPopUps");
    }

    public void OnNotify(string notify,object args){
        switch(notify){
            case "showPopUps":
                Args arg = args as Args;
                if (arg.args.Length == 6)
                    ShowPopUps(arg.args[0] as string, arg.args[1] as string, arg.args[2] as UnityAction, arg.args[3] as UnityAction, arg.args[4] as string, arg.args[5] as string);
                else if (arg.args.Length == 4 && arg.args[3] is string)
                    ShowPopUps(arg.args[0] as string, arg.args[1] as string, arg.args[2] as UnityAction, arg.args[3] as string);
                else if (arg.args.Length == 4 && arg.args[3] is UnityAction)
                    ShowPopUps(arg.args[0] as string, arg.args[1] as string, arg.args[2] as UnityAction, arg.args[3] as UnityAction);
                else if (arg.args.Length == 3)
                    ShowPopUps(arg.args[0] as string, arg.args[1] as string, arg.args[2] as UnityAction);
                else if(arg.args.Length==2)
                    ShowPopUps(arg.args[0] as string, arg.args[1] as string);
                break;
        }
    }

    public override void OnClosed()
    {
        sureButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        sureButtonTitle.text = "Sure";
        cancelButtonTitle.text = "Cancel";
    }

    void ShowPopUps(string title,string content,UnityAction onSureButtonClick,UnityAction onCancelButtonClick,string sureButtonTitle,string cancelButtonTitle){
        sureButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(true);
        sureButton.transform.localPosition = sureButtonPos;
        cancelButton.transform.localPosition = cancelButtonPos;
        this.title.text = title;
        this.content.text = content;
        this.sureButtonTitle.text = sureButtonTitle;
        this.cancelButtonTitle.text = cancelButtonTitle;
        sureButton.onClick.AddListener(onSureButtonClick);
        cancelButton.onClick.AddListener(onCancelButtonClick);
    }

    void ShowPopUps(string title,string content,UnityAction onSureButtonClick,string sureButtonTitle){
        sureButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(false);
        sureButton.transform.localPosition = new Vector3(0, sureButtonPos.y, 0);
        this.title.text = title;
        this.content.text = content;
        this.sureButtonTitle.text = sureButtonTitle;
        sureButton.onClick.AddListener(onSureButtonClick);
    }

    void ShowPopUps(string title, string content, UnityAction onSureButtonClick, UnityAction onCancelButtonClick)
    {
        sureButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(true);
        sureButton.transform.localPosition = sureButtonPos;
        cancelButton.transform.localPosition = cancelButtonPos;
        this.title.text = title;
        this.content.text = content;
        sureButton.onClick.AddListener(onSureButtonClick);
        cancelButton.onClick.AddListener(onCancelButtonClick);
    }

    void ShowPopUps(string title, string content, UnityAction onSureButtonClick)
    {
        sureButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(false);
        sureButton.transform.localPosition = new Vector3(0, sureButtonPos.y, 0);
        this.title.text = title;
        this.content.text = content;
        sureButton.onClick.AddListener(onSureButtonClick);
    }

    void ShowPopUps(string title, string content)
    {
        sureButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(false);
        sureButton.transform.localPosition = new Vector3(0, sureButtonPos.y, 0);
        this.title.text = title;
        this.content.text = content;
    }
}
