using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeUpManager:MonoBehaviour,IListener,IValueSender{

    Dictionary<int, Transform> mixDic = new Dictionary<int, Transform>();
    List<int> makeUpList = new List<int>();

    int showingMix;
    int mixPropId;
    int resultCount;
    Props mixingProps;

    private void Awake()
    {
        foreach(Transform child in transform){
            mixDic.Add(int.Parse(child.name.Split('_')[0]), child);
        }

        Mediator.AddListener(this, "setActiveProps","closeMakeUp");
        Mediator.AddValue(this,"mixProp");
    }

    private void OnDestroy()
    {
        Mediator.RemoveListener(this);
    }

    public void OnNotify(string notify,object args){
        switch (notify){
            case "setActiveProps":
                OpenMix(args as Props);
                break;
            case "closeMakeUp":
                OnClose();
                break;
        }
    }

    public object OnGetValue(string valueType){
        switch(valueType){
            case "mixProp":
                return mixPropId;
            default:
                return null;
        }
    }

    void OpenMix(Props props){
        if (props == null)
            return;
        if (props.type != PropsType.Whole && props.type != PropsType.Material)
            return;
        if (showingMix != 0)
            return;
        mixingProps = props;
        transform.localScale = Vector3.one;
        Mediator.SendMassage("openView", "makeUpView");
        switch(props.type){
            case PropsType.Material:
                if (showingMix == props.resultId)
                    return;
                if(!mixDic.ContainsKey(props.resultId)){
                    Debug.Log("没有这个物品的合成界面  " + props.resultId);
                    return;
                }
                mixPropId = props.id;
                Mediator.SendMassage("setMixProp",props.id);
                mixDic[props.resultId].localScale = Vector3.one;
                showingMix = props.resultId;
                break;
            case PropsType.Whole:
                if (!mixDic.ContainsKey(props.id))
                {
                    Debug.Log("没有这个物品的合成界面  " + props.id);
                    return;
                }
                mixDic[props.id].localScale = Vector3.one;
                showingMix = props.id;
                break;
        }
    }

    void OnClose(){
        mixDic[showingMix].localScale = Vector3.zero;
        showingMix = 0;


        transform.localScale = Vector3.zero;
        if(mixingProps.type==PropsType.Whole){
            if (resultCount == mixingProps.resultCount && resultCount != 0){
                Mediator.SendMassage("deleteProps", mixingProps.id);
            }
            Mediator.SendMassage("closeMixProp", mixPropId);
        }
        else if(mixingProps.type==PropsType.Material){
            if(resultCount==mixingProps.resultCount&&resultCount!=0)
            {
                Mediator.SendMassage("getProps", mixingProps.resultId);
                Mediator.SendMassage("deleteProps", mixingProps.id);
            }
            else
                Mediator.SendMassage("closeMixProp", mixPropId);
        }
        Mediator.SendMassage("closeView", "makeUpView");
        Mediator.SendMassage("setActiveProps", null);
        mixingProps = null;
        mixPropId = 0;
        resultCount = 0;
    }

    public void CountResult(){
        resultCount++;
    }

    public void Close(){
        OnClose();
    }
}
