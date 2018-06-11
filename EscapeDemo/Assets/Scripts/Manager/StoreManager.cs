using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Json;
using Tools.Files;
using Tools;
using UnityEngine.UI;

public class StoreManager :IListener,IValueSender {

    static StoreManager instance;

    List<Product> productList = new List<Product>();
    List<Product> ownProductList = new List<Product>();

    Timer timer;
    Text timerText;
    bool timeOut = true;

    public static StoreManager GetInstance(){
        if (instance == null)
            instance = new StoreManager();
        return instance;
    }

    public void Init(){
        productList = JsonFile.ReadFromFile<JsonList<Product>>("Text/", "productInfo").ToList();
        timer = new Timer(60f, false, timerText, TimeOut);//看完之后必须等60秒才能看第二个

        Mediator.AddListener(this, "onProcessPurchased","startWatchAdTimer","updateWatchAdTimer","onLoadOwnProductId");
        Mediator.AddValue(this, "productList","removeAd","watchAdTimeOut");
    }

    public void OnNotify(string notify,object args){
        switch(notify){
            case "onProcessPurchased":
                OnProcessPurchased(args.ToString());
                break;
            case "startWatchAdTimer":
                timer.text = args as Text;
                StartTimer();
                break;
            case "updateWatchAdTimer":
                timer.text = args as Text;
                break;
                case "onLoadOwnProductId":
                List<string> idList = args as List<string>;
                idList.ForEach((obj)=>ownProductList.Add(productList.Find((obj2)=>obj2.id==obj)));
                break;
        }
    }

    public object OnGetValue(string valueType){
        switch(valueType){
            case "productList":
                return productList;
            case "removeAd":
                return ownProductList.Exists((obj) => obj.type == ProductType.RemoveAd);
            case "watchAdTimeOut":
                return timeOut;
            default:
                return null;
        }
    }

    void OnProcessPurchased(string id){
        Product product = productList.Find((_product) => _product.id == id);
        switch (product.type){
            case ProductType.Coin:
                Mediator.SendMassage("addCoin", product.coin);
                break;
            case ProductType.RemoveAd:
                Mediator.SendMassage("onRemoveAd");
                break;
        }
        if (ownProductList.Exists((obj) => obj.id == id))
            return;
        ownProductList.Add(product);
        Mediator.SendMassage("onOwnProductUpdate",ownProductList);
    }

    void StartTimer(){
        if (timer.text == null)
            return;
        timer.Reset();
        timer.Start();
        timeOut = false;
    }

    void TimeOut(){
        timeOut = true;
        Mediator.SendMassage("watchAdTimeOut");
    }
}
