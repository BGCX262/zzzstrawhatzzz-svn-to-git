using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : SingletonMono<BoardManager> {

    public GameObject cardContainer;

    public List<BoardCellHandler> frontMonster;
    public List<BoardCellHandler> frontMagic;

    public List<BoardCellHandler> backMonster;
    public List<BoardCellHandler> backMagic;

    public List<BoardCellHandler> canSelectList = new List<BoardCellHandler>();

    public GameObject[] frontRowPileList;
    public GameObject[] backRowPileList;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowCellCanSelect()
    {
        
        CardHandler card = PlayerController.GetCurrentCard();
        if (card == null) return;
        if(card.type == eCardType.Monster)
        {
            if (frontMonster != null)
            {
                foreach (BoardCellHandler cell in frontMonster)
                {
                    if (cell.card == null)
                    {
                        cell.animator.PlayAnimation(eBoardCellAnimation.IsCanSelect);
                        canSelectList.Add(cell);
                    }
                }
            }
        }
    }

    public void HideCellCanSelect()
    {
        foreach (BoardCellHandler cell in canSelectList)
        {
            cell.animator.PlayAnimation(eBoardCellAnimation.Idle);
        }
        canSelectList.Clear();
    }

    public bool PutCard(Vector2 screenPos)
    {
        CardHandler curCard = PlayerController.GetCurrentCard();
        if (curCard == null) return false;
        foreach (BoardCellHandler cell in canSelectList)
        {
            if (cell.IsContain(screenPos))
            {
                curCard.PutToCell(cell);
                cell.card = curCard;
                return true;
            }
        }
        return false;
    }   

    public BoardCellHandler GetCellForPlayerMonster()
    {
        foreach (BoardCellHandler cell in frontMonster)
        {
            if (cell.card == null)
                return cell;
        }
        return null;
    }

    public GameObject GetFrontPileByIndex(int index)
    {
        return frontRowPileList[index];
    }

    public GameObject GetBackPileByIndex(int index)
    {
        return backRowPileList[index];
    }
}
