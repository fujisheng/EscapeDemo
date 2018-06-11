using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

//继承自baseView的类不用在OnDestroy中添加Mediator.RemoveListner方法  这个在UiManager中已经统一处理了

public class BaseView : MonoBehaviour {

    public UnityEvent onOpened;
    public UnityEvent onClosed;

    /// <summary>
    /// 打开一个view的时候执行这个  主要用来实现打开的效果
    /// </summary>
    /// <returns>The open.</returns>
    public virtual Tweener OnOpen(){
        return transform.DOScale(Vector3.one, 0f);//默认的打开效果
    }
    /// <summary>
    /// 打开一个view之后执行这个   主要用来初始化
    /// </summary>
    public virtual void OnOpened(){}

    /// <summary>
    /// 关闭一个view 的时候执行这个   主要用来实现关闭的效果
    /// </summary>
    /// <returns>The close.</returns>
    public virtual Tweener OnClose(){
        return transform.DOScale(Vector3.zero, 0f);//默认的关闭效果
    }

    /// <summary>
    /// 关闭一个view之后执行这个    主要用来实现关闭之后的事
    /// </summary>
    public virtual void OnClosed(){}

    /// <summary>
    /// 提供一个快捷关闭这个view的方法   也可以用UIManager中的方法
    /// </summary>
    public void Close(){
        string s = gameObject.name;//在运行的时候名字会有(Clone)后缀
        Mediator.SendMassage("closeView", s.Substring(0, s.Length - 7));
    }
}
