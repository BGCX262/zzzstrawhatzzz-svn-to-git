using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum GUIPanelHideAction
{
	Disable = 0,
	Destroy,
}


public class GUIDialogBase : MonoBehaviour 
{
	public enum GUIPanelStatus
	{
        Invalid,
		Ok,
		Showing,
		Showed,
		Hiding,
		Hidden
	}

    public const string GUI_PATH_PREFAB = "Prefabs/Dialogs/";

	// Hide action
	public DialogName dialogName = DialogName.None;
    public string dialogPrefab="";
    public string locationName = "UIContent";
    public int layer =0;
    public GUIPanelHideAction hideAction;
	public float destroyTimeout = 120;


	public float showDelay;

	// Show/Hide tween
	public string showTweenName = "show";
	public string hideTweenName = "hide";


    public bool useBlackBackground = true;

	// Active state
	private Dictionary<GameObject, bool> activeSave;
    [HideInInspector]
    public  GUIPanelStatus status = GUIPanelStatus.Invalid;
    private bool isHasAlpha = false;
    public bool isSetupLocation=false;
    public Vector2 vectorSetupLocation = new Vector2(0, 0); 

    [HideInInspector]
    public GameObject guiControlDlg;
    [HideInInspector]
    public GameObject guiControlLocation;

    [HideInInspector]
    GUIBaseDialogHandler uiBaseDialogHandler;

    public GUIBaseDialogHandler GetDialogHandler()
    {
        return uiBaseDialogHandler;
    }

