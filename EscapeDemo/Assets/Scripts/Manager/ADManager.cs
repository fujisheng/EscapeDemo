using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds;
using UnityEngine.Advertisements;
using System.Collections.Generic;
using Tools.Json;
using Tools;
using UnityEngine.iOS;
using System;
using LocalAd;

public class ADManager : IListener, IValueSender
{

	enum ShowingBannerType{
		General,
		Cube,
		None
	}

    private static ADManager instance;

	BannerView generalBannerView;
    BannerView cubeBannerView;
    RewardBasedVideoAd rewardBasedVideo;
    InterstitialAd interstitial;
	ShowingBannerType showingBannerType=ShowingBannerType.None;

    LocalBanner localGeneralBanner;
    LocalBanner localCubeBanner;
    bool generalBannerIsLoaded;
    bool cubeBannerIsLoaded;

    string admobGeneralBannerUnitId = string.Empty;
    string admobCubeBannerUnitId = string.Empty;
    string admobIntersUnitId = string.Empty;
    string admobRewardUnitId = string.Empty;
    string unityAdsGameId = string.Empty;
    string unityPlacementId = string.Empty;

    bool showTimerInters = false;//隔多久显示inters
    Timer intersTimer; 


    public static ADManager GetInstance()
    {
        if (instance == null)
            instance = new ADManager();
        return instance;
    }

    public void Init()
    {
        GameSetting setting = JsonFile.ReadFromFile<GameSetting>("Text/", "gameSetting");
#if UNITY_IOS
        admobGeneralBannerUnitId = setting.admobIosGeneralBannerUnitId;
        admobCubeBannerUnitId = setting.admobIosCubeBannerUnitId;
        admobIntersUnitId = setting.admobIosIntersUnitId;
        admobRewardUnitId = setting.admobIosRewardUnitId;
        unityAdsGameId = setting.unityIosAdsGameId;
        unityPlacementId = setting.unityIosPlacementId;
#endif
        InitAD();
        LocalAdManager.GetInstance().Init();
        Mediator.AddListener(this, "onRemoveAd", "showRewardAd", "showTopBanner", "hideBanner", "showIntersAd","showTimerIntersAd", "showCubeBanner","onLoadedLevel");
        Mediator.AddValue(this, "rewardAdIsReady");

        intersTimer = new Timer(120, false, () =>
        {
            showTimerInters = true;
        });//隔120s显示插屏
        intersTimer.Start();
    }

    public void OnNotify(string notify, object args)
    {
        switch (notify)
        {
    		case "onRemoveAd":
    		    generalBannerView.Hide ();
    		    cubeBannerView.Hide ();
                break;
            case "showRewardAd":
                ShowRewardsInterstitial();
                break;
            case "showIntersAd":
                ShowInterstitial();
                break;
            case "showTimerIntersAd":
                ShowTimerInterstitial();
                break;
            case "showTopBanner":
                ShowGeneralBanner();
                break;
            case "hideBanner":
                HideAllBanner();
                break;
    		case "showCubeBanner":
    		    ShowCubeBanner ();
                break;
            case "onLoadedLevel":
                intersTimer.Reset();
                showTimerInters = false;
                intersTimer.Start();
                break;
        }
    }

    public object OnGetValue(string valueType)
    {
        switch (valueType)
        {
            case "rewardAdIsReady":
                return RewardsAdIsReady();
            default:
                return null;
        }
    }

    void InitAD()
    {
        //InitAdmob
#if UNITY_IOS
        //float logicalPixelDensity = Screen.dpi / 154f;//iphone plus 的基准像素密度是154
        //Debug.Log("基准像素密度"+logicalPixelDensity);
        //AdSize cubeBannerSize = new AdSize((int)(600f * (float)Screen.width / 1136f / logicalPixelDensity), (int)(500f * (float)Screen.height / 640f / logicalPixelDensity));
        //cubeBannerView = new BannerView(admobCubeBannerUnitId, cubeBannerSize, AdPosition.Center);//固定大小的
        cubeBannerView = new BannerView(admobCubeBannerUnitId, AdSize.MediumRectangle, AdPosition.Center);//普通打小的
        generalBannerView = new BannerView(admobGeneralBannerUnitId, AdSize.Banner, AdPosition.Bottom);
#endif
        rewardBasedVideo = RewardBasedVideoAd.Instance;
        interstitial = new InterstitialAd(admobIntersUnitId);
        AdRequest request = new AdRequest.Builder()
                                         //.AddTestDevice(AdRequest.TestDeviceSimulator)
		                                 .AddTestDevice("98bdc5b5c1a9a6568f5a7fc558d13a5939a50fbb")//付继生的手机
		                                 .AddTestDevice("a3bdd163833a6f91ee55e81f186b9a43536b7bac")//杨鹏翎的手机
                                         .Build();
        cubeBannerView.OnAdLoaded += OnCubeBannerLoaded;
        generalBannerView.OnAdLoaded += OnGeneralBannerLoaded;
        rewardBasedVideo.OnAdRewarded += AdMobRewardedCallbackHandler;
        rewardBasedVideo.OnAdClosed += AdMobVideoClosedCallbackHandler;
        interstitial.OnAdClosed += AdMobIntersClosedCallbackHandler;

        cubeBannerView.LoadAd(request);
		generalBannerView.LoadAd(request);
        rewardBasedVideo.LoadAd(request, admobRewardUnitId);
        interstitial.LoadAd(request);


        Timer timer = new Timer(20, true, () =>
        {
            generalBannerView.LoadAd(request);
            cubeBannerView.LoadAd(request);
        }) ;
        timer.Start();
        generalBannerView.Hide();
        cubeBannerView.Hide();

        //InitLocalBanner
        localGeneralBanner = new LocalBanner(LocalBannerType.Banner, LocalBannerPosition.Bottom);
        localCubeBanner = new LocalBanner(LocalBannerType.CubeBanner, LocalBannerPosition.Center); 

        //InitUnityAds
        Advertisement.Initialize(unityAdsGameId, false);
    }

