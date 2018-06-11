using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickButtonChangeGameObject : ClickButton {

    public List<GameObject> gameObjectList = new List<GameObject> ();
    public List<GameObject> parentObjList = new List<GameObject> ();

    private void Awake()
    {
        Init();
    }

    protected override void OnClick()
    {
        foreach (var obj in gameObjectList)
        {
            obj.SetActive(false);
        }
        gameObjectList[number].SetActive(true);
        if (parentObj == null)
            return;
        foreach (var obj in parentObjList)
        {
            obj.SetActive(false);
        }
        parentObjList[number].SetActive(true);
    }

}
