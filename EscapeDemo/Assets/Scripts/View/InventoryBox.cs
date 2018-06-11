using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class InventoryBox : MonoBehaviour ,IPointerClickHandler{

    Image highLight;
    Image icon;
    Text numberText;
    GameObject nameObj;
    Text nameText;
    public List<Props> propList = new List<Props>();
    Props activeProp;

    void Awake()
    {
        highLight = transform.Find("highLight").GetComponent<Image>();
        icon = transform.Find("icon").GetComponent<Image>();
        numberText = transform.Find("numberText").GetComponent<Text>();
        nameObj = transform.Find("name").gameObject;
        nameText = transform.Find("name/Text").GetComponent<Text>();

        highLight.gameObject.SetActive(false);
        icon.gameObject.SetActive(false);
        numberText.gameObject.SetActive(false);
        nameObj.SetActive(false);
    }

    public void UpdateShow(Props activeProp)
    {
        if (propList.Count == 0)
        {
            numberText.gameObject.SetActive(false);
            highLight.gameObject.SetActive(false);
            icon.gameObject.SetActive(false);
            //nameObj.gameObject.SetActive(false);
            return;
        }
        this.activeProp = activeProp;
        icon.gameObject.SetActive(true);
        icon.sprite = Resources.Load<Sprite>("Image/Props/" + propList[0].icon);//TODO 优化
        nameText.text = propList[0].name;
        if (propList.Count < 2)
            numberText.gameObject.SetActive(false);
        else if (propList.Count >= 2)
        {
            numberText.gameObject.SetActive(true);
            numberText.text = propList.Count.ToString();
        }
        if (propList.Last() == activeProp){
            highLight.gameObject.SetActive(true);
            //nameObj.SetActive(true);
        }  
        else{
            highLight.gameObject.SetActive(false);
            //nameObj.gameObject.SetActive(false);
        }
            
    }

    public void OnPointerClick(PointerEventData eventDate)
    {
        if (propList.Count == 0)
            return;
        //nameObj.gameObject.SetActive(true);
        Mediator.SendMassage("playAudio", "select");
        Mediator.SendMassage("setActiveProps", propList.Last());
    }

}