    void HideAllBanner(){
        localGeneralBanner.Hide();
        localCubeBanner.Hide();
		generalBannerView.Hide ();
        cubeBannerView.Hide();
		showingBannerType = ShowingBannerType.None;
    }

    void ShowGeneralBanner()
    {
		if (IsRemoveAd () == true)
			return;
		HideAllBanner();
        if(!generalBannerIsLoaded)
            localGeneralBanner.Show();
		generalBannerView.Show();
        showingBannerType = ShowingBannerType.General;
	}

    void ShowCubeBanner()
    {
		if (IsRemoveAd () == true)
			return;
		HideAllBanner();
        if(!cubeBannerIsLoaded)
            localCubeBanner.Show();
        cubeBannerView.Show();
		showingBannerType = ShowingBannerType.Cube;
    }

    void ShowInterstitial()
    {
        if (IsRemoveAd() == true)
            return;
        if (Mediator.GetValue("showInters").ToString() == "False")
            return;

        if (interstitial.IsLoaded()){
            interstitial.Show();
        }
        else if (Advertisement.IsReady()){
            Advertisement.Show();
        }
    }

    void ShowTimerInterstitial(){
        if (showTimerInters == false)
            return;
        if (IsRemoveAd() == true)
            return;
        if (Mediator.GetValue("showInters").ToString() == "False")
            return;

        if (interstitial.IsLoaded())
        {
            interstitial.Show();
            showTimerInters = false;
            intersTimer.Reset();
            intersTimer.Start();
        }
        else if (Advertisement.IsReady()){
            Advertisement.Show();
            showTimerInters = false;
            intersTimer.Reset();
            intersTimer.Start();
        }
    }

    void ShowRewardsInterstitial()
    {
        if (rewardBasedVideo.IsLoaded())
        {
            rewardBasedVideo.Show();
        }
        else if (Advertisement.IsReady(unityPlacementId))
        {
            ShowOptions options = new ShowOptions();
            options.resultCallback = UnityAdsCallbackHandler;
            Advertisement.Show(unityPlacementId, options);

        }
    }

    bool RewardsAdIsReady()
    {
        if (rewardBasedVideo.IsLoaded())
        {
            Debug.Log("admobAdsIsReady");
            return true;
        }
        else if (Advertisement.IsReady(unityPlacementId))
        {
            Debug.Log("unityAdsIsReady");
            return true;
        }
        else
        {
            Debug.Log("adNotReady");
            return false;
        }
    }

    bool IsRemoveAd()
    {
        if (Mediator.GetValue("removeAd").ToString() == "False")
            return false;
        else
            return true;
    }

    void UnityAdsCallbackHandler(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Mediator.SendMassage("onRewardAdRewarded");
                break;
        }
    }

    public void AdMobRewardedCallbackHandler(object sender, Reward reward)
    {
        Debug.Log("admob rewarded");
        Mediator.SendMassage("onRewardAdRewarded");
    }

    void AdMobVideoClosedCallbackHandler(object sender,EventArgs args){
        AdRequest request = new AdRequest.Builder()
                                     .AddTestDevice("98bdc5b5c1a9a6568f5a7fc558d13a5939a50fbb")//付继生的手机
                                     .AddTestDevice("a3bdd163833a6f91ee55e81f186b9a43536b7bac")//杨鹏翎的手机
                                     .Build();
        rewardBasedVideo.LoadAd(request, admobRewardUnitId);
    }

    void AdMobIntersClosedCallbackHandler(object sender,EventArgs args){
        interstitial.LoadAd(new AdRequest.Builder()
                                     .AddTestDevice("98bdc5b5c1a9a6568f5a7fc558d13a5939a50fbb")//付继生的手机
                                     .AddTestDevice("a3bdd163833a6f91ee55e81f186b9a43536b7bac")//杨鹏翎的手机
                                     .Build());
    }

    void OnGeneralBannerLoaded(object sender,EventArgs args){
        generalBannerIsLoaded = true;
        if (showingBannerType == ShowingBannerType.General){
            ShowGeneralBanner();
        }
    }

    void OnCubeBannerLoaded(object sender,EventArgs args){
        cubeBannerIsLoaded = true;
        if (showingBannerType == ShowingBannerType.Cube){
            ShowCubeBanner();
        }	
    }
}