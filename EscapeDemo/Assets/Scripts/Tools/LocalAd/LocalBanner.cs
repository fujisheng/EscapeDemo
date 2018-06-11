using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

namespace LocalAd{
    public class LocalBanner
    {
        LocalBannerType type;
        LocalBannerPosition position;
        LocalBannerData data;

        public void Show() {
            Mediator.SendMassage("openView","localAdView");
            Mediator.SendMassage("showLocalBanner",new Args(data,position));
        }

        public void Hide() { 
            Mediator.SendMassage("hideLocalBanner",data);
        }

        public LocalBanner(LocalBannerType type, LocalBannerPosition position)
        {
            this.type = type;
            this.position = position;
            GetData(type);
            Timer timer = new Timer(5f,true, () => { 
                GetData(type);
                Mediator.SendMassage("updateBannerData",data); 
            });
            timer.Start();
        }

        void GetData(LocalBannerType type){
			LocalBannerData data = new LocalBannerData();
			switch (type)
			{
				case LocalBannerType.Banner:
					data = Mediator.GetValue("bannerData") as LocalBannerData;
					break;
				case LocalBannerType.CubeBanner:
					data = Mediator.GetValue("cubeBannerData") as LocalBannerData;
					break;
			}
                this.data = data;
        }
	}
}


