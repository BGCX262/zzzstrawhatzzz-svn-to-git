using UnityEngine;
using System.Collections;

public class GameOfflineManager : SingletonMono<GameOfflineManager> {
    public PlayerCardManager playerCardManager;
    public AICardManager aiCardManager;

    void Awake()
    {
       
    }
	// Use this for initialization
	void Start () {
        
	}

    public void StartGame()
    {
        if(playerCardManager == null)
            playerCardManager = GameObject.FindObjectOfType<PlayerCardManager>();
        if(aiCardManager == null)
            aiCardManager = GameObject.FindObjectOfType<AICardManager>();
        playerCardManager.onCompleteDeal += OnCompleteDeal;
        playerCardManager.DealCard();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnCompleteDeal(ePlayerType type)
    {
        if (type == ePlayerType.Player)
            aiCardManager.DealCard();
        
    }
}
