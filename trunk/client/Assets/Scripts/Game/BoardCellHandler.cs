using UnityEngine;
using System.Collections;

public class BoardCellHandler : MonoBehaviour {

    public eCardType typeContains;
    bool isContainer = false;
    public BoardCellAnimator animator;
    Rect rectInScreen = new Rect(0,0,50,50);
    public CardHandler card;

    public bool IsContainer
    {
        get { return isContainer; }
        set { isContainer = value; }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Awake()
    {
        animator = GetComponent<BoardCellAnimator>();
        Vector3 pos = Camera.mainCamera.WorldToScreenPoint(transform.position);
        rectInScreen.center = new Vector2(pos.x, pos.y);
    }

    public bool IsContain(Vector2 screenPos)
    {
        return rectInScreen.Contains(screenPos);
    }
    
}
