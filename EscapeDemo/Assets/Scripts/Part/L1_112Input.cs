using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class L1_112Input : MonoBehaviour {

    InputField inputField;
    Button button;
    public string needString;
    public UnityEvent onComplete;

    private void Awake()
    {
        inputField = transform.Find("InputField").GetComponent<InputField>();
        button = transform.Find("button").GetComponent<Button>();

        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick(){
        if (inputField.text == needString)
            onComplete.Invoke();
        else
            inputField.text = string.Empty;
    }
}
