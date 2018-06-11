using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LocalAd;

public class LocalAdView : BaseView, IListener, IBanner
{

    Image bannerImage;
    Button bannerButton;

    RectTransform rectTransform;
    RectTransform bannerRectTransform;

    LocalBannerData data;

    private void Awake()
    {
        bannerImage = transform.Find("bannerImage").GetComponent<Image>();
        bannerButton = bannerImage.GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();
        bannerRectTransform = bannerImage.GetComponent<RectTransform>();

        bannerButton.onClick.AddListener(OnBannerClick);

        Mediator.AddListener(this, "showLocalBanner", "hideLocalBanner","updateBannerData");
    }

    private void OnDestroy()
    {
        Mediator.RemoveListener(this);
    }

    public void OnNotify(string notify, object args)
    {

        switch (notify)
        {
            case "showLocalBanner":
                Args arg = args as Args;
                LocalBannerData data = arg.args[0] as LocalBannerData;
                this.data = data;
                string position = arg.args[1].ToString();
                Debug.Log(data);
                bannerImage.sprite = Resources.Load<Sprite>("Image/Banner/" + data.sprite);
                bannerImage.gameObject.SetActive(true);
                bannerImage.SetNativeSize();
                SetBannerPosition(position);
                break;
            case "hideLocalBanner":
                bannerImage.gameObject.SetActive(false);
                break;
            case "updateBannerData":
                if((args as LocalBannerData).type==this.data.type)
                    bannerImage.sprite = Resources.Load<Sprite>("Image/Banner/" + (args as LocalBannerData).sprite);
                break;
        }
    }

    void SetBannerPosition(string positon)
    {
        float y = 0f;
        switch (positon)
        {
            case "Top":
                y = rectTransform.sizeDelta.y / 2f - bannerRectTransform.sizeDelta.y / 2f;
                break;
            case "Center":
                y = 0f;
                break;
            case "Bottom":
                y = -(rectTransform.sizeDelta.y / 2f - bannerRectTransform.sizeDelta.y / 2f);
                break;
        }
        bannerImage.transform.localPosition = new Vector3(0f, y, 0f);
    }

    void OnBannerClick()
    {
        if (data == null)
            return;
#if UNITY_IOS
        Application.OpenURL(data.appStoreUrl);
#else
        Application.OpenURL(data.googlePlayUrl);
#endif
    }
}
