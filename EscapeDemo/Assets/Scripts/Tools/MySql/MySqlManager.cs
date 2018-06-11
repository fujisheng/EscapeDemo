using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using Tools.Unity;

namespace Tools.MySql{
    
    public class MySqlManager
    {
        string connectionString;
        MySqlConnection mySqlConnection;
        public Action onSqlConnected;
        public Action<string> onSqlFailed;

        public MySqlManager(string server, string database,string userId,string password){
            connectionString = "Server=" + server + ";Database=" + database + ";User ID=" + userId + ";Password=" + password + ";CharSet=utf8";
        }

        public void OpenSqlConnection(){
            CoroutineManager.StartCoroutine(_OpenSqlConnection());
        }

        IEnumerator _OpenSqlConnection(){
            try{
                mySqlConnection = new MySqlConnection(connectionString);
                mySqlConnection.Open();
                Debug.Log("成功连接到服务器");
                if (onSqlConnected != null)
                    onSqlConnected.Invoke();
            }catch(Exception e){
                Debug.Log("连接数据库失败"+e.ToString());
                if (onSqlFailed != null)
                    onSqlFailed.Invoke(e.ToString());
            }
            yield return null;
        }

        public void CloseSqlConnection(){
            mySqlConnection.Close();
        }

        public void DoCommand(string sqlCommand,out MySqlDataReader reader){
            MySqlCommand mySqlCommand = new MySqlCommand(sqlCommand, mySqlConnection);
            reader = mySqlCommand.ExecuteReader();
        }
    }
}