﻿//--------------------------------------------
//            NGUI: HUD Text
// Copyright © 2012 Tasharen Entertainment
//--------------------------------------------

using UnityEngine;

/// <summary>
/// Attaching this script to an object will make it visibly follow another object, even if the two are using different cameras to draw them.
/// </summary>

[AddComponentMenu("NGUI/Examples/Follow Target")]
public class UIFollowTarget : MonoBehaviour
{
	/// <summary>
	/// 3D target that this object will be positioned above.
	/// </summary>

	public Transform target;

	/// <summary>
	/// Whether the children will be disabled when this object is no longer visible.
	/// </summary>

	public bool disableIfInvisible = true;

	Transform mTrans;
	Camera mGameCamera;
	Camera mUICamera;
	//bool mIsVisible = false;

	/// <summary>
	/// Cache the transform;
	/// </summary>

	void Awake () { mTrans = transform; }

	/// <summary>
	/// Find both the UI camera and the game camera so they can be used for the position calculations
	/// </summary>

	void Start()
	{
		if (target != null)
		{
            mGameCamera = Camera.mainCamera;
            if (mGameCamera == null)
            {
                if (GameObject.FindGameObjectWithTag("MainCamera") != null)
                    mGameCamera = GameObject.FindGameObjectWithTag("MainCamera").camera;
            }
			mUICamera = GameObject.FindObjectOfType<UICamera>().camera;
			SetVisible(false);
		}
		else
		{
			//Debug.LogError("Expected to have 'target' set to a valid transform", this);
			//enabled = false;
		}
	}

	/// <summary>
	/// Enable or disable child objects.
	/// </summary>

	void SetVisible (bool val)
	{
		//mIsVisible = val;
		//Debug.LogWarning("Set Visible : " + val + mTrans.childCount);
		for (int i = 0, imax = mTrans.childCount; i < imax; ++i)
		{
			NGUITools.SetActive(mTrans.GetChild(i).gameObject, val);
		}
	}

	/// <summary>
	/// Update the position of the HUD object every frame such that is position correctly over top of its real world object.
	/// </summary>

	void Update ()
	{
		if (target != null)
		{
            if (mGameCamera == null)
            {
                mGameCamera = Camera.mainCamera;
                if (mGameCamera == null)
                {
                    if (GameObject.FindGameObjectWithTag("MainCamera") != null)
                        mGameCamera = GameObject.FindObjectOfType<UICamera>().camera;
                }
            }
			if(mUICamera == null)
			{
				mUICamera = GameObject.FindObjectOfType<UICamera>().camera;
			}
			//SetVisible(false);
		}
		else{
			return;
		}

		Vector3 pos = mGameCamera.WorldToViewportPoint(target.position);
		// Determine the visibility and the target alpha
		bool isVisible = (pos.z > 0f && pos.x > 0f && pos.x < 1f && pos.y > 0f && pos.y < 1f);
		// Update the visibility flag
		//if (disableIfInvisible && mIsVisible != isVisible) SetVisible(isVisible);

		SetVisible(isVisible);

		//mTrans.localPosition = new Vector3( 1000.0f,1000.0f,1000.0f);
		// If visible, update the position
		if (isVisible)
		{

			Vector3 convert = mUICamera.ViewportToWorldPoint(pos);
			transform.position = convert;
			pos = mTrans.localPosition;
			pos.x = Mathf.RoundToInt(pos.x);
			pos.y = Mathf.RoundToInt(pos.y);
			pos.z = 0f;
			mTrans.localPosition = pos;
		}

	}
}
