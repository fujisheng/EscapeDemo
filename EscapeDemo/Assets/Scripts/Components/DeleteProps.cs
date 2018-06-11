using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteProps : MonoBehaviour {

    public List<int> propsList = new List<int>(); 


    public void Delete(){
        foreach(var props in propsList){
            Mediator.SendMassage("deleteProps", props);
        }
    }
}
