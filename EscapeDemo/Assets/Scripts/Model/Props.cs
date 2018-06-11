using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Props:IDeepClone<Props>{

    public int id;
    public string name;
    public string icon;
    public int usageCount;
    public int resultCount;
    public int resultId;
    public PropsType type;
    public ConsumType consumType;

    public Props Clone(){
        Props props = new Props();
        props.id = id;
        props.name = name;
        props.icon = icon;
        props.usageCount = usageCount;
        props.resultId = resultId;
        props.resultCount = resultCount;
        props.type = type;
        props.consumType = consumType;
        return props;
    }
}
