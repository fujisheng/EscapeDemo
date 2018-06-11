using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L2_114Manager : PuzzlesManager {

    public List<GameObject> lightList = new List<GameObject>(); 

    protected override bool CanComplete()
    {
        ShowLight(buttonList.Count);
        for (int i = 0; i < buttonList.Count;i++){
            if(buttonList[i]!=needButtonList[i]){
                ShowLight(0);
                Reset();
                return false;
            }
        }
        if(buttonList.Count!=needButtonList.Count){
            return false;
        }
        return true;
    }

    void ShowLight(int number){
        foreach(var light in lightList){
            light.SetActive(false);
        }
        for (int i = 0; i < number;i++){
            lightList[i].SetActive(true);
        }
    }
}
