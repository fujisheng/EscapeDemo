using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IListener
{
    void OnNotify(string notify,object args);
}