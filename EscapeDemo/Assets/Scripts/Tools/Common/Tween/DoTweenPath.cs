using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System;
using Tools;

[System.Serializable]
public class WayPoint{
    public UnityEvent inEvents;
    public Vector3 position;
    public float stayTime = 0f;
    public float duration = 1f;
    public UnityEvent outEvents;
    public bool useDefaultData = true;
    public bool showInEvents = false;
    public bool showOutEvents = false;
}

public class DoTweenPath : MonoBehaviour {

    public float defaultDuration = 1f;
    public float defaultStayTime = 0f;
    public int loops= 1;
    public Ease ease = Ease.Linear;
    public LoopType loopType=LoopType.Restart;
    public bool autoPlay = true;
    public bool seal = false;
    public List<WayPoint> wayPoints = new List<WayPoint>();


    void Awake(){
        Init();
        AutoPlay();
    }

    void Init(){
        if (seal == true)
            wayPoints.Add(wayPoints[0]);
    }

    void AutoPlay(){
        if (autoPlay == false)
            return;
        DoPlay();
    }

    public void DoPlay(){
        switch (loopType)
        {
            case LoopType.Restart:
                PlayLoops(true);
                break;
            case LoopType.Yoyo:
                PlayLoops(false);
                break;
            case LoopType.Incremental:
                Debug.Log("不常用，已经弃用");
                break;
        }
    }

    void MoveToPoint(int index,Action onComplete){
        //transform.DOLookAt(transform.TransformDirection(wayPoints[index].position), 0f, AxisConstraint.X,Vector3.up);
        transform.DOLocalMove(wayPoints[index].position, index == 0 ? 0f : wayPoints[index].duration, true)
            .OnComplete(() =>
            {
                if (wayPoints[index].showInEvents == true)
                    wayPoints[index].inEvents.Invoke();
                if(wayPoints[index].stayTime > 0)
                {
                    Timer timer = new Timer (wayPoints[index].stayTime,()=>{
                        if (wayPoints[index].showOutEvents == true)
                            wayPoints[index].outEvents.Invoke();
                        if(onComplete != null)
                            onComplete.Invoke();
                    });
                    timer.Start();
                    return;
                }
                if (wayPoints[index].showOutEvents == true)
                    wayPoints[index].outEvents.Invoke();
                if(onComplete != null)
                    onComplete.Invoke();
            })
            .SetEase(ease);
    }

    void PlayNext(int index){
        MoveToPoint(index, () =>
            {
                if(index < wayPoints.Count-1)
                {
                    index++;
                    PlayNext(index);
                }
                else
                    playOnceCallBack.Invoke();
            });
    }


    Action playOnceCallBack;

    void PlayLoops(bool restart){
        int index = 0;
        int loops = 0;
        PlayNext(index);
        playOnceCallBack += () =>
            {
                loops++;
                if(restart == false)
                    wayPoints.Reverse();
                if(loops < this.loops)
                    PlayNext(index);
            };
    }
}
