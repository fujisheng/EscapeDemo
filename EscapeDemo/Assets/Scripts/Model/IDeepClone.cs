using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDeepClone<T> where T:class
{
    T Clone();
}
