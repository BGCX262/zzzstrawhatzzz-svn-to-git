using UnityEngine;
using System.Collections;

public class CardHandler : MonoBehaviour {

    //public BoardCellAnimator animator;
    const float inRowY = -13;
    const float inViewY = -11;
    public ePlayerType playerType;
    public eCardType type;
    public eCardStatus status = eCardStatus.InRow;
    bool isDragging = false;
    bool isLock = false;
    public BoardCellHandler cellContainer;
    public SpriteRenderer sprite;
    int rowPileIndex = 0;
    
    
    public int RowPileIndex
    {
        get { return rowPileIndex; }
        set { rowPileIndex = value; }
    }

    public bool Lock
    {
        get { return isLock; }
        set { isLock = value; }
    }

    public bool Dragging
    {
        get { return isDragging; }
        set { isDragging = value; }
    }

    void Awake()
    {


    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PutToCell(BoardCellHandler cell)
    {
        transform.parent = cell.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        sprite.sortingOrder = -1;
        cellContainer = cell;
    }

    public void Deal(int  index)
    {
        rowPileIndex = index;
    }

    public void OnClick()
    {
        if (isDragging) return;
        if (status == eCardStatus.InRow)
        {
            GameUIHandler.instance.ShowCardMenu(BoardManager.instance.GetFrontPileByIndex(rowPileIndex));
            status = eCardStatus.InView;
            isDragging = true;
            Util.RunTweenMove(gameObject, transform.localPosition.x, inViewY, 0.5f, "onTweenComplete");
        }
        else if(status == eCardStatus.InView)
        {
            GameUIHandler.instance.HideCardMenu();
            status = eCardStatus.InRow;
            isDragging = true;
            Util.RunTweenMove(gameObject, transform.localPosition.x, inRowY, 0.5f, "onTweenComplete");
        }
    }

    public void OnEndFocus()
    {
        if (status == eCardStatus.InView)
        {
            status = eCardStatus.InRow;
            isDragging = true;
            GameUIHandler.instance.HideCardMenu();
            Util.RunTweenMove(gameObject, transform.localPosition.x, inRowY, 0.5f, "onTweenComplete");
        }
    }

    public void Summon()
    {
        if (playerType != ePlayerType.Player || type != eCardType.Monster || status != eCardStatus.InView) return;
        BoardCellHandler cell = BoardManager.instance.GetCellForPlayerMonster();
        if (cell == null) return;
        
    }

    public void Set()
    {

    }

    public void onTweenComplete()
    {
        isDragging = false;
    }

    
}
