using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[System.Serializable]
public class UseList
{
    public int propID = 0;
    public float startDelayTime = 0f;
    public UnityEvent useRsult;
    public float overDelayTime = 0f;
}

public enum UseType
{
    Order,//按照PropList顺序使用
    OutOfOrder//只要在PropList内都能使用
}


public class UsePropsTrigger : MonoBehaviour,IPointerClickHandler {

    public UseType useType = UseType.Order;
    public bool clickToTrigger = true;
    public List<UseList> propList = new List<UseList>();
    bool active = true;
    int index = 0;

    public void OnPointerClick(PointerEventData eventDate)
    {
        if (clickToTrigger == false)
            return;
        UseProps();
    }

    public void UseProps(){
        if (active == false)
            return;
        if (Mediator.GetValue("activeProps") == null)
            return;
        if ((int)Mediator.GetValue("mixProp") == (Mediator.GetValue("activeProps") as Props).id)
            return;
        if (IsNeedThis() == true)
        {
            Debug.Log("isNeedThis");
            Mediator.SendMassage("playAudio", "usePop");
            StartCoroutine(UseRsult());
        }
    }

    int GetNeedID()
    {
        if (propList.Count > index)
            return propList[index].propID;
        else
            return 0;
    }
    List<UseList> GetNeedIDList()
    {
        return propList;
    }
    bool IsNeedThis()
    {
        switch (useType)
        {
            case UseType.Order:
                if (GetNeedID() == 0)
                    return false;
                if ((Mediator.GetValue("activeProps")as Props).id == GetNeedID())
                    return true;
                break;
            case UseType.OutOfOrder:
                for (int i = 0; i < propList.Count; i++)
                {
                    if ((Mediator.GetValue("activeProps") as Props).id == propList[i].propID)
                    {
                        index = i;
                        return true;
                    }
                }
                return false;
        }
        return false;
    }

    IEnumerator UseRsult()
    {
        active = false;
        yield return new WaitForSeconds(propList[index].startDelayTime);
        propList[index].useRsult.Invoke();
        yield return new WaitForSeconds(propList[index].overDelayTime);
        Mediator.SendMassage("useProps");
        active = true;

        switch (useType)
        {
            case UseType.Order:
                index++;
                break;
            case UseType.OutOfOrder:
                break;
        }

    }
}
