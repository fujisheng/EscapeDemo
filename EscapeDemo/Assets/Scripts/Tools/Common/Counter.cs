using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Counter : MonoBehaviour {

    public int maxNumber = 0;
    public int threshold = 0;
    public float overDelayTime = 0;
    public UnityEvent finishEvents;
    private int number = 0;

    public void Count(){
        if (maxNumber == 0)
            number++;
        else if (maxNumber > 0)
        {
            if (number < maxNumber)
                number++;
        }

        if (number == threshold )
            StartCoroutine(CompleteOfCount());
        Debug.Log("number" + number);
    }

    public void Min(){
        number--;
        if (number <= 0)
            number = 0;
        if (number == threshold)
            StartCoroutine(CompleteOfCount());
        Debug.Log("number" + number);
    }
    public void Clear(){
        number = 0;
    }

    IEnumerator CompleteOfCount(){
        yield return new WaitForSeconds(overDelayTime);
        finishEvents.Invoke();
        Debug.Log("counter  complete");
    }
}
