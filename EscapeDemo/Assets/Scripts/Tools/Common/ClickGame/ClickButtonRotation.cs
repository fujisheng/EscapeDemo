using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClickButtonRotation : ClickButton{
    public Vector3 rotationAngle = Vector3.zero;
    private Quaternion defaultRotation;

    void Awake(){
        Init();
        defaultRotation = transform.localRotation;
        number = defaultNumber;
    }
	protected override void OnClick(){
        transform.DOLocalRotate(rotationAngle, animationTime,RotateMode.LocalAxisAdd);
        if (parentObj == null)
            return;
        parentObj.transform.DOLocalRotate(rotationAngle, animationTime,RotateMode.LocalAxisAdd);
	}
    protected override void _Reset()
    {
        this.transform.localRotation = defaultRotation;
        if (parentObj == null)
            return;
        parentObj.transform.localRotation = defaultRotation;
    }
    public override void OnAllComplete()
    {
        
    }
}
