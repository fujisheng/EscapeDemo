using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public enum PlayType{
    Once,
    Loop,
    PingPong
}

public class SequenceFrameAnimation : MonoBehaviour {

    public PlayType playStyle = PlayType.Once;
    [Range(0f,30f)]public float frameRate = 2f;
    public bool playOnAwake = true;
    public float startDelayTime = 0f;
    public float overDelayTime = 0f;
    public List<Sprite> frameList = new List<Sprite>();
    private Image image;
    public UnityEvent finishEvents;

    void Awake(){
        image = this.GetComponent<Image>();
        if (playOnAwake == true)
            Play();
    }

    public void Play(){
        switch (playStyle)
        {
            case PlayType.Loop:
                StartCoroutine(PlayLoop());
                break;
            case PlayType.Once:
                StartCoroutine(PlayOnce());
                break;
            case PlayType.PingPong:
                StartCoroutine(PlayPingPong());
                break;
            default:
                StartCoroutine(PlayOnce());
                break;
        }
    }

    IEnumerator PlayLoop(){
        if (frameList.Count == 0)
        {
            Debug.LogWarning("frameList is empty !");
            yield return 0;
        }
        yield return new WaitForSeconds(startDelayTime);
        for (int i = 0; i < frameList.Count; i++)
        {
            image.sprite = frameList[i];
            yield return new WaitForSeconds(1f / frameRate);
            if (i == frameList.Count - 1){
                i = 0;
                yield return new WaitForSeconds(overDelayTime);
            }
        }
    }

    IEnumerator PlayOnce(){
        
        if (frameList.Count == 0)
        {
            Debug.LogWarning("frameList is empty !");
            yield return 0;
        }
        yield return new WaitForSeconds(startDelayTime);
        for (int i = 0; i < frameList.Count; i++)
        {
            image.sprite = frameList[i];
            yield return new WaitForSeconds(1f / frameRate);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
        }
        yield return new WaitForSeconds(overDelayTime);
        finishEvents.Invoke();
    }

    IEnumerator PlayPingPong(){
        if (frameList.Count == 0)
        {
            Debug.LogWarning("frameList is empty !");
            yield return 0;
        }
        yield return new WaitForSeconds(startDelayTime);
        bool frist = true;
        for (int i = 0; i < frameList.Count; i++)
        {
            if (frist == true)
                image.sprite = frameList[i];
            else
                image.sprite = frameList[frameList.Count - 1 - i];

            yield return new WaitForSeconds(1f / frameRate); 
            if (i == frameList.Count - 1 && frist == true)
            {
                i = 0;
                frist = false;
            }
            else if (i == frameList.Count - 1 && frist == false)
            {
                i = 0;
                frist = true;
            }
        }
    }
}
