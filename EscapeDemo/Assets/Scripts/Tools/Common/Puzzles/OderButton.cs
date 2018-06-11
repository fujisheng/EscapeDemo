using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OderButton : PuzzlesButton {

	public UnityEvent OnClick;

    protected override void OnActive()
    {
        manager.AddToButtonList(this);
		OnClick.Invoke ();
    }

    protected override void OnDefault()
    {
        manager.AddToButtonList(this);
		OnClick.Invoke ();
    }
}
