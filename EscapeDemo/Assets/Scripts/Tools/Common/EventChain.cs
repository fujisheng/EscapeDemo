using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EventItem{
    public float startDelayTime = 0f;
    public UnityEvent events;
}

public class EventChain : MonoBehaviour {

    public bool executeOnAwake = false;
    public List<EventItem> eventChain = new List<EventItem>();

    void Awake(){
        if (executeOnAwake == false)
            return;
        Execute();
    }

    public void Execute(){
        StartCoroutine(_Execute());
    }

    IEnumerator _Execute(){
        foreach (var eventItem in eventChain)
        {
            yield return new WaitForSeconds(eventItem.startDelayTime);
            eventItem.events.Invoke();
        }
    }
}
