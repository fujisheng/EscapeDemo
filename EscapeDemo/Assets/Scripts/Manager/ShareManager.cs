using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cn.sharesdk.unity3d;
using Tools.Json;

public class ShareManager:IListener {

    static ShareManager instance;

    ShareSDK shareSdk = new ShareSDK(); 
    ShareContent content = new ShareContent();
    string shareText;
    string shareTitle;
    string shareImageName;

    public static ShareManager GetInstance(){
        if (instance == null)
            instance = new ShareManager();
        return instance;
    }

    public void Init()
    {
#if UNITY_IOS
        GameSetting setting = JsonFile.ReadFromFile<GameSetting>("Text/", "gameSetting");
        shareText = setting.shareText;
        shareTitle = setting.shareTitle;
        shareImageName = setting.shareImageName;
        SetShareContent();
        shareSdk.shareHandler += ShareResultHandler;
        Mediator.AddListener(this, "share");
#endif
    }

    public void OnNotify(string notify,object args){
        switch(notify){
            case "share":
                Share();
                break;
        }
    }

    void SetShareContent(){
        content.SetText(shareText);
        content.SetImageUrl(Mediator.GetValue("shareImageUrl").ToString() + shareImageName + ".jpg");
        content.SetTitle(shareTitle);
        content.SetUrl(Mediator.GetValue("shareUrl").ToString());
        content.SetShareType(ContentType.Image);
    }

    void ShareResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
        if (state == ResponseState.Success)
        {
            
        }
    }

    void Share(){
        shareSdk.ShowPlatformList(null, content, 100, 100);
    }
}
