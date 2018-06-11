using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyTipsView : BaseView,IPopUps{

    Button buyButton;
    Button cancelButton;
    Text needCoinText;
	GameObject price;
	GameObject getCoin;

    private void Awake()
    {
        buyButton = transform.Find("buyButton").GetComponent<Button>();
        cancelButton = transform.Find("cancelButton").GetComponent<Button>();
        needCoinText = transform.Find("buyButton/price/Text").GetComponent<Text>();
		price = transform.Find ("buyButton/price").gameObject;
		getCoin = transform.Find ("buyButton/getCoin").gameObject;

        buyButton.onClick.AddListener(OnBuyButtonClick);
        cancelButton.onClick.AddListener(OnCancelButtonClick);
    }

    public override void OnOpened()
    {
		Tips nowTips = Mediator.GetValue ("nowTips") as Tips;
		int coin = (int)Mediator.GetValue ("coin");

		if (coin >= nowTips.price) {
			price.SetActive (true);
			getCoin.SetActive (false);
			needCoinText.text = nowTips.price.ToString ();
		} else {
			price.SetActive (false);
			getCoin.SetActive (true);
		}
    }

    void OnBuyButtonClick(){
        Tips nowTips = Mediator.GetValue("nowTips") as Tips;
        int coin = (int)Mediator.GetValue("coin");
        if(coin>=nowTips.price)
        {
            Mediator.SendMassage("addCoin", -nowTips.price);
            Mediator.SendMassage("getTips", nowTips);
            Close();
            Mediator.SendMassage("openView", "tipsView");
        }
        else
        {
            Close();
			Mediator.SendMassage("openView", "storeView");
        }
    }

    void OnCancelButtonClick(){
        Close();
    }
}
