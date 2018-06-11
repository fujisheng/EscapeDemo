using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingView : BaseView,IListener {
    Image progressBar;
    Text text;

    private void Awake()
    {
        progressBar = transform.Find("progressBar/bar").GetComponent<Image>();
        text = transform.Find("progressBar/text").GetComponent<Text>();

        Mediator.AddListener(this,"loadSchedule");
    }

    public void OnNotify(string notify,object args){
        switch(notify){
            case "loadSchedule":
                progressBar.fillAmount = (float)args;
                text.text = ((float)args * 100f).ToString().Split('.')[0] + "%";
                break;
        }
    }
}
