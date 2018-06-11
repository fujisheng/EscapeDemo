using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.NetworkInformation;
using MySql.Data.MySqlClient;
using System;
using Tools.MySql;

public class FeedbackManager : IListener, IValueSender
{

    static FeedbackManager instance;

    string physicalAddress;
    MySqlManager manager;
    string reply = string.Empty;

    public static FeedbackManager GetInstance()
    {
        if (instance == null)
            instance = new FeedbackManager();
        return instance;
    }

    public void Init()
    {
        GetPhysicalAddress();
        manager = new MySqlManager("23.105.221.177", "feedback", "feedback", "1344710445");
        manager.onSqlConnected += OnSqlConnected;
        manager.OpenSqlConnection();
        Mediator.AddListener(this, "feedback");
        Mediator.AddValue(this, "reply");
    }

    public void OnNotify(string notify, object args)
    {
        switch (notify)
        {
            case "feedback":
                Feedback(args.ToString());
                break;
        }
    }

    public object OnGetValue(string valueType)
    {
        switch (valueType)
        {
            case "reply":
                return reply;
            default:
                return null;
        }
    }

    void OnSqlConnected()
    {
        GetReply();
    }

    void GetPhysicalAddress()
    {
#if UNITY_EDITOR
        physicalAddress = "test";
#else
        physicalAddress=DeviceID.Get();
#endif
    }

    void Feedback(string text){
        Debug.Log("feedback");
        MySqlDataReader reader;
        try{
            manager.DoCommand("SELECT * FROM feedback WHERE physicalAddress=" + "'" + physicalAddress + "'", out reader);
            if (reader.HasRows)
            {
                reader.Close();
                MySqlDataReader feedbackReader;
                Debug.Log("UPDATE feedback SET feedbackText=" + "'" + text + "'" + " WHERE physicalAddress=" + "'" + physicalAddress + "'");
                manager.DoCommand("UPDATE feedback SET feedbackText=" + "'" + text + "'" + " WHERE physicalAddress=" + "'" + physicalAddress + "'", out feedbackReader);
                feedbackReader.Close();
                Mediator.SendMassage("feedbackSucceed");
            }
            else
            {
                reader.Close();
                MySqlDataReader feedbackReader;
                Debug.Log("INSERT INTO feedback(physicalAddress, feedbackText) VALUES (" + "'" + physicalAddress + "'" + "," + "'" + text + "'" + "," + "''" + "," + ")");
                manager.DoCommand("INSERT INTO feedback(physicalAddress, feedbackText) VALUES (" + "'" + physicalAddress + "'" + "," + "'" + text + "'" + ")", out feedbackReader);
                feedbackReader.Close();
                Mediator.SendMassage("feedbackSucceed");
            }
        }catch (Exception e){
            Mediator.SendMassage("feedbackFailed",e.ToString());
        }
    }

    void GetReply(){
        Debug.Log("getReply");
        MySqlDataReader reader;
        manager.DoCommand("SELECT reply FROM feedback WHERE physicalAddress="+"'"+physicalAddress+"'",out reader);
        if(reader.HasRows){
            while(reader.Read()){
                Debug.Log("reply:" + reader[0]);
                if (reader[0] != null)
                    reply = reader[0].ToString();
            }
        }
        reader.Close();
    }
}
