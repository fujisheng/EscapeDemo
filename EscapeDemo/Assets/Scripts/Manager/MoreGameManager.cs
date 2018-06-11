using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using Tools.Unity;
using System;

public class MoreGameManager:IValueSender{

    public class MoreGameInfo{
        public int appId;
        public string name;
        public Sprite sprite;
        public string url;
    }

    List<MoreGameInfo> infoList = new List<MoreGameInfo>();
    MySqlConnection mySqlConnection = new MySqlConnection(); 

    static MoreGameManager instance;

    public static MoreGameManager GetInstance(){
        if (instance == null)
            instance = new MoreGameManager();
        return instance;
    }

    public void Init(){
        CoroutineManager.StartCoroutine(ConnectionMySql());
        Mediator.AddValue(this,"moreGameInfo");
    }

    public object OnGetValue(string valueType){
        switch(valueType){
            case "moreGameInfo":
                return infoList;
            default:
                return null;
        }
    }

    IEnumerator ConnectionMySql(){
        string connectionString = "Server = 23.105.221.177;Database = moreGame;User ID = moreGame;Password = 1344710445;CharSet = utf8";
        bool mySqlConnected = false;

        try{
            mySqlConnection = new MySqlConnection(connectionString);
            mySqlConnection.Open();
            Debug.Log("成功连接到数据库");
            mySqlConnected = true;
        }catch(Exception e){
            mySqlConnected = false;
            Debug.Log("连接到服务器失败"+e.ToString());
        }

        if(mySqlConnected==true){
            MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM moreGame", mySqlConnection);
            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            while (reader.Read())
            {
                MoreGameInfo moreGameInfo = new MoreGameInfo();
                moreGameInfo.appId = int.Parse(reader[0].ToString());
                moreGameInfo.name = reader[1].ToString();
                moreGameInfo.url = "itms-apps://itunes.apple.com/app/itunes-u/id" + moreGameInfo.appId;
                WWW myWWW = new WWW("http://www.yiyefanhua.site/moreGame/" + moreGameInfo.name + ".jpg");
                yield return myWWW;
                Sprite sprite = new Sprite();
                if (myWWW.isDone)
                {
                    Texture2D tex = myWWW.texture;
                    sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                    moreGameInfo.sprite = sprite;
                    infoList.Add(moreGameInfo);
                }
            }
            reader.Close();
        }
    }
}
