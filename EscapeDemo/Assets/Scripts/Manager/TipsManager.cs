using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Json;

public class TipsManager:IValueSender,IListener {

    static TipsManager instance;

    List<Tips> allTips = new List<Tips>();
    List<Tips> ownTips = new List<Tips>();

    public static TipsManager GetInstance(){
        if (instance == null)
            instance = new TipsManager();
        return instance;
    }

    public void Init(){
        allTips = JsonFile.ReadFromFile<JsonList<Tips>>("Text/", "tipsInfo").ToList();

        Mediator.AddValue(this, "ownTips","nowTips","ownNowTips");
        Mediator.AddListener(this, "getTips","onLoadOwnTipsId","getAllTips","getThisLevelTips");
    }

    public void OnNotify(string notify,object args){
        switch (notify)
        {
            case "getTips":
                GetTips(args as Tips);
                break;
            case "getAllTips":
                foreach(var tips in allTips){
                    if (!ownTips.Contains(tips))
                    {
                        ownTips.Add(tips);
                        Mediator.SendMassage("onOwnTipsUpdate", ownTips);
                    }
                }
                break;
            case "onLoadOwnTipsId":
                List<int> ownTipsId = args as List<int>;
                foreach(var tipsId in ownTipsId){
                    ownTips.Add(allTips.Find((tips) => tips.id == tipsId));
                }
                break;
            case "getThisLevelTips":
                foreach(var tips in allTips){
                    if (tips.levelId == (args as Tips).levelId)
                        GetTips(tips);
                }
                break;
        }
    }

    public object OnGetValue(string valueType){
        switch(valueType){
            case "ownTips":
                return ownTips;
            case "nowTips":
                Level nowLevel = Mediator.GetValue("nowLevel") as Level;
                int nowPart = (int)Mediator.GetValue("nowPart");
                return FindTips(nowLevel, nowPart);
            case "ownNowTips":
                Level nowLevel1 = Mediator.GetValue("nowLevel") as Level;
                int nowPart1 = (int)Mediator.GetValue("nowPart");
                return OwnTips(nowLevel1, nowPart1);
            default:
                return null;
        }
    }

    Tips FindTips(Level level,int partId){
        if (!allTips.Exists((tips) => tips.levelId == level.id))
            return null;

        foreach (var tips in allTips.FindAll((obj) => obj.levelId == level.id))
        {
            List<string> partIdList = new List<string>();
            partIdList = new List<string>(tips.partId.Split(','));
            if (partIdList.Count == 0)//当一关只有一个提示的时候
                return tips;
            else if (partIdList.Contains(partId.ToString()))//当一关有很多提示的时候
                return tips;
        }
        return null;
    }

    bool OwnTips(Level level,int partId){
        if (!ownTips.Exists((tips) => tips.levelId == level.id))
            return false;
        
        foreach(var tips in ownTips.FindAll((obj)=>obj.levelId==level.id)){
            List<string> partIdList = new List<string>();
            partIdList = new List<string>(tips.partId.Split(','));
            if (partIdList.Count == 0)//当一关只有一个提示的时候
                return true;
            else if (partIdList.Contains(partId.ToString()))//当一关有很多提示的时候
                return true;
        }
        return false;
    }

    void GetTips(Tips tips){
        if (!ownTips.Contains(tips))
        {
            if (ownTips.Count == 0)
                ownTips.Add(tips);
            else
                ownTips.Insert(0, tips);
            Mediator.SendMassage("onOwnTipsUpdate", ownTips);
        }
    }
}
