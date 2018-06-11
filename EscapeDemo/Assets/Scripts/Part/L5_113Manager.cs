using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L5_113Manager : PuzzlesManager {

    protected override bool CanComplete()
    {
        if(buttonList.Count!=needButtonList.Count)
        {
            return false;
        }

        for (int i = 0; i < needButtonList.Count;i++){
            if(buttonList[i]!=needButtonList[i])
            {
                Reset();
                return false;
            }
        }
        return true;
    }

    protected override void ResetAction()
    {
        foreach(var button in allButtonList){
            button.OnReset();
        }
    }
}
