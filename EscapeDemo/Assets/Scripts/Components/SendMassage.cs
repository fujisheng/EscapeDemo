using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SendMassage : MonoBehaviour {

    public UnityEvent onAwake;

    private void Awake()
    {
        onAwake.Invoke();
    }

    public void SendString(string massage){
        string notify = massage.Split(',')[0];
        string args = massage.Split(',')[1];
        Mediator.SendMassage(notify,args);
    }
    public void SendInt(string massage){
        string notify = massage.Split(',')[0];
        int args = int.Parse(massage.Split(',')[1]);
        Mediator.SendMassage(notify, args);
    }

    public void SendMassag(string massage){
        Mediator.SendMassage(massage);
    }
}
