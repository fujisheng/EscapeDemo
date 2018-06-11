using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MoreGameManager;

public class PassLevelView : BaseView {

    Button nextButton;
    Button homeButton;
    Button likeButton;
    Button moreGameButton;

    private void Awake()
    {
        nextButton = transform.Find("nextButton").GetComponent<Button>();
        homeButton = transform.Find("homeButton").GetComponent<Button>();
        likeButton = transform.Find("likeButton").GetComponent<Button>();
        moreGameButton = transform.Find("moreGameButton").GetComponent<Button>();

        nextButton.onClick.AddListener(OnNextButtonClick);
        homeButton.onClick.AddListener(OnHomeButtonClick);
        likeButton.onClick.AddListener(OnLikeButtonClick);
        moreGameButton.onClick.AddListener(OnMoreGameButtonClick);
    }

    public override void OnOpened()
    {
        Mediator.SendMassage("outGame");
        Mediator.SendMassage("playAudio","gameOver");
        Mediator.SendMassage("showCubeBanner");
        List<Level> allLevel = Mediator.GetValue("allLevel") as List<Level>;
        Level nowLevel = Mediator.GetValue("nowLevel") as Level;
        if(allLevel.Exists((level)=>level.id==nowLevel.id+1)){
            moreGameButton.gameObject.SetActive(false);
			likeButton.gameObject.SetActive (false);
            nextButton.gameObject.SetActive(true);
        }else{
			moreGameButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);
			likeButton.gameObject.SetActive (true);
            if((Mediator.GetValue("moreGameInfo")as List<MoreGameInfo>).Count!=0){
                Mediator.SendMassage("openView", "moreGameView");
                Mediator.SendMassage("hideBanner");
            }
        }
    }

    void OnNextButtonClick(){
        Mediator.SendMassage("loadLevel", (Mediator.GetValue("nowLevel") as Level).id + 1);
    }

    void OnHomeButtonClick(){
        Mediator.SendMassage("openView", "levelSelectPannal");
    }

    void OnLikeButtonClick(){
        Mediator.SendMassage("startOpenLikeUrl");
    }

    void OnMoreGameButtonClick(){
        Mediator.SendMassage("openMoreGameUrl");
    }
}
