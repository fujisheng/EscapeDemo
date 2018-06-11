using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InventoryView : BaseView ,IListener{

    enum SettingType{
        Show,
        Hide
    }

    List<InventoryBox> boxList = new List<InventoryBox>();
    GameObject boxPrefab;
    Transform grid;
    Button settingButton;
    Button helpButton;
    GameObject helpButtonLight;
    Transform settingTrans;
    Button homeButton;
    Button soundButton;
    Button likeButton;
    SettingType settingType = SettingType.Hide;

    public class InventoryProp
    {
        public Props props;
        public int number;

        public InventoryProp(Props _props, int _number)
        {
            this.props = _props;
            this.number = _number;
        }
    }

    void Awake()
    {
        grid= transform.Find("ScrollView/grid");
        boxPrefab = Resources.Load<GameObject>( "Prefabs/View/inventoryBox");

        settingButton = transform.Find("settingButton").GetComponent<Button>();
        helpButton = transform.Find("helpButton").GetComponent<Button>();
        helpButtonLight = transform.Find("helpButton/light").gameObject;
        settingTrans = transform.Find("settingView");
        homeButton = transform.Find("settingView/homeButton").GetComponent<Button>();
        soundButton = transform.Find("settingView/soundButton").GetComponent<Button>();
        likeButton = transform.Find("settingView/likeButton").GetComponent<Button>();

        settingButton.onClick.AddListener(OnSettingButtonClick);
        helpButton.onClick.AddListener(OnHelpButtonClick);
        soundButton.onClick.AddListener(OnSoundButtonClick);
        homeButton.onClick.AddListener(OnHomeButtonClick);
        likeButton.onClick.AddListener(OnLikeButtonClick);

        ShowSoundButton();

        Mediator.AddListener(this, "updateOwnProps","openPart","closePart");

        UpdateHelpButton();
    }

    public void OnNotify(string notify,object args){
        switch(notify){
            case "updateOwnProps":
                Args arg = args as Args;
                UpdateShow(arg.args[0] as List<Props>, arg.args[1] as Props);
                break;
            case "openPart":
                UpdateHelpButton();
                break;
            case "closePart":
                UpdateHelpButton();
                break;
        }
    }

    void Start()
    {
        AddBox(5);
    }

    void AddBox(int number)
    {
        if (grid == null)
        {
            Debug.Log("dont have inventory box prefab");
            return;
        }

        for (int i = 0; i < number; i++)
        {
            GameObject box = Instantiate(boxPrefab);
            box.transform.SetParent(grid);
            box.transform.localScale = Vector3.one;
            box.transform.localPosition = Vector3.zero;
            boxList.Add(box.GetComponent<InventoryBox>());
        }
    }

    void UpdateHelpButton(){
        Tips nowTips = Mediator.GetValue("nowTips") as Tips;
        if (nowTips == null)
        {
            helpButtonLight.SetActive(false);
        }
        else
            helpButtonLight.SetActive(true);
    }

    void UpdateShow(List<Props> ownPorpList, Props activeProp)
    {
        List<InventoryProp> _inventoryPropList = GetInventoryPorpList(ownPorpList);
        if (_inventoryPropList.Count > boxList.Count)
            AddBox(_inventoryPropList.Count - boxList.Count);
        for (int i = 0; i < boxList.Count; i++)
        {
            boxList[i].propList.Clear();
            if (i < _inventoryPropList.Count)
            {
                boxList[i].propList.AddRange(ownPorpList.FindAll((obj) => obj.id == _inventoryPropList[i].props.id));
            }

            boxList[i].UpdateShow(activeProp);
        }
    }

    List<InventoryProp> GetInventoryPorpList(List<Props> propList)
    {
        List<InventoryProp> inventoryPropList = new List<InventoryProp>();
        foreach (var prop in propList)
        {
            int index = InventoryPropListContans(inventoryPropList, prop);
            if (index >= 0)
                inventoryPropList[index].number++;
            else
                inventoryPropList.Add(new InventoryProp(prop, 1));
        }
        return inventoryPropList;
    }
    int InventoryPropListContans(List<InventoryProp> inventoryPropList, Props prop)
    {//判断在inventoryPropList中是否已经包含这个Prop
        for (int i = 0; i < inventoryPropList.Count; i++)
        {
            if (inventoryPropList[i].props.id == prop.id && inventoryPropList[i].props.usageCount == prop.usageCount){
                return i;//代表拥有了这个prop并且返回在inventoryPorpList中的index
            }
        }
        return -1;//代表在inventoryPropList中没有这个Prop
    }

    void ShowSoundButton()
    {
        if (Mediator.GetValue("soundOn").ToString() == "True")
        {
            soundButton.transform.Find("on").gameObject.SetActive(true);
        }
        else
        {
            Mediator.SendMassage("soundOff");
            soundButton.transform.Find("on").gameObject.SetActive(false);
        }
    }

    void OnSoundButtonClick(){
        if (Mediator.GetValue("soundOn").ToString() == "True")
        {
            soundButton.transform.Find("on").gameObject.SetActive(false);
            Mediator.SendMassage("soundOff");
        }
        else
        {
            soundButton.transform.Find("on").gameObject.SetActive(true);
            Mediator.SendMassage("soundOn");
        }
    }

    void OnSettingButtonClick(){
        switch(settingType){
            case SettingType.Hide:
                settingTrans.DOScale(Vector3.one, 0.2f);
                settingButton.transform.DOLocalRotate(new Vector3(0f,0f,90f),0.2f);
                settingType = SettingType.Show;
                break;
            case SettingType.Show:
                settingTrans.DOScale(Vector3.zero, 0.1f);
                settingButton.transform.DOLocalRotate(new Vector3(0f, 0f, -90f), 0.2f);
                settingType = SettingType.Hide;
                break;
        }
    }

    void OnHelpButtonClick(){
		if (Mediator.GetValue("nowTips") == null)//代表不存在这个提示
        {
            Level level = Mediator.GetValue("nowLevel")as Level;
            if(!(Mediator.GetValue("ownTips")as List<Tips>).Exists((obj)=>obj.levelId==level.id))//没有获得这一关的任何提示
            {
                PopUpsManager.ShowPopUps(LanguageManager.GetInstance().GetString("tips"),
                                         LanguageManager.GetInstance().GetString("tipsBuyCoin"),
                                         ()=>{PopUpsManager.HidePopUps();Mediator.SendMassage("openView", "storeView");},
                                         ()=>{PopUpsManager.HidePopUps();},
                                         LanguageManager.GetInstance().GetString("getCoin"),
                                         LanguageManager.GetInstance().GetString("close")
                                        );
            }
            else
                Mediator.SendMassage("openView", "tipsView");
        }
        else if (Mediator.GetValue("ownNowTips").ToString() == "True")
			Mediator.SendMassage("openView", "tipsView");
		else
			Mediator.SendMassage("openView", "buyTipsView");
    }

    void OnHomeButtonClick(){
        Mediator.SendMassage("openView", "levelSelectPannal");
    }

    void OnLikeButtonClick(){
        Mediator.SendMassage("startOpenLikeUrl");
    }
}
