using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void CompleteDealEvent(ePlayerType type);

public class PlayerCardManager : MonoBehaviour {

    public ePlayerType type;
    public GameObject cardContainer;
    public List<ConfigCardRecord> cardDesk = new List<ConfigCardRecord>();
    public List<CardHandler> rowCard = new List<CardHandler>();
    public List<CardHandler> putCard = new List<CardHandler>();
    public List<CardHandler> lockCard = new List<CardHandler>();
    public bool[] rowCardCheck;
    int curPileIndex = 1;
    int curRowCardCount = 0;    

    public CompleteDealEvent onCompleteDeal;
    void Awake()
    {
        rowCardCheck = new bool[GameConst.rowCardLength];
        for (int i = 0; i < GameConst.rowCardLength; i++)
            rowCardCheck[i] = false;
        SetupDesk();
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetupDesk()
    {
        if (ConfigManager.configCard == null)
            ConfigManager.instance.Init();
        List<ConfigCardRecord> records = ConfigManager.configCard.GetAllCard();
        List<ConfigCardRecord> cards = new List<ConfigCardRecord>();
        foreach (ConfigCardRecord c in records)
        {
            cards.Add(c);
        }

        int count = cards.Count;
        while (count > 0)
        {
            int index = Util.rand.Next(count);
            cardDesk.Add(cards[index]);
            cards.RemoveAt(index);
            count--;
        }
        Debug.LogError(cardDesk.Count);
    }

    public void DealCard()
    {
        //Debug.LogError("AAAAAAA");
        if(cardDesk.Count > 0 && curRowCardCount < GameConst.rowCardLength)
        {
            //Debug.LogError("AAAAAAA");
            GameObject card = Util.LoadPreafab(GameConst.cardPrefab, cardContainer);
            CardHandler hander = card.GetComponent<CardHandler>();
            hander.playerType = type;
            hander.sprite.sprite = Util.LoadSprite(cardDesk[0].GetSpritePath());
            cardDesk.RemoveAt(0);
            GameObject target;
            if(type == ePlayerType.Player)
            {
                card.transform.localPosition = BoardManager.instance.GetFrontPileByIndex(0).transform.localPosition;
                target = BoardManager.instance.GetFrontPileByIndex(curPileIndex);
            }
            else
            {
                card.transform.localPosition = BoardManager.instance.GetBackPileByIndex(0).transform.localPosition;
                target = BoardManager.instance.GetBackPileByIndex(curPileIndex);
            }
            card.transform.localScale = target.transform.localScale;
            hander.Deal(curPileIndex);
            rowCard.Add(hander);
            rowCardCheck[curPileIndex - 1] = true;
            curPileIndex++;
            curRowCardCount++;             
            iTween.MoveTo(card, iTween.Hash(
                    "x", target.transform.localPosition.x,
                    "y", target.transform.localPosition.y,                    
                    "time", 1,
                    "isLocal", true,
                    "oncompletetarget", gameObject,
                    "oncomplete", "CompleteDealTween"                
                    )
                );
        }
        else
        {
            if (onCompleteDeal != null)
                onCompleteDeal(type);
        }
    }

    public void CompleteDealTween()
    {
        DealCard();
    }   

    public void Lock()
    {
        SortCard();
        foreach (CardHandler card in putCard)
        {
            lockCard.Add(card);
            card.Lock = true;
            card.RowPileIndex = -1;            
        }
        putCard.Clear();
    
    }

    public virtual void SortCard()
    {
        if (rowCard.Count == 0) return;
        Debug.LogError("i'm here");
        
        for (int i = 0; i < GameConst.rowCardLength; i++)
        {            
            if (!rowCardCheck[i])
            {
                int j = 0;
                while (j < rowCard.Count)
                {
                    
                    CardHandler car = rowCard[j];
                    if (car.RowPileIndex - 1 <= i) j++;
                    else
                    {
                        rowCardCheck[car.RowPileIndex - 1] = false;
                        car.RowPileIndex = i + 1;
                        rowCardCheck[i] = true;
                        
                        GameObject target =  BoardManager.instance.GetFrontPileByIndex(car.RowPileIndex);
                        iTween.MoveTo(car.gameObject, iTween.Hash(
                           "x", target.transform.localPosition.x,
                           "y", target.transform.localPosition.y,
                           "time", 1,
                           "isLocal", true
                           )
                       );
                        break;
                    }
                }
                if (j >= rowCard.Count) break;
            }
        }
    }

    public virtual void PutCard()
    {
        CardHandler curCard = PlayerController.GetCurrentCard();
        if (curCard == null) return;
        rowCardCheck[curCard.RowPileIndex - 1] = false;
        rowCard.Remove(curCard);
        putCard.Add(curCard);
    }   

}
