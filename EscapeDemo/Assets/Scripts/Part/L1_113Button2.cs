using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class L1_113Button2 : MonoBehaviour,IPointerClickHandler {

    public int number = 0;

    public List<L1_113Button2> otherButton = new List<L1_113Button2>();
    public bool canClick = false;

    public void OnPointerClick(PointerEventData data){
        if (canClick == false)
            return;
        foreach(var button in otherButton){
            button.Ratation();
        }
    }

    public void Ratation(){
        transform.DOLocalRotate(new Vector3(0f, 0f, -45f), 0f, RotateMode.LocalAxisAdd).OnComplete(
            () =>{
                number++;
                if (number == 8)
                    number = 0;
                L1_113Manager2.instance.Complete();
            });
    }
}
