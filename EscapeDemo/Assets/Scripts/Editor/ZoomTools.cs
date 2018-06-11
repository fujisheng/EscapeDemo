using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Tools.Unity;

public class ZoomTools : EditorWindow{

    Vector2 scrollPos = new Vector2();
    Transform showingTrans = null;
    bool autoShow = true;

    Transform levelTrans;

    bool stop = false;

    [MenuItem("MyEditor/Zoom Tools")]
    static void Init()
    {
        ZoomTools zoomTools = (ZoomTools)EditorWindow.GetWindow(typeof(ZoomTools), false, "Zoom Tools", true);
        zoomTools.minSize = new Vector2(20, 100);
        zoomTools.autoRepaintOnSceneChange = true;
        zoomTools.Show();
    }

    private void OnEnable()
    {
        levelTrans = GameObject.Find("LevelCanvas").transform;
    }

    void OnGUI()
    {
        if (levelTrans.childCount == 0)
        {
            stop = true;
            return;
        }
        else
            stop = false;
        Repaint();
        GUILayout.Space(10);
        if (GUILayout.Button(autoShow == true ? "Auto Show Is On" : "Auto Show IsOff"))
        {
            if (autoShow == true)
                autoShow = false;
            else
                autoShow = true;
        }
        if (Selection.activeGameObject == null)
            return;
        EditorGUILayout.LabelField("Information", EditorStyles.boldLabel);
        EditorGUILayout.LabelField(Selection.activeGameObject.transform.name + "  Is Selected");
        if (Selection.activeGameObject.transform == showingTrans)
            return;
        if (autoShow == false)
            return;
        OnSceneClickActive(Selection.activeGameObject.transform);
    }

    void OnSceneClickActive(Transform trans)
    {
        if (trans.parent && trans.parent == levelTrans.GetChild(0).GetChild(0))
            Show(trans, levelTrans.GetChild(0).GetChild(0));
        else if (trans.parent && trans.parent == levelTrans.GetChild(0).GetChild(1))
            Show(trans, levelTrans.GetChild(0).GetChild(1));
        else if (trans.parent && trans.parent == levelTrans.GetChild(0).GetChild(2))
            Show(trans, levelTrans.GetChild(0).GetChild(2));
        else if (trans == levelTrans.GetChild(0).GetChild(0))
        {
            HideAll();
            trans.localScale = Vector3.one;
            showingTrans = trans;
        }
        else if (trans==levelTrans.GetChild(0).GetChild(1))
        {
            HideAll();
            trans.localScale = Vector3.one;
            showingTrans = trans;
        }
        else if(trans==levelTrans.GetChild(0).GetChild(2)){
            HideAll();
            trans.localScale = Vector3.one;
            showingTrans = trans;
        }
    }

    void HideAll()
    {
        levelTrans.GetChild(0).GetChild(0).localScale = Vector3.zero;
        foreach (Transform mainScene in levelTrans.GetChild(0).GetChild(0))
        {
            mainScene.localScale = Vector3.zero;
        }
        levelTrans.GetChild(0).GetChild(1).localScale = Vector3.zero;
        foreach (Transform childScene in levelTrans.GetChild(0).GetChild(1))
        {
            childScene.localScale = Vector3.zero;
        }
        levelTrans.GetChild(0).GetChild(2).localScale = Vector3.zero;
        foreach (Transform child in levelTrans.GetChild(0).GetChild(2))
        {
            child.localScale = Vector3.zero;
        }
    }

    void Show(Transform showTrans, Transform parentTrans)
    {
        HideAll();
        parentTrans.localScale = Vector3.one;
        foreach (Transform trans in parentTrans)
        {
            if (trans == showTrans)
            {
                trans.localScale = Vector3.one;
                showingTrans = trans;
            }
        }
    }
}
