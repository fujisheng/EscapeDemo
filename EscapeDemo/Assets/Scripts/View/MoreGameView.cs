using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MoreGameManager;

public class MoreGameView : BaseView {

    Button closeButton;

    private void Awake()
    {
        closeButton = transform.Find("closeButton").GetComponent<Button>();
        closeButton.onClick.AddListener(()=>{
            Mediator.SendMassage("showCubeBanner");
            Close();
        });

        Init();
    }

    void Init(){
        List<MoreGameInfo> infoList = Mediator.GetValue("moreGameInfo") as List<MoreGameInfo>;

        if (infoList.Count == 0)
            return;
        GameObject infoItem = Resources.Load<GameObject>("Prefabs/View/moreGameItem");
        foreach(var info in infoList){
            GameObject obj = Instantiate(infoItem);
            obj.transform.SetParent(transform.Find("window/Scroll View/grid"));
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<Image>().sprite=info.sprite;
            obj.GetComponent<Button>().onClick.AddListener(() =>
            {
                Application.OpenURL(info.url);
                Debug.Log(info.url);
            });
        }
    }
}
