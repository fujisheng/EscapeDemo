using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class MRatation : MonoBehaviour {

    public bool playOnAwake = false;
    public int animationTime;
    public Vector3 angularVelocity = Vector3.zero;
    public UnityEvent finishEvents;

    bool isPlaying;


    void Awake()
    {
        if (playOnAwake == false)
            return;
        Play();
    }

    public void Play()
    {
        if (isPlaying == true)
            return;
        isPlaying = true;
        transform.DORotate(angularVelocity, animationTime == 0 ? 0 : 1, RotateMode.LocalAxisAdd).
                 SetEase(Ease.Linear).
                 SetLoops(animationTime == 0 ? 1 : animationTime).
                 OnComplete(() => { finishEvents.Invoke(); isPlaying = false; });
    }
}
