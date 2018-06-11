using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class L6_606Book : MonoBehaviour {

    Text number1;
    Text number2;
    Button button;
    int page = 1;

    private void Awake()
    {
        number1 = transform.Find("number1").GetComponent<Text>();
        number2 = transform.Find("number2").GetComponent<Text>();
        button = transform.Find("button").GetComponent<Button>();

        button.onClick.AddListener(OnButtonClick);

        number1.text = "1243";
        number2.text = "1432";
    }


    void OnButtonClick(){
        page++;
        if(page>=3){
            page = 1;
        }

        if(page==1){
            number1.text = "1243";
            number2.text = "1432";
        }
        else if(page==2){
            number1.text = "4312";
            number2.text = "4123";
        }
    }
}
