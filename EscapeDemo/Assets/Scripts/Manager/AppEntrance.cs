using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Code;

public class AppEntrance : MonoBehaviour,IListener {

    bool inGame;
    bool inStore;
    public bool debug;
    CodeProgress progress;

    private void Awake()
    {
        Mediator.AddListener(this, "inGame", "outGame","inStore");
    }

    private void Start()
    {
        progress = new CodeProgress(
            LanguageManager.GetInstance().Init,
            AudioManager.GetInstance().Init,
            StoreManager.GetInstance().Init,
            IAPManager.GetInstance().Init,
            UrlManager.GetInstance().Init,
            LocalAdManager.GetInstance().Init,
            ADManager.GetInstance().Init,
            TipsManager.GetInstance().Init,
            InventoryManager.GetInstance().Init,
            PlotManager.GetInstance().Init,
            MoreGameManager.GetInstance().Init,
            FeedbackManager.GetInstance().Init,
            ShareManager.GetInstance().Init,
            DataManager.GetInstance().Init,
            DataManager.GetInstance().LoadFile
        );
        progress.onProgress += OnProgress;
        StartCoroutine(Init());
    }

    public void OnNotify(string notify,object args){
        switch (notify){
            case "inGame":
                inGame = true;
                break;
            case "outGame":
                inGame = false;
                break;
            case "inStore":
                if (args.ToString() == "True")
                    inStore = true;
                else
                    inStore = false;
                break;
        }
    }

    IEnumerator Init(){
        Mediator.SendMassage("openView", "loadView");
        yield return new WaitForSeconds(0.1f);
        yield return progress.Excute();
        yield return 0;
        Mediator.SendMassage("openView", "startMenuView");
        if (debug == true)
        {
            Mediator.SendMassage("openView", "testModelView");
        }
        Mediator.SendMassage("playBgAudio");
    }

    void OnProgress(float f){
        Mediator.SendMassage("loadSchedule",f);
    }

    private void OnApplicationPause(bool pause)
    {
        if (inGame == false)
            return;
        else if (inGame == true && inStore == true)
            return;
        if(pause==true){
            Mediator.SendMassage("showCubeBanner");
            Mediator.SendMassage("openView", "pauseView");
        }
    }
}
