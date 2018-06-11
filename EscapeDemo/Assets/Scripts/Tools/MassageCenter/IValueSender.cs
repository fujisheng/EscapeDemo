using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IValueSender
{
    object OnGetValue(string valueType);
}