    public virtual GameObject OnInit()
    {
        try
        {
            //Debug.LogError("Init Dialog" + name);
			Debug.Log("Load dialog " + dialogPrefab);
            GameObject obj = Util.LoadPreafab(GUI_PATH_PREFAB + dialogPrefab, gameObject);
            obj.name = dialogPrefab;
            if (obj.transform.Find(locationName) != null)
            {
                guiControlLocation = obj.transform.Find(locationName).gameObject;
				guiControlLocation.transform.localPosition = new Vector3(1000,0,0);
            }
            UIPanel panel = obj.GetComponent<UIPanel>();
            if (panel != null)
                panel.depth = layer;
   
            UIPanel[] panels = obj.GetComponentsInChildren<UIPanel>();
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].depth = layer;
            }
            uiBaseDialogHandler = gameObject.GetComponentInChildren<GUIBaseDialogHandler>();
            return obj;
        }
        catch (Exception ex)
        {
            Debug.LogError("Error:" + ex.Message);
        }
        return null;
    }

	/// <summary>
	/// Inits this instance.
	/// </summary>
	public bool Init()
	{
        if (status >= GUIPanelStatus.Ok)
			return true;
		status = GUIPanelStatus.Invalid; 
		
		try
		{
			guiControlDlg = OnInit();
           	if (guiControlDlg != null)
			{
				status = GUIPanelStatus.Ok;
			}
           
			return true;
		}
		catch (System.Exception ex)
		{
			Debug.LogError("Init GUI panel - Init " + this.GetType().Name + "Exception - "+ ex);
		}

		return false;
	}


	/// <summary>
	/// Tries the show.
	/// </summary>
	public bool TryShow(object parameter)
	{
        if (status == GUIPanelStatus.Invalid)
        {
            Init();
        }
		StopCoroutine("WaitForDestroy");

        // End hiding
        if (status == GUIPanelStatus.Hiding)
        {
            iTween.Stop(guiControlLocation);
            OnEndHide(hideAction == GUIPanelHideAction.Destroy);
            status = GUIPanelStatus.Hidden;
        }

		if (status == GUIPanelStatus.Hidden || status == GUIPanelStatus.Ok)
		{
			status = GUIPanelStatus.Showing;

			RestoreActiveState(true);
            if (isHasAlpha)
            {
                isHasAlpha = false;
                OnResetAlpha();
            }
            //Debug.LogWarning("2" + parameter);
			StartCoroutine(DoShow(parameter));
			//Debug.LogWarning("3");
			return true;
		}

		if (status == GUIPanelStatus.Showed || status == GUIPanelStatus.Showing)
		{
            StartCoroutine(DoShow(parameter));
			Debug.LogWarning("4");
			return true;
		}
		Debug.LogWarning("5");
		return false;
	}

	private IEnumerator DoShow(object parameter)
	{
        //Debug.LogWarning("DoShow" + parameter);
		yield return new WaitForSeconds(showDelay);
        if (isSetupLocation && guiControlLocation!=null)
        {
            Vector3 pos = guiControlLocation.transform.localPosition;
            pos.x = vectorSetupLocation.x;
            pos.y = vectorSetupLocation.y;
            pos.z = 0;
            guiControlLocation.transform.localPosition = pos;
        }
        //Debug.LogWarning("OnBeginShow" + parameter);
		float wait = OnBeginShow(parameter);


        AvUIManager.instance.CheckShowBorder();
		if (wait > 0)
			yield return new WaitForSeconds(wait);
		else
			yield return null;
        if (status == GUIPanelStatus.Showing)
        {
            OnEndShow();
            status = GUIPanelStatus.Showed;
        }
	}


	/// <summary>
	/// Hides the specified after time out.
	/// </summary>
	public void Hide(object parameter)
	{
		if (gameObject == null)
			return;

        // End showing
        if (status == GUIPanelStatus.Showing)
        {
            iTween.Stop(guiControlLocation);
            OnEndShow();
            status = GUIPanelStatus.Showed;
        }

		if (status == GUIPanelStatus.Showed)
		{
			status = GUIPanelStatus.Hiding;

			SaveActiveState();

			StartCoroutine(DoHide(parameter));
		}
        AvUIManager.instance.CheckShowBorder();
    }
    public void InitialDefaulState()
    {
        status = GUIPanelStatus.Hiding;
        SaveActiveState();
		guiControlDlg.SetActive(false);
    }

	/// <summary>
	/// Does the hide.
	/// </summary>
	private IEnumerator DoHide(object parameter)
	{
		float wait = OnBeginHide(parameter);
		if (wait > 0)
			yield return new WaitForSeconds(wait);
		else
			yield return null;

        if (status == GUIPanelStatus.Hiding)
        {
            OnEndHide(hideAction == GUIPanelHideAction.Destroy);

            switch (hideAction)
            {
                case GUIPanelHideAction.Disable:
					guiControlDlg.SetActive(false);
                    status = GUIPanelStatus.Hidden;
                    break;

                case GUIPanelHideAction.Destroy:
					guiControlDlg.SetActive(false);
					status = GUIPanelStatus.Hidden;
					StartCoroutine("WaitForDestroy");
                    break;
            }
        }
	}

	IEnumerator WaitForDestroy()
	{
		yield return new WaitForSeconds(destroyTimeout);
		Destroy(guiControlDlg);
        status = GUIPanelStatus.Invalid;
	}

	/// <summary>
	/// Does the tween.
	/// </summary>
	protected float DoTween(string tweenName)
	{
		if (string.IsNullOrEmpty(tweenName))
			return 0;

		iTweenEvent tween = gameObject.GetComponents<iTweenEvent>().Where(tw => tw.tweenName == tweenName).FirstOrDefault();
        if (tween != null)
		{
            if (tween.type != iTweenEvent.TweenType.ValueTo)
            {
                tween.SetObjectTarget(guiControlLocation);
            }
            else
            {
                tween.SetObjectTarget(gameObject);
            }
            tween.Play();
			if (!tween.Values.ContainsKey("time"))
				return 2f;

			return (float)tween.Values["time"];
		}
		return 0;
	}
    private void OnUpdateAlpha(double alpha)
    {
        UIPanel[] panels = gameObject.GetComponentsInChildren<UIPanel>();
        for (int i = 0; i < panels.Length; i++)
        {
            if (alpha > 1)
                alpha = 1;
            else if (alpha < 0)
                alpha = 0;
            panels[i].alpha = (float)alpha;
        }
        isHasAlpha = true;
    }
    private void OnResetAlpha()
    {
        UIPanel[] panels = gameObject.GetComponentsInChildren<UIPanel>();
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].alpha = 0.99f;
        }
    }
	/// <summary>
	/// Called when [begin show].
	/// </summary>
	protected virtual float OnBeginShow(object parameter)
	{
        if (uiBaseDialogHandler != null)
        {
            uiBaseDialogHandler.OnBeginShow(parameter);
        }
        ApplyDepthPosition(guiControlLocation);
        AvUIManager.instance.CheckShowBorder();
		return DoTween(showTweenName);
	}

	/// <summary>
	/// Called when [end show].
	/// </summary>
	protected virtual void OnEndShow()
	{
		ApplyDepthPosition(guiControlLocation);
        AvUIManager.instance.CheckShowBorder();
	}

	/// <summary>
	/// Called when [begin hide].
	/// </summary>
	protected virtual float OnBeginHide(object parameter)
	{
        if (uiBaseDialogHandler != null)
        {
            uiBaseDialogHandler.OnBeginHide(parameter);
        }
		return DoTween(hideTweenName);
	}

	/// <summary>
	/// Called when [end hide].
	/// </summary>
	protected virtual void OnEndHide(bool isDestroy)
	{
	}

	/// <summary>
	/// Saves the state of the active.
	/// </summary>
	protected void SaveActiveState()
	{
		if (guiControlDlg == null)
			return;

		//Debug.LogError("save");
		if (activeSave == null)
			activeSave = new Dictionary<GameObject, bool>();
		else
			activeSave.Clear();

		activeSave[guiControlDlg] = gameObject.activeSelf;

		Transform[] children = gameObject.GetComponentsInChildren<Transform>(true);
		foreach (var child in children)
		{
			activeSave[child.gameObject] = child.gameObject.activeSelf;
		}
	}

	/// <summary>
	/// Restores the state of the active.
	/// </summary>
	protected void RestoreActiveState(bool defaultState)
	{
		if (gameObject == null || activeSave == null)
			return;

		//Debug.LogError("restore");
		bool isActive = false;
		if (activeSave.TryGetValue(gameObject, out isActive))
			gameObject.SetActive(isActive);

		Transform[] children = gameObject.GetComponentsInChildren<Transform>(true);
		foreach (var child in children)
		{
			if (activeSave.TryGetValue(child.gameObject, out isActive))
				child.gameObject.SetActive(isActive);
			else
				child.gameObject.SetActive(defaultState);
		}
	}

	public void ApplyDepthPosition(GameObject guiObject)
	{
		if (guiObject == null)
			return;
        if (layer >=15)
        {
            layer = 15;
        }
        Vector3 pos = guiObject.transform.localPosition;
        if (layer >= 0)
        {
            pos.z = -20-layer*10;
        }
        else
        {
            pos.z = -20-150;
        }
        guiObject.transform.localPosition = pos;
    }
}
