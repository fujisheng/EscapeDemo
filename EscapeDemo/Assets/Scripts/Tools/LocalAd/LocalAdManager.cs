using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Json;
using LocalAd;

public class LocalAdManager:IValueSender{

    static LocalAdManager instance;

    List<LocalBannerData> dataList = new List<LocalBannerData>();

    public static LocalAdManager GetInstance(){
        if (instance == null)
            instance = new LocalAdManager();
        return instance;
    }

    public void Init(){
        dataList = JsonFile.ReadFromFile<JsonList<LocalBannerData>>("Text/", "localBannerInfo").ToList();
        Mediator.AddValue(this,"bannerData","cubeBannerData");
    }

    public object OnGetValue(string valueType){
        switch(valueType){
            case "bannerData":
                return GetBannerData(LocalBannerType.Banner);
            case "cubeBannerData":
                return GetBannerData(LocalBannerType.CubeBanner);
            default:
                return null;    
        }
    }

    LocalBannerData GetBannerData(LocalBannerType type){
        List<LocalBannerData> data = new List<LocalBannerData>();
        data = dataList.FindAll((obj) => obj.type == type);
        int index = Random.Range(0, data.Count);
        return data[index];
    }
}
