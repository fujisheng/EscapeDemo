using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickButtonsManager : ClickButtonManager {

    public List<UnityEvent> finishedEvents = new List<UnityEvent>();
    private int index = 0;

    void Awake(){
        finishEvents = finishedEvents[0];
    }

    protected override void OnComplete()
    {
        index++;
        if (index <= finishedEvents.Count - 1)
            finishEvents = finishedEvents[index];
        if (index == finishedEvents.Count)
            StartCoroutine(DisabledAllButton());
    }
    IEnumerator DisabledAllButton(){
        yield return new WaitForSeconds(0.2f);
        DisableAllButton();
        StopCoroutine(DisabledAllButton());
    }
}
