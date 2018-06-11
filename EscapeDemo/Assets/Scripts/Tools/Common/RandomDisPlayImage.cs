using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomDisPlayImage : MonoBehaviour {

    public List<Sprite> spriteList = new List<Sprite>();

    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Show(){
        if (spriteList.Count == 0)
            return;
        int index = Random.Range(0, spriteList.Count);
        image.sprite = spriteList[index];
    }
}
