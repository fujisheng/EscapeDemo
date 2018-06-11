using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Json;

public class LevelManager : MonoBehaviour,IListener ,IValueSender{

    List<Level> allLevel = new List<Level>();
    List<Level> unlockLevel = new List<Level>();
    Level nowLevel;

    private void Awake()
    {
        allLevel = JsonFile.ReadFromFile<JsonList<Level>>("Text/", "levelInfo").ToList();
        Mediator.AddListener(this, "loadLevel","passLevel","onLoadUnlockLevelId","unlockAllLevel");
        Mediator.AddValue(this, "nowLevel","allLevel","unlockLevel");
    }

    private void Start()
    {
        UnlockLevel(allLevel[0]);
    }

    public void OnNotify(string notify,object args){
        switch(notify){
            case "loadLevel":
                if(args is Level)
                    LoadLevel(args as Level);
                else if(args is int){
                    int id = (int)args;
                    if(!allLevel.Exists((obj) => obj.id == id))
                        return;
                    LoadLevel(allLevel.Find((obj) => obj.id == id));
                }
                break;
            case "passLevel":
                PassLevel();
                break;
            case "onLoadUnlockLevelId":
                List<int> unlockLevelId = args as List<int>;
                foreach(var levelId in unlockLevelId){
                    UnlockLevel(allLevel.Find((level) => level.id == levelId));
                }
                break;
            case "unlockAllLevel":
                foreach(var level in allLevel){
                    UnlockLevel(level);
                }
                break;
        }
    }

    public object OnGetValue(string valueType){
        switch(valueType){
            case "nowLevel":
                return nowLevel;
            case "allLevel":
                return allLevel;
            case "unlockLevel":
                return unlockLevel;
            default :
                return null;
        }
    }

    void LoadLevel(Level level){
        ClearLevel();
        Mediator.SendMassage("clearView");
        string prefabName = level.prefab;
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Level/" + prefabName);
        GameObject levelObj = Instantiate(prefab);
        levelObj.transform.SetParent(transform);
        levelObj.transform.localScale = Vector3.one;
        levelObj.transform.localPosition = Vector3.zero;
        nowLevel = level;
        Mediator.SendMassage("openView", "mainView");
        Mediator.SendMassage("openView", "inventory");
        Mediator.SendMassage("showTopBanner");
        if (level.id != 1)
            Mediator.SendMassage("showIntersAd");
        Mediator.SendMassage("inGame");
        Mediator.SendMassage("onLoadedLevel", level);
    }

    void ClearLevel(){
        foreach(Transform child in transform){
            Destroy(child.gameObject);
        }
    }

    void PassLevel(){
        if(allLevel.Exists((level)=>level.id == nowLevel.id+1)){
            UnlockLevel(allLevel.Find((level) => level.id == nowLevel.id + 1));
        }
    }

    void UnlockLevel(Level level){
        if (unlockLevel.Contains(level))
            return;
        unlockLevel.Add(level);
        Mediator.SendMassage("onUnlockLevelUpdate", unlockLevel);
    }
}
