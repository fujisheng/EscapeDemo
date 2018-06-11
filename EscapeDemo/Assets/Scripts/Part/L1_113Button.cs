using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L1_113Button : PuzzlesButton {

    public GameObject activeObj;

    private void Awake()
    {
        activeObj.SetActive(false);
    }

    protected override void OnActive()
    {
        activeObj.SetActive(true);
        manager.AddToButtonList(this);
    }

    protected override void OnDefault()
    {
        activeObj.SetActive(false);
        manager.MidFormButtonList(this);
    }
}
