using System.Collections.Generic;

public class Mediator
{
    static Dictionary<string, List<IListener>> notifyDic = new Dictionary<string, List<IListener>>();
    static Dictionary<string, IValueSender> valueDic = new Dictionary<string, IValueSender>();

    public static void AddListener(IListener listener, string notify)
    {
        if (notifyDic.ContainsKey(notify))
        {
            notifyDic[notify].Add(listener);
        }
        else
        {
            List<IListener> listenerList = new List<IListener>();
            listenerList.Add(listener);
            notifyDic.Add(notify, listenerList);
        }
    }

    public static void AddListener(IListener listener, params string[] notifys)
    {
        foreach (var str in notifys)
        {
            AddListener(listener, str);
        }
    }

    public static void RemoveListener(IListener listener, string notify)
    {
        if (notifyDic.ContainsKey(notify))
        {
            notifyDic[notify].Remove(listener);
        }
    }

    public static void RemoveListener(IListener listener){
        List<string> nullKeyList = new List<string>();
        foreach(KeyValuePair<string,List<IListener>> keyValue in notifyDic){
            if(keyValue.Value.Contains(listener)){
                keyValue.Value.Remove(listener);
            }
            if(keyValue.Value.Count==0){
                nullKeyList.Add(keyValue.Key);
            }
        }
        foreach(var key in nullKeyList){
            notifyDic.Remove(key);
        }
    }

    public static void RemoveListener(IListener listener, params string[] notifys)
    {
        foreach (var str in notifys)
        {
            RemoveListener(listener, str);
        }
    }

    public static void SendMassage(string notify,object args)
    {
        if (notifyDic.ContainsKey(notify))
        {
            notifyDic[notify].ForEach((listener) => listener.OnNotify(notify,args));
        }
    }

    public static void SendMassage(string notify){
        SendMassage(notify, null);
    }

    public static void AddValue(IValueSender sender, string valueType)
    {
        
        if (valueDic.ContainsKey(valueType))
        {
            valueDic.Remove(valueType);
        }
        valueDic.Add(valueType, sender);
    }

    public static void AddValue(IValueSender sender, params string[] valueTypes)
    {
        foreach (var type in valueTypes)
        {
            AddValue(sender, type);
        }
    }

    public static object GetValue(string valueType)
    {
        if (!valueDic.ContainsKey(valueType))
            return null;
        return valueDic[valueType].OnGetValue(valueType);
    }
}

