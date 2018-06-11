using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PassLevel : MonoBehaviour {

    public UnityEvent OnPassLevel;

    public void Pass(){
        Mediator.SendMassage("passLevel");
        Mediator.SendMassage("openView", "passLevelView");
        OnPassLevel.Invoke();
    }
}
