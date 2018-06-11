using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Tools.Json;
using Tools.Unity;
using MySql.Data.MySqlClient;

public class UrlManager : IListener, IValueSender
{

    static UrlManager instance;

    string iosMoreGameUrl = "https://itunes.apple.com/cn/developer/jisheng-fu/id";
    string androidMoreGameUrl = "https://play.google.com/store/apps/developer?id=";
    string iosLikeUrlL = "itms-apps://itunes.apple.com/app/itunes-u/id";
    string iosLikeUrlR = "?action=write-review";
    string iosShareUrl = "itms-apps://itunes.apple.com/app/itunes-u/id";
    string androidLikeUrl = "https://play.google.com/store/apps/details?id=";
    string shareImageUrl = "http://www.yiyefanhua.site/gameIcon/";

    string gameId;
    string developerId;
    string appId;

    MySqlConnection mySqlConnection;

    bool showInters = true;

    public static UrlManager GetInstance()
    {
        if (instance == null)
            instance = new UrlManager();
        return instance;
    }

    public void Init()
    {
        GameSetting setting = JsonFile.ReadFromFile<GameSetting>("Text/", "gameSetting");
        gameId = setting.gameId;
#if UNITY_IOS
        developerId = setting.iosDeveloperId;
        appId = setting.iosAppId;
#elif UNITY_ANDROID
        developerId = setting.androidDeveloperId;
        appId = setting.androidAppId;
#endif

        MonoManager.mono.StartCoroutine(ConnectionMySql());

        Mediator.AddListener(this, "openLikeUrl", "openMoreGameUrl","startOpenLikeUrl");
        Mediator.AddValue(this, "showInters","shareUrl","shareImageUrl");
    }

    public void OnNotify(string notify, object args)
    {
        switch (notify)
        {
            case "openLikeUrl":
#if UNITY_IOS
                Application.OpenURL(iosLikeUrlL + appId + iosLikeUrlR);
                Debug.Log(iosLikeUrlL + appId + iosLikeUrlR);
#elif UNITY_ANDROID
                Application.OpenURL(androidLikeUrl + appId);
#endif
                Mediator.SendMassage("onReviewed");
                break;
            case "openMoreGameUrl":
#if UNITY_IOS
                Application.OpenURL(iosMoreGameUrl + developerId);
#elif UNITY_ANDROID
                Application.OpenURL(androidMoreGameUrl + developerId);
#endif
                break;
            case "startOpenLikeUrl":
#if UNITY_IOS
                Application.OpenURL(iosLikeUrlL + appId + iosLikeUrlR);
                Debug.Log(iosLikeUrlL + appId + iosLikeUrlR);
#elif UNITY_ANDROID
                Application.OpenURL(androidLikeUrl + appId);
#endif
                break;
        }
    }

    public object OnGetValue(string valueType)
    {
        switch (valueType)
        {
            case "showInters":
#if UNITY_IOS
                return showInters;
#else
                return true;
#endif
            case "shareUrl":
                return iosShareUrl + appId;
            case "shareImageUrl":
                return shareImageUrl;
            default :
                return null;
        }
    }

    IEnumerator ConnectionMySql(){

        string connectionString = "Server = 23.105.221.177;Database = games;User ID = games;Password = 1344710445;CharSet = utf8";
        try
        {
            mySqlConnection = new MySqlConnection(connectionString);
            mySqlConnection.Open();
            Debug.Log("成功连接到数据库");
            MySqlCommand mySqlCommand = new MySqlCommand("SELECT showinters,moregame FROM games WHERE gameid=" + gameId, mySqlConnection);
            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            while (reader.Read())
            {
                Debug.Log("RequestShowInters   " + reader[0]);
                Debug.Log("RequestMoreGame     " + reader[1]);
                string showIntersStr = reader[0].ToString();
                if (showIntersStr == "0")
                    showInters = false;
                else
                    showInters = true;
                if (!string.IsNullOrEmpty(reader[1].ToString()))
                {
                    developerId = reader[1].ToString();
                }
            }
            reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log("数据库连接失败:"+e.Message.ToString());
        }
        yield return null;
    }
}
