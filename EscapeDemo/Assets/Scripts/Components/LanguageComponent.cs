using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageComponent : MonoBehaviour {

    Text text;
    public string key;

    private void Awake()
    {
        text = transform.GetComponent<Text>();
        if(!string.IsNullOrEmpty(LanguageManager.GetInstance().GetString(key)))
            text.text = LanguageManager.GetInstance().GetString(key);
    }
}
