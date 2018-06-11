using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Tools.Json;

public class GameSettingEditor : EditorWindow {

    string gameSettingPath;
    GameSetting json;

    [MenuItem("MyEditor/Game Setting")]
    static void Init()
    {
        GameSettingEditor gameSettingEditor = (GameSettingEditor)EditorWindow.GetWindow(typeof(GameSettingEditor), true, "Game Setting", false);
        gameSettingEditor.maxSize = new Vector2(900, 600);
        gameSettingEditor.minSize = new Vector2(900, 200);
        gameSettingEditor.Show();
    }

    void OnEnable()
    {
        gameSettingPath = "Text/";
        json = JsonFile.ReadFromFile<GameSetting>(gameSettingPath, "gameSetting");
        if (json == null)
            json = new GameSetting();
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("保存文件", GUILayout.Width(100)))
        {
            JsonFile.SaveToFile(json, gameSettingPath, "gameSetting");
        }
        EditorGUILayout.EndHorizontal();
        json.deleteData = EditorGUILayout.Toggle("deleteData",json.deleteData);
        json.gameId = EditorGUILayout.TextField("requestId",json.gameId);
        json.shareText = EditorGUILayout.TextField("shareText", json.shareText);
        json.shareTitle = EditorGUILayout.TextField("shareTitle", json.shareTitle);
        json.shareImageName = EditorGUILayout.TextField("shareImageName", json.shareImageName);

        EditorGUILayout.LabelField("Ios");
        json.iosAppId = EditorGUILayout.TextField("appId",json.iosAppId);
        json.iosDeveloperId = EditorGUILayout.TextField("developerId",json.iosDeveloperId);
        json.admobIosGeneralBannerUnitId = EditorGUILayout.TextField("admobGeneralUnitId",json.admobIosGeneralBannerUnitId);
        json.admobIosCubeBannerUnitId = EditorGUILayout.TextField("admobCubeBannerUnitId", json.admobIosCubeBannerUnitId);
        json.admobIosIntersUnitId = EditorGUILayout.TextField("admobIntersUnitId", json.admobIosIntersUnitId);
        json.admobIosRewardUnitId = EditorGUILayout.TextField("admobRewardUnitId", json.admobIosRewardUnitId);
        json.unityIosAdsGameId = EditorGUILayout.TextField("unityAdsGameId",json.unityIosAdsGameId);
        json.unityIosPlacementId = EditorGUILayout.TextField("unityPlacementId", json.unityIosPlacementId);
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Android");
        json.androidAppId = EditorGUILayout.TextField("appId",json.androidAppId);
        json.androidDeveloperId = EditorGUILayout.TextField("developerId",json.androidDeveloperId);
        json.admobAndroidGeneralBannerUnitId = EditorGUILayout.TextField("admobGeneralUnitId",json.admobAndroidGeneralBannerUnitId);
        json.admobAndroidCubeBannerUnitId = EditorGUILayout.TextField("admobCubeBannerUnitId", json.admobAndroidCubeBannerUnitId);
        json.admobAndroidIntersUnitId = EditorGUILayout.TextField("admobIntersUnitId", json.admobAndroidIntersUnitId);
        json.admobAndroidRewardUnitId = EditorGUILayout.TextField("admobRewardUnitId", json.admobAndroidRewardUnitId);
        json.unityAndroidAdsGameId = EditorGUILayout.TextField("unityAdsGameId",json.unityAndroidAdsGameId);
        json.unityAndroidPlacementId = EditorGUILayout.TextField("unityPlacementId", json.unityAndroidPlacementId);
    }

    void OnDestroy()
    {
        JsonFile.SaveToFile(json, gameSettingPath, "gameSetting");
    }
}
