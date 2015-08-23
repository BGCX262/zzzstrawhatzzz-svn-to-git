using UnityEngine;
using System.Collections;

public class BoardCellAnimator : MonoBehaviour {

	// Use this for initialization
    Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlayAnimation(eBoardCellAnimation ani)
    {
        
        if(animator == null) return;
        switch (ani)
        {
            case eBoardCellAnimation.Idle:
                animator.SetTrigger("Idle");
                break;
            case eBoardCellAnimation.IsCanSelect:
                animator.SetTrigger("Select");
                break;
        }
    }

}
