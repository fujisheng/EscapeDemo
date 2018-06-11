using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using DG.Tweening;

public class TweenImageFilled : MonoBehaviour {

    private Image image;
    public bool autoPlay = false;
    public float duration = 1f;
    public float fromFillAmount = 0f;
    public float toFillAmount = 1f;
    public LoopType loopType = LoopType.Restart;
    public int loops = 1;
    public UnityEvent onCompleteEvents = null;

    void Awake(){
        image = GetComponent<Image>();
        image.type = Image.Type.Filled;
        if (loops < 1)
            loops = 1;
        if (autoPlay == false)
            return;
        Play();
    }

    public static void Play(Image _image,float _duration,float _fromFillAmount,float _toFillAmount,LoopType _loopType,int _loops,UnityEvent _onCompleteEvents){
        TweenImageFilled imageFilled;
        if (_image.GetComponent<TweenImageFilled>())
            imageFilled = _image.GetComponent<TweenImageFilled>();
        else
            imageFilled = _image.gameObject.AddComponent<TweenImageFilled>();
        imageFilled.image = _image;
        imageFilled.duration = _duration;
        imageFilled.fromFillAmount = _fromFillAmount;
        imageFilled.toFillAmount = _toFillAmount;
        imageFilled.loopType = _loopType;
        imageFilled.loops = _loops;
        imageFilled.onCompleteEvents = _onCompleteEvents;

        imageFilled.Play();
    }

    public static void Play(Image _image,float _duration,float _fromFillAmount,float _toFillAmount,LoopType _loopType,int _loops){
        Play(_image, _duration, _fromFillAmount, _toFillAmount, _loopType, _loops, null);
    }

    public static void Play(Image _image,float _duration,float _fromFillAmount,float _toFillAmount){
        Play(_image, _duration, _fromFillAmount, _toFillAmount, LoopType.Restart, 1);
    }

    public void Play(){
        switch (loopType)
        {
            case LoopType.Restart:
                StartCoroutine(_PlayWithRestart());
                break;
            case LoopType.Yoyo:
                StartCoroutine(_PlayWithYoyo());
                break;
            case LoopType.Incremental:
                Debug.Log("不常用，已弃用");
                break;
        }
    }

    IEnumerator _PlayWithRestart(){
        for (int i = 0; i < loops; i++)
        {
            yield return StartCoroutine(_PlayForward());
        }
        if(onCompleteEvents!=null)
            onCompleteEvents.Invoke();
    }

    IEnumerator _PlayWithYoyo(){
        int i = 0;
        while (true)
        {
            switch (i % 2)
            {
                case 0:
                    yield return StartCoroutine(_PlayForward());
                    break;
                case 1:
                    yield return StartCoroutine(_PlayRevers());
                    break;  
            }
            i++;
            if (i >= loops)
                break;
        }
        if(onCompleteEvents!=null)
            onCompleteEvents.Invoke();
    }

    IEnumerator _PlayForward(){
        image.fillAmount = fromFillAmount;
        while (true)
        {
            image.fillAmount += ((toFillAmount - fromFillAmount) / duration) * Time.deltaTime;
            yield return 0;
            if (toFillAmount > fromFillAmount && image.fillAmount >= toFillAmount)
                break;
            else if (toFillAmount < fromFillAmount && image.fillAmount <= toFillAmount)
                break;
        }
    }

    IEnumerator _PlayRevers(){
        image.fillAmount = toFillAmount;
        while (true)
        {
            image.fillAmount += ((fromFillAmount - toFillAmount) / duration) * Time.deltaTime;
            yield return 0;
            if (image.fillAmount <= fromFillAmount)
                break;
        }
    }
}
