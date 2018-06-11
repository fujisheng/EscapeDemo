using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsView : BaseView,IPopUps {

    Button closeButton;
    Transform grid;

    private void Awake()
    {
        grid = transform.Find("Scroll View/Grid");
        closeButton = transform.Find("closeButton").GetComponent<Button>();

        closeButton.onClick.AddListener(Close);
    }

    public override void OnOpened()
    {
        List<Tips> ownTips = Mediator.GetValue("ownTips") as List<Tips>;
        Level nowLevel = Mediator.GetValue("nowLevel") as Level;

		Debug.Log (ownTips.Count);

        foreach(Transform child in grid.transform){
            Destroy(child.gameObject);
        }

        foreach(var tips in ownTips){
            if(tips.levelId==nowLevel.id){
                GameObject tipsObj = Resources.Load<GameObject>("Prefabs/Other/tipsItem");
                GameObject obj = Instantiate(tipsObj);
                obj.transform.SetParent(grid);
                obj.transform.localScale = Vector3.one;
                obj.GetComponent<TipsItem>().SetTips(tips);
            }
        }
    }
}
