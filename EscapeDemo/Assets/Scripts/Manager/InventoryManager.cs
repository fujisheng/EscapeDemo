using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Json;

public class InventoryManager :IListener,IValueSender {

    static InventoryManager instance;

    List<Props> allProps = new List<Props>();
    List<Props> ownProps = new List<Props>();
    Props activeProps;

    public static InventoryManager GetInstance(){
        if (instance == null)
            instance = new InventoryManager();
        return instance;
    }

    public void Init(){
        allProps = JsonFile.ReadFromFile<JsonList<Props>>("Text/", "propsInfo").ToList();
        Mediator.AddListener(this, "getProps","setActiveProps","useProps","onLoadedLevel","deleteProps","onLoadOwnPropsId");
        Mediator.AddValue(this, "activeProps");
    }

    public void OnNotify(string notify,object args){
        switch (notify){
            case "getProps":
                ownProps.Add(GetNewProps((int)args));
                Mediator.SendMassage("updateOwnProps", new Args(ownProps, activeProps));
                break;
            case "setActiveProps":
                activeProps = args as Props;
                Mediator.SendMassage("updateOwnProps", new Args(ownProps, activeProps));
                break;
            case "useProps":
                UseProps();
                break;
            case "onLoadedLevel":
                activeProps = null;
                UpdateOwnProps();
                break;
            case "deleteProps":
                Debug.Log(args);
                DeleteProps(int.Parse(args.ToString()));
                break;
            case "onLoadOwnPropsId":
                List<int> idList = args as List<int>;
                idList.ForEach((id) => ownProps.Add(allProps.Find((prop)=>prop.id==id)));
                break;
        }
    }

    public object OnGetValue(string valueType){
        switch (valueType){
            case "activeProps":
                return activeProps;
            default :
                return null;
        }
    }

    Props GetNewProps(int propId){
        if (!allProps.Exists((obj) => obj.id == propId)){
            Debug.Log("没有这个道具  " + propId);
            return null;
        }
        return allProps.Find((obj) => obj.id == propId).Clone();
    }

    void UseProps(){
        if (activeProps.usageCount == 0)
            return;
        activeProps.usageCount--;
        if(activeProps.usageCount==0){
            ownProps.Remove(activeProps);
            if(ownProps.Exists((obj)=>obj.id==activeProps.id) ){//优化体验  当拥有多个相同的物体的时候  可以快速使用
                activeProps = ownProps.FindLast((obj) => obj.id == activeProps.id);
            }
            else
                activeProps = null;
            Mediator.SendMassage("updateOwnProps", new Args(ownProps, activeProps));
        }
    }

    void DeleteProps(int id){
        if (!ownProps.Exists((props) => props.id == id))
            return;
        Props deleteProps = ownProps.Find((props) => props.id == id);
        ownProps.Remove(deleteProps);
        activeProps = null;
        Mediator.SendMassage("updateOwnProps", new Args(ownProps, activeProps));
    }

    void UpdateOwnProps(){
        List<Props> removePropsList = new List<Props>();
        foreach(var prop in ownProps){
            if (prop.consumType == ConsumType.Consum)
                removePropsList.Add(prop);
        }
        foreach(var prop in removePropsList){
            ownProps.Remove(prop);
        }

        Mediator.SendMassage("updateOwnProps", new Args(ownProps, activeProps));
    }
}
