using UnityEngine;
using System.Collections;

public class AICardManager : PlayerCardManager {


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
}
