using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;

public enum PickType{
    Destory,
    Hide
}

public class PickUpProps : MonoBehaviour,IPointerClickHandler {

    public int propsId;
    public bool clickToTrigger = true;
    public GameObject target;
    public PickType pickType = PickType.Destory;
    public List<GameObject> link = new List<GameObject>();
    public UnityEvent onPickUp;

    Image image;

    private void Awake()
    {
        if (target == null)
            image = transform.GetComponent<Image>();
        else
            image = target.transform.GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData){
        if (clickToTrigger == false)
            return;
        Mediator.SendMassage("playAudio", "pickUp");
        image.DOFade(0, 0.4f).OnComplete(()=>{
            Mediator.SendMassage("getProps", propsId);
            onPickUp.Invoke();
            foreach (var obj in link)
            {
                if (pickType == PickType.Destory)
                    Destroy(obj);
                else
                    obj.SetActive(false);
            }
            if (target == null)
            {
                if (pickType == PickType.Destory)
                    Destroy(gameObject);
                else
                    gameObject.SetActive(false);
            }
            else
            {
                if (pickType == PickType.Destory)
                {
                    Destroy(target.gameObject);
                    Destroy(gameObject);
                }
                else
                {
                    target.gameObject.SetActive(false);
                    gameObject.SetActive(false);
                }
            }

            image.DOFade(1,0);
        });
    }

    public void PickUp(){
        Mediator.SendMassage("playAudio", "pickUp");
        image.DOFade(0, 0.4f).OnComplete(() => {
            Mediator.SendMassage("getProps", propsId);
            onPickUp.Invoke();
            foreach (var obj in link)
            {
                if (pickType == PickType.Destory)
                    Destroy(obj);
                else
                    obj.SetActive(false);
            }
            if (target == null)
            {
                if (pickType == PickType.Destory)
                    Destroy(gameObject);
                else
                    gameObject.SetActive(false);
            }
            else
            {
                if (pickType == PickType.Destory)
                {
                    Destroy(target.gameObject);
                    Destroy(gameObject);
                }
                else
                {
                    target.gameObject.SetActive(false);
                    gameObject.SetActive(false);
                }
            }
            image.DOFade(1, 0);
        });
    }
}
