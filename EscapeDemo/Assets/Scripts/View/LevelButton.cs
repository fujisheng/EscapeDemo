using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour ,IPointerClickHandler{

    public Level level;
    Image icon;
    GameObject lockObj;

	Color unlockColor=Color.white;
	Color lockColor=Color.gray;

    bool unLock;

    private void Awake()
    {
        icon = transform.Find("icon").GetComponent<Image>();
        lockObj = transform.Find("lock").gameObject;

		icon.color = lockColor;
    }

    public void SetLevel(Level level){
        this.level = level;
        icon.sprite = Resources.Load<Sprite>("Image/Level/" + level.icon);
    }

    public void OnPointerClick(PointerEventData eventData){
        if (unLock == false)
            return;
        Mediator.SendMassage("loadLevel", level);
    }

    public void Unlock(){
        unLock = true;
        lockObj.gameObject.SetActive(false);
		icon.color = unlockColor;
    }
}
