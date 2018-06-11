using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;

public class LevelSelectPannal : BaseView{

    Transform grid;
    Button returnButton;
	Button nextButton;
	Button lastButton;
    GameObject buttonPrefab;
    List<LevelButton> allButton = new List<LevelButton>();

	List<Level> allLevel = new List<Level> ();
	List<Level> unlockLevel = new List<Level> ();

	Level showingLevel;

    private void Awake()
    {
        grid = transform.Find("Scroll View/grid");
        returnButton = transform.Find("returnButton").GetComponent<Button>();
		nextButton = transform.Find ("nextButton").GetComponent<Button> ();
		lastButton = transform.Find ("lastButton").GetComponent<Button> ();
        buttonPrefab = Resources.Load<GameObject>("Prefabs/View/levelButton");

        returnButton.onClick.AddListener(OnReturnButtonClick);
		nextButton.onClick.AddListener (OnNextButtonClick);
		lastButton.onClick.AddListener (OnLastButtonClick);
    }

    void OnReturnButtonClick(){
        Mediator.SendMassage("openView", "startMenuView");
    }

    public override void OnOpened()
    {
        Debug.Log("outGame");
        Mediator.SendMassage("outGame");
		Mediator.SendMassage ("hideBanner");
		allLevel = Mediator.GetValue ("allLevel")as List<Level>;
		unlockLevel = Mediator.GetValue ("unlockLevel") as List<Level>;
        if(grid.transform.GetChildCount()!=0){
            foreach (Transform child in grid.transform)
            {
                Destroy(child.gameObject);
            }
            allButton.Clear();
        }

		foreach(var level in allLevel){
            GameObject obj = Instantiate(buttonPrefab);
            obj.transform.SetParent(grid);
            obj.transform.localScale = Vector3.one;
            allButton.Add(obj.GetComponent<LevelButton>());
            obj.GetComponent<LevelButton>().SetLevel(level);
        }
        foreach (var level in unlockLevel)
        {
            allButton.Find((levelButton) => levelButton.level.id == level.id).Unlock();
        }

		showingLevel = unlockLevel[0];
		ShowLevelButton (allLevel.Find ((level) => level.id == showingLevel.id), 0f);
    }

	void ShowLevelButton(Level level,float duration){
		Vector3 pos = allButton.Find ((button) => button.level.id == level.id).transform.localPosition;
		grid.transform.DOLocalMoveX(-pos.x, duration,true);
		showingLevel = level;
		ShowOderButton ();
	}

	void ShowOderButton(){
		if (showingLevel != allLevel [0]) {
			lastButton.gameObject.SetActive (true);
		} else {
			lastButton.gameObject.SetActive (false);
		}

		if (showingLevel != allLevel.Last ()) {
			nextButton.gameObject.SetActive (true);
		} else {
			nextButton.gameObject.SetActive (false);
		}
	}

	void OnNextButtonClick(){
		Level nextLevel = allLevel.Find ((level) => level.id == showingLevel.id + 1);
		ShowLevelButton (nextLevel,0.5f);
	}

	void OnLastButtonClick(){
		Level lastLevel = allLevel.Find ((level) => level.id == showingLevel.id - 1);
		ShowLevelButton (lastLevel,0.5f);
	}
}
