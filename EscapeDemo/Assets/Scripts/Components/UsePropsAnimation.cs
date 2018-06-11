using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class UsePropsAnimation : MonoBehaviour {

    Image image;
    public bool autoPlay = true;
    public UnityEvent OnUsed;

    private void Awake()
    {
        image = transform.GetComponent<Image>();

        image.color = new Color(1, 1, 1, 0f);
        transform.localScale = new Vector3(2f, 2f, 2f);

        if (autoPlay == true)
            Play();
    }

    public void Play(){
        image.DOFade(1, 0.4f);
        transform.DOScale(Vector3.one, 0.4f).OnComplete(()=>{
            OnUsed.Invoke();
        });
    }
}
