using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlotView : BaseView ,IListener,IPopUps{

    Image npc1;
    Image npc2;
    Button wordsButton;
    Text words;
    bool canClick = true;

    private void Awake()
    {
        npc1 = transform.Find("npc1").GetComponent<Image>();
        npc2 = transform.Find("npc2").GetComponent<Image>();
        wordsButton = transform.Find("words").GetComponent<Button>();
        words = transform.Find("words/Text").GetComponent<Text>();

        wordsButton.onClick.AddListener(OnWordsClick);

        Mediator.AddListener(this, "updateDialogue");
    }

    public void OnNotify(string notify,object args){
        switch (notify){
            case "updateDialogue":
                UpdateShow(args as Dialogue);
                break;
        }
    }

    void UpdateShow(Dialogue dialogue){
        words.text = string.Empty;
        string wordsStr = string.Empty;
        if (string.IsNullOrEmpty(LanguageManager.GetInstance().GetString(dialogue.str)))
            wordsStr = dialogue.str;
        else
            wordsStr = LanguageManager.GetInstance().GetString(dialogue.str);
        words.DOText(wordsStr, 0.3f).SetEase(Ease.Linear).OnComplete(() => canClick = true);
    }

    void OnWordsClick(){
        if (canClick == false)
            return;
        Mediator.SendMassage("goNextDialogue");
        canClick = false;
    }
}
