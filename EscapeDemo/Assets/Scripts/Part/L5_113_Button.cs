using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L5_113_Button : PuzzlesButton {

    GameObject obj;

    private void Awake()
    {
        obj = transform.Find("buttonHight").gameObject;
    }

    protected override void OnActive()
    {
        manager.AddToButtonList(this);
        obj.SetActive(true);
    }

    protected override void OnDefault()
    {
        manager.AddToButtonList(this);
        obj.SetActive(false);
    }

    public override void OnReset()
    {
        obj.SetActive(false);
    }
}
