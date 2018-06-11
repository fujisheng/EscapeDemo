using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Tools.Unity;
using DG.Tweening;

namespace Tools{
    public class Timer
    {
        float threshold;
        Action callBack;
        bool forward;
        bool loop;
        Tweener tweener;
        public Text text;
        public Image image;
        public Action DO;

        float timer = 0f;
        bool stop = false;

        public float Time{
            get { return timer; }
        }

        public Timer(float threshold, bool forward,bool loop,Action DO,Text showText,Image image,Action callBack){
            this.threshold = threshold;
            this.forward = forward;
            this.loop = loop;
            this.callBack = callBack;
            this.text = showText;
            this.image = image;
            this.DO = DO;
        }

        public Timer(float threshold, bool forward, Text showText, Action callBack) : this(threshold, forward, false,null,showText,null, callBack){}

        public Timer(float threshold, bool forward,Image image,Action callBack):this(threshold, forward, false,null,null,image,callBack){}

        public Timer(float threshold,Action callBack):this(threshold, true, false,null,null,null,callBack){}

        public Timer(float threshold, bool loop, Action callBack) : this(threshold, true, loop, null, null, null, callBack){}

        public Timer(float threshold, bool forward,Action DO,Action callBack):this(threshold, forward, false,DO,null,null,callBack){}

        public void Start()
        {
            CoroutineManager.StartCoroutine(_StartTimer());
        }

        public void Reset(){
            timer = 0f;
        }

        public void Stop(){
            tweener.Pause();
            stop = true;
            timer = 0f;
            callBack = null;
            text = null;
            image = null;
            DO = null;
            CoroutineManager.StopCoroutine(_StartTimer());
        }

        public void RemoveListener(){
            callBack = null;
        }

        IEnumerator _StartTimer()
        {
            if (image != null)
                ShowImage();
            while(true){
                yield return new WaitForSeconds(1f);
                timer++;
                if (DO != null)
                    DO.Invoke();
                if (text != null)
                    ShowText();
                if (timer >= threshold){
                    if (callBack != null)
                        callBack.Invoke();
                    if (loop == true)
                        timer = 0f;
                    else
                        break;
                }
                if (stop == true)
                    break;
            }
        }

        void ShowImage(){
            if (forward == true)
            {
                image.fillAmount = 0f;
                tweener = image.DOFillAmount(1, threshold).SetEase(Ease.Linear);
            }
            else
            {
                image.fillAmount = 1f;
                tweener = image.DOFillAmount(0, threshold).SetEase(Ease.Linear);
            }
        }

        void ShowText(){
            int h = 0;
            int m = 0;
            if (forward==true){
                h = (int)timer / 60;
                m = (int)timer % 60;
            }
            else{
                h = (int)(threshold - timer) / 60;
                m = (int)(threshold - timer) % 60;
            }
            text.text = (h < 10 ? "0" + h.ToString() : h.ToString()) +":"+ (m < 10 ? "0" + m.ToString() : m.ToString());
        }
    }
}


