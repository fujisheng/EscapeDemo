using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class L4_401Puzzle : MonoBehaviour,IListener {

    public List<GameObject> objList = new List<GameObject>();
    public UnityEvent onFinish; 

    List<Props> ownPropsList = new List<Props>(); 

    private void Awake()
    {
        Mediator.AddListener(this,"updateOwnProps");
    }

    public void OnNotify(string notify,object args){
        switch(notify){
            case "updateOwnProps":
                ownPropsList = (args as Args).args[0] as List<Props>;
                UpdateShow();
                break;
        }
    }

    void UpdateShow(){
        int number = ownPropsList.FindAll((obj) => obj.id == 404).Count;

        for (int i = 0; i < number;i++){
            objList[i].SetActive(true);
        }

        if (number == 4)
            onFinish.Invoke();
    }
}
