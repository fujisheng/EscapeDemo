using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PreloaderView : BaseView ,IPopUps{

    private void Update()
    {
        this.transform.Rotate(Time.fixedDeltaTime * 0, Time.fixedDeltaTime * 0, Time.fixedDeltaTime * -720f);
    }
}
