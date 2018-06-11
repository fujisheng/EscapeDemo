using UnityEngine;
using System.Collections;
using UnityEngine.Purchasing;
using System.Collections.Generic;
using Tools;

public class IAPManager : IStoreListener, IListener, IValueSender
{

    private static IAPManager instance;

    private IStoreController controller;
    private IExtensionProvider extensions;

    class ProcessInfo
    {
        public string productID = string.Empty;
        public Timer timer;

        public ProcessInfo(string _productID, Timer _timer)
        {
            productID = _productID;
            timer = _timer;
        }
    }
    private List<ProcessInfo> processInfoList = new List<ProcessInfo>();

    public static IAPManager GetInstance()
    {
        if (instance == null)
            instance = new IAPManager();
        return instance;
    }
    public void Init()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        foreach (var product in Mediator.GetValue("productList") as List<Product>)
        {
            if (product.type == ProductType.Coin)
                builder.AddProduct(product.id, UnityEngine.Purchasing.ProductType.Consumable);
            else
                builder.AddProduct(product.id, UnityEngine.Purchasing.ProductType.NonConsumable);
        }
        UnityPurchasing.Initialize(this, builder);
        Mediator.AddListener(this, "buyProduct", "restore");
        Mediator.AddValue(this, "coin1Price", "coin2Price");
    }

    public void OnNotify(string notify, object args)
    {
        switch (notify)
        {
            case "buyProduct":
                Debug.Log(args.ToString());
                BuyProduct(args.ToString());
                break;
            case "restore":
                RestoreTransaction();
                break;
        }
    }

    public object OnGetValue(string valueType)
    {
        switch (valueType)
        {
            case "coin1Price":
                return GetProductPrice((Mediator.GetValue("productList") as List<Product>)[0].id);
            case "coin2Price":
                return GetProductPrice((Mediator.GetValue("productList") as List<Product>)[1].id);
            default:
                return null;
        }
    }

    bool IsInternetConnection()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
            return false;
        else
            return true;
    }

    ProcessInfo GetProcessInfo(string productID)
    {
        foreach (var processInfo in processInfoList)
        {
            if (processInfo.productID == productID)
                return processInfo;
        }
        return null;
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        this.controller = controller;
        this.extensions = extensions;
    }

    void BuyProduct(string productID)
    {
        Debug.Log("购买" + productID);
        if (IsInternetConnection() == false)
        {
			PopUpsManager.ShowPopUps(LanguageManager.GetInstance().GetString("connectionError"),
				                     LanguageManager.GetInstance().GetString("connectionTryAgain"),
                                     () => PopUpsManager.HidePopUps(),
				                     LanguageManager.GetInstance().GetString("close"));
            return;
        }
        Mediator.SendMassage("openView", "preloader");
        Debug.Log("<<<<<ShowPreloader>>>>>");
        ProcessInfo processInfo = new ProcessInfo(productID, new Timer(60f, () =>
        {
            if (GetProcessInfo(productID) != null)
            {
					PopUpsManager.ShowPopUps(LanguageManager.GetInstance().GetString("purchaseFailed"),
						                     LanguageManager.GetInstance().GetString("purchaseTryAgain"),
                                         delegate
                                         {
                                             PopUpsManager.HidePopUps();
                                             Mediator.SendMassage("closeView", "preloader");
                                             processInfoList.Remove(GetProcessInfo(productID));
                                         },
						                 LanguageManager.GetInstance().GetString("close"));
            }
        }));
        processInfo.timer.Start();
        processInfoList.Add(processInfo);
        controller.InitiatePurchase(productID);
    }

    public void RestoreTransaction()
    {
#if UNITY_ANDROID
        return;
#endif
        if (IsInternetConnection() == false)
        {
			PopUpsManager.ShowPopUps(LanguageManager.GetInstance().GetString("connectionError"),
				LanguageManager.GetInstance().GetString("connectionTryAgain"),
				() => PopUpsManager.HidePopUps(),
				LanguageManager.GetInstance().GetString("close"));
            return;
        }
        Mediator.SendMassage("openView", "preloader");
        Debug.Log("<<<<<ShowPreloader>>>>>");
        ProcessInfo processInfo = new ProcessInfo(string.Empty, new Timer(60f, () => {
            if (GetProcessInfo(string.Empty) != null)
            {
				PopUpsManager.ShowPopUps(LanguageManager.GetInstance().GetString("restoreFailed"),
					LanguageManager.GetInstance().GetString("restoreTryAgain"), 
                                         ()=>{
                                                PopUpsManager.HidePopUps();
                                                Mediator.SendMassage("closeView", "preloader");
                                                processInfoList.Remove(GetProcessInfo(string.Empty));
                                             }, 
					LanguageManager.GetInstance().GetString("close")
                                        );

            }
        }));
        processInfo.timer.Start();
        processInfoList.Add(processInfo);
        extensions.GetExtension<IAppleExtensions>().RestoreTransactions(result => {
            if (result)
            {
                Debug.Log("恢复购买成功");
                if (GetProcessInfo(string.Empty) != null)
                {
					PopUpsManager.ShowPopUps("", 
						LanguageManager.GetInstance().GetString("restoreSuccessful"), 
                                             delegate {
                                                        PopUpsManager.HidePopUps();
                                                        Mediator.SendMassage("closeView", "preloader");
                                                      },
						LanguageManager.GetInstance().GetString("close"));
                    processInfoList.Remove(GetProcessInfo(string.Empty));
                }
            }
            else
            {
                Debug.Log("恢复购买失败");
                if (GetProcessInfo(string.Empty) != null)
                {
					PopUpsManager.ShowPopUps(LanguageManager.GetInstance().GetString("restoreFailed"), 
						LanguageManager.GetInstance().GetString("restoreTryAgain"),
                                             delegate {
                                                        PopUpsManager.HidePopUps();
                                                        Mediator.SendMassage("closeView", "preloader");
                                                      },
						LanguageManager.GetInstance().GetString("close"));
                    processInfoList.Remove(GetProcessInfo(string.Empty));
                }

            }
        });
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
//        PopUpsManager.ShowPopUps("Initialize",
//                                 "Initialized IAP Failed!",
//                                 ()=>PopUpsManager.HidePopUps(),
//                                 "Close");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs a)
    {
        Mediator.SendMassage("closeView", "preloader");
        Debug.Log("<<<<<HidePreloader>>>>>");
        Mediator.SendMassage("onProcessPurchased", a.purchasedProduct.definition.id);
        processInfoList.Remove(GetProcessInfo(a.purchasedProduct.definition.id));
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(UnityEngine.Purchasing.Product i, PurchaseFailureReason p)
    {
        if (p == PurchaseFailureReason.UserCancelled)
        {
            Mediator.SendMassage("closeView", "preloader");
            processInfoList.Remove(GetProcessInfo(i.definition.id));
            return;
        }
        if (GetProcessInfo(i.definition.id) == null)
            return;
		PopUpsManager.ShowPopUps(LanguageManager.GetInstance().GetString("purchaseFailed"), 
			LanguageManager.GetInstance().GetString("purchaseTryAgain"), 
                                 delegate {
                                            PopUpsManager.HidePopUps();
                                            Mediator.SendMassage("closeView", "preloader");
                                          },
			LanguageManager.GetInstance().GetString("close"));
        processInfoList.Remove(GetProcessInfo(i.definition.id));
    }

    string GetProductPrice(string productID)
    {
        foreach (var product in controller.products.all)
        {
            if (string.Equals(product.definition.id, productID, System.StringComparison.Ordinal))
            {
                return product.metadata.localizedPriceString;
            }
        }
        return string.Empty;
    }
}