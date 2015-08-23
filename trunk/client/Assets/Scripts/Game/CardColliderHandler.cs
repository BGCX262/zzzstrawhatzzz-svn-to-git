using UnityEngine;
using System.Collections;

public class CardColliderHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.LogError("Enter " + other.name);
    }


    void OnTriggerExit2D(Collider2D other)
    {
        Debug.LogError("Exit " + other.name);
    }

    
}
