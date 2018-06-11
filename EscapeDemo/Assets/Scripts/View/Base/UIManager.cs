using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class UIManager : MonoBehaviour,IListener {

    Dictionary<string, BaseView> loadedViewDic = new Dictionary<string, BaseView>();

    //存储打开的view的层次关系
    List<string>openedBaseViewList = new List<string>();
    List<string>openedPopUpsList = new List<string>();
    List<string>openedBannerList=new List<string> ();

    private void Awake()
    {
        Mediator.AddListener(this, "openView", "closeView", "loadView", "unloadView", "clearView");
    }

    public void OnNotify(string notify,object args){
        switch (notify){
            case "openView":
                OpenView(args.ToString());
                break;
            case "closeView":
                CloseView(args.ToString());
                break;
            case "loadView":
                LoadView(args.ToString());
                break;
            case "unloadView":
                UnloadView(args.ToString());
                break;
            case "clearView":
                ClearView();
                break;
        }
    }

    void OpenView(string viewName){
        if(!loadedViewDic.ContainsKey(viewName)){
            LoadView(viewName);
        }
        BaseView view = loadedViewDic[viewName];
        view.gameObject.SetActive(true);

        view.OnOpen().OnComplete(() =>
        {
            if (view is IPopUps)
            {
                if (openedPopUpsList.Contains(viewName))
                {
                    openedPopUpsList.Remove(viewName);
                    openedPopUpsList.Add(viewName);
                }
                else
                    openedPopUpsList.Add(viewName);
            }
            else if (view is IBanner)
            {
                if (openedBannerList.Contains(viewName))
                {
                    openedBannerList.Remove(viewName);
                    openedBannerList.Add(viewName);
                }
                else
                    openedBannerList.Add(viewName);
            }
            else
            {
                if (openedBaseViewList.Contains(viewName))
                {
                    openedBaseViewList.Remove(viewName);
                    openedBaseViewList.Add(viewName);
                }
                else
                    openedBaseViewList.Add(viewName);
            }

            view.OnOpened();
            view.onOpened.Invoke();
            UpdateShow();
        });


    }

    void CloseView(string viewName)
    {
        if (!loadedViewDic.ContainsKey(viewName))
        {
            Debug.Log("dont load this view  " + viewName);
            return;
        }
        BaseView view = loadedViewDic[viewName];
        view.OnClose().OnComplete(() => {
            view.gameObject.SetActive(false);
            view.OnClosed();
            view.onClosed.Invoke();
            if(view is IPopUps)
                openedPopUpsList.Remove(viewName);
            else if(view is IBanner)
                openedBannerList.Remove(viewName);
            else
                openedBaseViewList.Remove(viewName);

            UpdateShow();
        });
    }

    void LoadView(string viewName){
        if(loadedViewDic.ContainsKey(viewName)){
            Debug.Log(viewName + "  isLoaded");
            return;
        }
        GameObject viewPrefab = Resources.Load<GameObject>("Prefabs/View/" + viewName);
        if(viewPrefab==null){
            Debug.Log("dont have this prefab " + viewName);
            return;
        }
        GameObject obj = Instantiate(viewPrefab);
        obj.transform.SetParent(transform);
        obj.transform.localScale = Vector3.zero;
        obj.transform.localPosition = Vector3.zero;
        obj.SetActive(false);
        loadedViewDic.Add(viewName, obj.GetComponent<BaseView>());
    }

    void UnloadView(string viewName){
        if(!loadedViewDic.ContainsKey(viewName)){
            Debug.Log("dont load this view " + viewName);
            return;
        }
        if(loadedViewDic[viewName]is IListener){
            Mediator.RemoveListener(loadedViewDic[viewName] as IListener);
        }
        Destroy(loadedViewDic[viewName].gameObject);
        loadedViewDic.Remove(viewName);
        if (openedPopUpsList.Contains(viewName))
            openedPopUpsList.Remove(viewName);
        else if (openedBannerList.Contains(viewName))
            openedBannerList.Remove(viewName);
        else if (openedBaseViewList.Contains(viewName))
            openedBaseViewList.Remove(viewName);
    }

    void ClearView(){
        List<string> keys = new List<string>();
        foreach(var key in loadedViewDic.Keys){
            keys.Add(key);
        }
        foreach(var key in keys){
            UnloadView(key);
        }
    }
       
    void UpdateShow(){
        List<string> allViewList = new List<string>();
        allViewList.AddRange(openedBaseViewList);
        allViewList.AddRange(openedPopUpsList);
        allViewList.AddRange(openedBannerList);
        for (int i = 0; i < allViewList.Count; i++)
        {
            loadedViewDic[allViewList[i]].transform.SetSiblingIndex(i);
        }

        if (openedPopUpsList.Count == 0)
        {
            if (loadedViewDic.ContainsKey("mask"))
            {
                BaseView mask = loadedViewDic["mask"];
                mask.transform.localScale = Vector3.zero;
                mask.transform.SetAsLastSibling();
                mask.gameObject.SetActive(false);
            }
                
        }
        else
        {
            if (!loadedViewDic.ContainsKey("mask"))
                LoadView("mask");
			BaseView mask = loadedViewDic["mask"];
			mask.transform.localScale = Vector3.one;
			mask.transform.SetSiblingIndex(allViewList.FindIndex((obj) => obj == openedPopUpsList.Last()));
            mask.gameObject.SetActive(true);
        }
    }
}
