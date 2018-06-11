using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L1_113Manager : PuzzlesManager {

	protected override bool CanComplete()
    {
        if (buttonList.Count != needButtonList.Count)
            return false;
        foreach(var button in buttonList){
            if (!needButtonList.Exists((obj) => obj == button))
                return false;
        }
        return true;
    }
}
