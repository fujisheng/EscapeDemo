using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MixProp{
    public int propId;
    public UnityEvent onActive;
    public UnityEvent onDefault;
}

public class MaterialMix : MonoBehaviour,IListener {

    public List<MixProp> mixPropList = new List<MixProp>();

    private void Awake()
    {
        Mediator.AddListener(this,"setMixProp","closeMixProp");
    }

    private void OnDestroy()
    {
        Mediator.RemoveListener(this);
    }

    public void OnNotify(string notify,object args){
        switch(notify){
            case "setMixProp":
                OnSetActiveProp((int)args);
                break;
            case "closeMixProp":
                OnCloseMix((int)args);
                break;
        }
    }

    void OnSetActiveProp(int id){
        if (!mixPropList.Exists((prop) => prop.propId == id))
            return;
        mixPropList.Find((prop) => prop.propId == id).onActive.Invoke();
    }
    void OnCloseMix(int id){
        if (!mixPropList.Exists((prop) => prop.propId == id))
            return;
        mixPropList.Find((prop) => prop.propId == id).onDefault.Invoke();
    }
}
