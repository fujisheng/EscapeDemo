using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LocalAd{
	[System.Serializable]
	public class LocalBannerData
	{

		public int id;
        public string sprite;
        public string appStoreUrl;
        public string googlePlayUrl;
        public LocalBannerType type;
	}
}