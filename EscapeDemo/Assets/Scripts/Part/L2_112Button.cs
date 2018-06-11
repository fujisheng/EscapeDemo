using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class L2_112Button : PuzzlesButton {

    Color activeColor = Color.green;
    Color defaultColor = Color.white;

    Image image;

    private void Awake()
    {
        image = transform.GetComponent<Image>();
    }

    protected override void OnActive()
    {
        image.color = activeColor;
        manager.AddToButtonList(this);
    }

    protected override void OnDefault()
    {
        image.color = activeColor;
        manager.AddToButtonList(this);
    }

    public override void OnReset()
    {
        Debug.Log("reset");
        image.color = defaultColor;
    }
}
