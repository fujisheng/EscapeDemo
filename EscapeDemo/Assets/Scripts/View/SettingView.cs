using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingView : BaseView {

    Button restoreButton;
    Button removeAdButton;
    Button soundButton;
    Button moreGameButton;
    Button returnButton;

    Text soundText;
    string soundOn;

    List<Product> productList = new List<Product>(); 

    private void Awake()
    {
        restoreButton = transform.Find("restoreButton").GetComponent<Button>();
        removeAdButton = transform.Find("removeAdButton").GetComponent<Button>();
        soundButton = transform.Find("soundButton").GetComponent<Button>();
        moreGameButton = transform.Find("moreGameButton").GetComponent<Button>();
        returnButton = transform.Find("returnButton").GetComponent<Button>();

        soundText = soundButton.transform.Find("Text").GetComponent<Text>();

        restoreButton.onClick.AddListener(OnRestoreButtonClick);
        removeAdButton.onClick.AddListener(OnRemoveAdButtonClick);
        soundButton.onClick.AddListener(OnSoundButtonClick);
        moreGameButton.onClick.AddListener(OnMoreGameButtonClick);
        returnButton.onClick.AddListener(OnReturnButtonClick);

        productList = Mediator.GetValue("productList") as List<Product>;
    }

    public override void OnOpened(){
        soundOn = Mediator.GetValue("soundOn").ToString();
        if(soundOn=="True")
            soundText.text = LanguageManager.GetInstance().GetString("sound")+":" + LanguageManager.GetInstance().GetString("on");
        else
            soundText.text = LanguageManager.GetInstance().GetString("sound")+":" + LanguageManager.GetInstance().GetString("off");
    }

    void OnRestoreButtonClick(){
        Mediator.SendMassage("restore");
    }

    void OnRemoveAdButtonClick(){
        Mediator.SendMassage("buyProduct", productList.Find((obj) => obj.type == ProductType.RemoveAd).id);
    }

    void OnSoundButtonClick(){
        if(soundOn=="True"){
            soundText.text = LanguageManager.GetInstance().GetString("sound") + ":" + LanguageManager.GetInstance().GetString("off");
            Mediator.SendMassage("soundOff");
        }
        else{
            soundText.text = LanguageManager.GetInstance().GetString("sound") + ":" + LanguageManager.GetInstance().GetString("on");
            Mediator.SendMassage("soundOn");
        }
    }

    void OnMoreGameButtonClick(){
        Mediator.SendMassage("openMoreGameUrl");
    }

    void OnReturnButtonClick(){
        Mediator.SendMassage("openView","startMenuView");
    }
}
