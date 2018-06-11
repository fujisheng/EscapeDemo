using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveAdView : BaseView,IPopUps
{

    Button removeAdButton;
    Button restoreButton;
    Button closeButton;

    private void Awake()
    {
        removeAdButton = transform.Find("removeAdButton").GetComponent<Button>();
        restoreButton = transform.Find("restoreButton").GetComponent<Button>();
        closeButton = transform.Find("closeButton").GetComponent<Button>();

        removeAdButton.onClick.AddListener(OnRemoveAdButtonClick);
        restoreButton.onClick.AddListener(OnRestoreButtonClick);
        closeButton.onClick.AddListener(Close);
    }

    void OnRemoveAdButtonClick(){
        Mediator.SendMassage("buyProduct", (Mediator.GetValue("productList") as List<Product>)[1].id);
    }

    void OnRestoreButtonClick(){
        Mediator.SendMassage("restore");
    }
}
