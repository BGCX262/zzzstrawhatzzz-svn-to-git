using UnityEngine;
using System.Collections;

public class GameUIHandler : SingletonMono<GameUIHandler> {

    public UIFollowTarget cardMenu;
	// Use this for initialization
	void Start () {
        GameOfflineManager.instance.StartGame();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void FinishPutCard()
    {        
        GameOfflineManager.instance.playerCardManager.Lock();
    }

    public void ShowCardMenu(GameObject target)
    {
        cardMenu.target = target.transform;
        cardMenu.gameObject.SetActive(true);
    }

    public void HideCardMenu()
    {
        cardMenu.gameObject.SetActive(false);
    }

    public void Summon()
    {
        CardHandler curCard = PlayerController.GetCurrentCard();
        if (curCard != null)
            curCard.Summon();
    }
}
