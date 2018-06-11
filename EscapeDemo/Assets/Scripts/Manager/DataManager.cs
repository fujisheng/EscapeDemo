using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Files;
using Tools.Json;

public class DataManager : IListener,IValueSender{

    static DataManager instance;
    int rewardGem;

    GameData data = new GameData();

    public static DataManager GetInstance(){  
        if(instance==null){
            instance = new DataManager();
        }
        return instance;
    }

    public void Init(){
        GameSetting setting = JsonFile.ReadFromFile<GameSetting>("Text/", "gameSetting");
        if(setting.deleteData==true){
            DeleteData();
        }
        Mediator.AddListener(this,
                             "onAddGem",
                             "addCoin",
                             "onUnlockLevelUpdate",
                             "onReviewed",
                             "showRewardAd",
                             "onOwnTipsUpdate",
                             "deleteData",
                             "onOwnProductUpdate",
                             "updateOwnProps"
                            );
        Mediator.AddValue(this, "coin","reviewed","rewardGem","rewardNumber");
    }

    public void OnNotify(string notify,object args){
        switch(notify){
            case "addCoin":
                data.coin += (int)args;
                Files.SaveFile("gameData.json", data);
                Mediator.SendMassage("onCoinUpdate", data.coin);
                break;
            case "onUnlockLevelUpdate":
                SaveUnlockLevel(args as List<Level>);
                break;
            case "onReviewed":
                data.reviewed = true;
                Files.SaveFile("gameData.json", data);
                Mediator.SendMassage("onReviewedUpdate", data.reviewed);
                break;
            case "showRewardAd":
                data.time = System.DateTime.Now.ToString();
                data.rewardNumber++;
                Files.SaveFile("gameData.json", data);
                break;
            case "onOwnTipsUpdate":
                SaveOwnTips(args as List<Tips>);
                break;
            case "onOwnProductUpdate":
                SaveOwnProduct(args as List<Product>);
                break;
            case "updateOwnProps":
                SaveOwnProps((args as Args).args[0] as List<Props>);
                break;
            case "deleteData":
                DeleteData();
                break;
        }
    }

    public object OnGetValue(string valueType){
        switch(valueType){
            case "coin":
                return data.coin;
            case "reviewed":
                return data.reviewed;
            case "rewardNumber":
                return data.rewardNumber;
            default:
                return null;
        }
    }

    List<T> LoadListFromJson<T>(string jsonName)
    {
        List<T> returnList = new List<T>();
        JsonList<T> jsonList = new JsonList<T>();
        jsonList = Files.LoadFile<JsonList<T>>(jsonName);
        if (jsonList != null)
            jsonList.list.ForEach((obj) => returnList.Add(obj));
        return returnList;
    }

    public void LoadFile(){
        //load unlock level
        Mediator.SendMassage("onLoadUnlockLevelId", LoadListFromJson<int>("unlockLevel.json"));

        //load own tips
        Mediator.SendMassage("onLoadOwnTipsId", LoadListFromJson<int>("ownTips.json"));

        //load OwnProduct
        Mediator.SendMassage("onLoadOwnProductId", LoadListFromJson<string>("ownProduct.json"));

        //load own props
        Mediator.SendMassage("onLoadOwnPropsId", LoadListFromJson<int>("ownProps.json"));

        //load game data
        data = Files.LoadFile<GameData>("gameData.json");
        if (data != null)
        {
            Mediator.SendMassage("onCoinUpdate", data.coin);
            Mediator.SendMassage("onReviewedUpdate", data.reviewed);

            if (!string.IsNullOrEmpty(data.time) && data.time.Substring(0, 10) != System.DateTime.Now.ToString().Substring(0, 10))
            {
                data.time = System.DateTime.Now.ToString();
                data.rewardNumber = 0;
                Files.SaveFile("gameData.json", data);
            }
        }
        else
            data = new GameData();
    }

    void SaveUnlockLevel(List<Level> levelList){
        JsonList<int> levelJsonList = new JsonList<int>();
        levelList.ForEach((level) => levelJsonList.list.Add(level.id));
        Files.SaveFile("unlockLevel.json", levelJsonList);
    }

    void SaveOwnTips(List<Tips> tipsList){
        JsonList<int> tipsJsonList = new JsonList<int>();
        tipsList.ForEach((tips) => tipsJsonList.list.Add(tips.id));
        Files.SaveFile("ownTips.json", tipsJsonList);
    }

    void SaveOwnProduct(List<Product> productList){
        JsonList<string> productJsonList = new JsonList<string>();
        productList.ForEach((obj)=>productJsonList.list.Add(obj.id));
        Files.SaveFile("ownProduct.json",productJsonList);
    }

    void SaveOwnProps(List<Props> propsList){
        JsonList<int> propsJosnList = new JsonList<int>();
        propsList.ForEach((obj)=>propsJosnList.list.Add(obj.id));
        Files.SaveFile("ownProps.json",propsJosnList);
    }

    void DeleteData(){
		Files.DeleteFile("unlockLevel.json");
		Files.DeleteFile("ownTips.json");
		Files.DeleteFile("gameData.json");
		Files.DeleteFile("ownProduct.json");
        Files.DeleteFile("ownProps.json");
    }
}
