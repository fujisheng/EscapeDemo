using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Json;
using System;
using UnityEngine.Events;

public class PlotManager :IListener {

    static PlotManager instance;

    List<Plot> allPlot = new List<Plot>();

    Plot nowPlot;
    Dialogue nowDialogue;
    UnityEvent onNowPlotFinish;

    public static PlotManager GetInstance(){
        if (instance == null)
            instance = new PlotManager();
        return instance;
    }

    public void Init(){
        allPlot = JsonFile.ReadFromFile<JsonList<Plot>>("Text/", "plotInfo").ToList();

        Mediator.AddListener(this, "triggerPlot","goNextDialogue");
    }

    public void OnNotify(string notify,object args){
        switch (notify){
            case "triggerPlot":
                Args arg = args as Args;
                TriggerPlot((int)(arg.args[0]),arg.args[1] as UnityEvent);
                Debug.Log("triggerPlot  "+(int)arg.args[0]);
                break;
            case "goNextDialogue":
                GoNext();
                break;
        }
    }

    void TriggerPlot(int id,UnityEvent onPlotFinish){
        Mediator.SendMassage("openView", "plotView");
        nowPlot = allPlot.Find((obj) => obj.id == id);
        nowDialogue = nowPlot.dialogueList[0];
        onNowPlotFinish = onPlotFinish;
        Mediator.SendMassage("updateDialogue", nowDialogue);
    }

    void GoNext(){
        if(!nowPlot.dialogueList.Exists((obj)=>obj.id==nowDialogue.next)){//这个剧情结束了
            if (onNowPlotFinish != null)
                onNowPlotFinish.Invoke();
            nowPlot = null;
            nowDialogue = null;
            onNowPlotFinish = null;
            Mediator.SendMassage("closeView", "plotView");
            return;
        }
        nowDialogue = nowPlot.dialogueList.Find((obj) => obj.id == nowDialogue.next);

        Mediator.SendMassage("updateDialogue", nowDialogue);
    }
}
