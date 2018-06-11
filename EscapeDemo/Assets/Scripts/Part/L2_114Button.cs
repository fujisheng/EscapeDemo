using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L2_114Button : PuzzlesButton {

    protected override void OnActive()
    {
        manager.AddToButtonList(this);
    }

    protected override void OnDefault()
    {
        manager.AddToButtonList(this);
    }
}
