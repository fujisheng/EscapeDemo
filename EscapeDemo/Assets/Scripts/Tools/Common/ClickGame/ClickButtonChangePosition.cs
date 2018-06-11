using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClickButtonChangePosition : ClickButton {

    public Vector3 moveDistance;
    public Vector3 parentMoveDistance;
    private Vector3 defaultPos;
    private Vector3 parentDefaultPos;
    private bool firstShot = true;

    private void Awake()
    {
        Init();
    }

    protected override void OnClick()
    {
        if (firstShot == true)
        {
            defaultPos = this.gameObject.transform.localPosition;
            if(parentObj!=null)
                parentDefaultPos = parentObj.transform.localPosition;
            firstShot = false;
        }
        if (number == 0)
        {
            firstShot = true;
            transform.DOLocalMove(defaultPos, animationTime, true);
            if (parentObj != null)
                parentObj.transform.localPosition = parentDefaultPos;
            return;
        }
        transform.DOLocalMove(transform.localPosition + moveDistance, animationTime);
        if (parentObj == null)
            return;
        parentObj.transform.localPosition += parentMoveDistance;
    }
    protected override void _Reset()
    {
        firstShot = true;
        this.transform.localPosition = defaultPos;
        if (parentObj == null)
            return;
        parentObj.transform.localPosition = parentDefaultPos;
    }
    public override void OnAllComplete()
    {
        
    }

    //OnInspectorGUI

    private Vector3 From;
    private Vector3 To;
    private int index = 0;
    private int indexP = 0;
    public override void Recording()
    {
        if (index == 0)
        {
            From = this.transform.localPosition;
            index++;
        }
        else if (index == 1)
        {
            To = this.transform.localPosition;
            index--;
            moveDistance = (To - From)/((float)totel - 1f);
        }
    }
    public override void Diaplasis()
    {
        this.transform.localPosition = From;
    }
    public override void RecordingParent()
    {
        if (parentObj == null)
            return;
        if (indexP == 0)
        {
            From = parentObj.transform.localPosition;
            indexP++;
        }
        else if (indexP == 1)
        {
            To = parentObj.transform.localPosition;
            indexP--;
            parentMoveDistance = (To - From)/((float)totel - 1f);
        }
    }
    public override void DiaplasisParent()
    {
        if (parentObj == null)
            return;
        parentObj.transform.localPosition = From;
    }

}
