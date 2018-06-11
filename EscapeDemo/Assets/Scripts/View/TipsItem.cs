using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsItem : MonoBehaviour {

    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetTips(Tips tips){
        image.sprite = Resources.Load<Sprite>("Image/Tips/" + tips.sprite);
        image.SetNativeSize();
    }
}
