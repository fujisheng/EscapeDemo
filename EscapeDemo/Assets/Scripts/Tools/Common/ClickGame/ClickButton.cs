using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class ClickButton : MonoBehaviour, IPointerClickHandler {

	public int totel=0;
	public int needNumber=0;
    public int defaultNumber = 0;
    public float animationTime=0;
    public bool thisCompleteCanClick=true;
    public ClickButtonManager manager = null;
    public Button mCollider;
    protected int number = 0;
    public GameObject parentObj = null;
	private bool canClick=true;

    protected void Init(){
        if (mCollider != null)
            mCollider.onClick.AddListener(OnColliderClick);
        number = defaultNumber;
    }

	public void OnPointerClick(PointerEventData eventData){
        if (mCollider != null)
            return;
		if(canClick==false)
		    return;
		StartCoroutine(click());
        Debug.Log("number" + number);
        Debug.Log("needNumber   " + needNumber);
	}

    void OnColliderClick(){
        if (mCollider == null)
            return;
        if (canClick == false)
            return;
        StartCoroutine(click());
        Debug.Log("number" + number);
        Debug.Log("needNumber   " + needNumber);
    }

	protected abstract void OnClick();
    protected virtual void _Reset(){}
    public virtual void OnAllComplete(){}

    //OnInspectorGUI
    public virtual void Recording(){}
    public virtual void Diaplasis(){}
    public virtual void RecordingParent(){}
    public virtual void DiaplasisParent(){}

	IEnumerator click(){
		canClick=false;
        number++;
        if(number==totel)
            number=0;
        OnClick();
        yield return new WaitForSeconds(animationTime);
        //Debug.Log("number    " + number);
        Complete();
        if (manager != null)
            manager.Complete();
            
		canClick=true;
	}
	public bool Complete(){
        if (number == needNumber)
        {
            if (thisCompleteCanClick == true)
                canClick = true;
            else
                canClick = false;
            return true;
        } 
		return false;
	}
	public void DisableButton(){
        canClick=false;
	}
	public void EnableButton(){
		canClick=true;
	}

    public void Reset(){
        number = 0;
        canClick = true;
        _Reset();
    }
}
