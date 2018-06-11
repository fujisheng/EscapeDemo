using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OderButtonManager : PuzzlesManager {

    protected override bool CanComplete()
    {
        if (buttonList.Count != needButtonList.Count)
        {
            return false;
        }
        for (int i = 0; i < needButtonList.Count; i++)
        {
            if (needButtonList[i] != buttonList[i])
            {
                Flash();
                Reset();
                return false;
            }
        }
        return true;
    }

    void Flash(){
        foreach (var button in allButtonList)
        {
            button.CanClick(false);
            button.GetComponent<Image>().DOFade(0.2f, 0.1f).SetLoops(2).OnComplete(() =>
            {
                button.CanClick(true);
                button.GetComponent<Image>().DOFade(1f, 0f);
            });
        }
    }
}
