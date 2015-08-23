using UnityEngine;
using System.Collections;


public class PlayerController : MonoBehaviour {

	// Use this for initialization
    static CardHandler curCard;

	void Start () {
        FingerGestures.OnFingerDown += OnFingerDown;
        //FingerGestures.OnDragMove += OnDragMove;
        //FingerGestures.OnDragBegin += OnDragBegin;
        //ingerGestures.OnDragEnd += OnDragEnd;
        //FingerGestures.OnFingerUp += OnFingerUp;   
       
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnFingerDown( int fingerIndex, Vector2 fingerPos )
    {
        CardHandler preCard = curCard;
        Camera uiCam = CameraManager.instance.uiCamera;
        if (uiCam != null)
        {
            Ray ray = uiCam.ScreenPointToRay(fingerPos);
            if (Physics.Raycast(ray, Mathf.Infinity, 1 << (int)eLayerName.UI))
            {
                return;
            }
        }

        GameObject obj = PickObject(fingerPos);
       
        if (obj != null && obj.layer == (int)eLayerName.Card)
        {            
            Debug.Log("Touch Down " + fingerPos);        
            CardHandler card = obj.GetComponent<CardHandler>();
            
            curCard = card;
            if (preCard != null && curCard != preCard )
                preCard.OnEndFocus();
            if (card.playerType == ePlayerType.Player)
            {
                curCard.OnClick();
            }
            else
            {

            }
        }
        else if (preCard != null)
        {
            preCard.OnEndFocus();
        }
    }

    public void OnDragMove( Vector2 fingerPos, Vector2 delta )
    {
        if (curCard != null && curCard.playerType == ePlayerType.Player && curCard.Dragging)
        {
            Camera mainCamera = Camera.main;
            Vector3 pos = mainCamera.ScreenToWorldPoint(new Vector3(fingerPos.x, fingerPos.y, Mathf.Abs(curCard.gameObject.transform.position.z - mainCamera.transform.position.z)));
            //Debug.LogError(pos);
            //pos.z = 0;
            curCard.gameObject.transform.position = pos;
            //Debug.Log("Move " +delta);
            //Vector3 pos = curCard.transform.localPosition;
            //curCard.transform.localPosition =  Vector3.MoveTowards(pos, new Vector3(pos.x + delta.x, pos.y + delta.y, pos.z), 20);
        }
        
    }   

    public void OnFingerUp(int fingerIndex, Vector2 fingerPos, float timeHeldDown)
    {
        if(curCard != null && !curCard.Dragging)
            Debug.LogError("Up");        
    }

    public static GameObject PickObject(Vector2 screenPos)
    {
        /*Vector3 MousePosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.transform.position.z));
       
        RaycastHit2D hit = Physics2D.Raycast(MousePosition, -Vector2.up);
        if (hit != null && hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        return null;*/

        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
            return hit.collider.gameObject;

        return null;
    }


    public static CardHandler GetCurrentCard()
    {
        return curCard;
    }
}
