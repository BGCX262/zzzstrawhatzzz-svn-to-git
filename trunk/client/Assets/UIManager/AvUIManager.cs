using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AvUIManager :  SingletonMono<AvUIManager> {

	private List<GUIDialogBase> listDialogs = null;

    public GameObject blackBorder;
	public GameObject loading;
    public Camera uiCamera;
    
    private int screenWidth=0;
    private int screenHeight=0;
	public UICamera GetUICamera()
    {
        return uiCamera.GetComponent<UICamera>();
    }
	// Use this for initialization
    void Start()
    {
        Reset();
		listDialogs = new List<GUIDialogBase>(GetComponentsInChildren<GUIDialogBase>());
	}
	
	// Update is called once per frame
    void Update()
    {
        UpdateScreen();
    }
    public void Reset()
    {
        screenWidth = screenHeight = 1;
    }
    void UpdateScreen()
    {
//         if (screenWidth != Screen.width || screenHeight != Screen.height)
//         {
//             screenWidth = Screen.width;
//             screenHeight = Screen.height;
//             //Debug.LogError(screenWidth + "," + screenHeight + (((float)screenWidth / (float)screenHeight)).ToString());
//             if (((float)screenWidth / (float)screenHeight) < 1.61f)
//             {
//                 UIAnchor[] anchors = GameObject.FindObjectsOfType(typeof(UIAnchor)) as UIAnchor[];
//                 //Debug.LogError("BBBBBBBBBB"+anchors.Length);
//                 for (int i = 0; i < anchors.Length; i++)
//                 {
//                     Vector3 vec = anchors[i].autoScaleSmall;
//                     anchors[i].gameObject.transform.localScale = vec;
//                 }
//             }
//             else
//             {
// 				UIAnchor[] anchors = GameObject.FindObjectsOfType(typeof(UIAnchor)) as UIAnchor[];
// 
//                 //Debug.LogError("AAAAAAAAAAA" + anchors.Length);
//                 for (int i = 0; i < anchors.Length; i++)
//                 {
//                     Vector3 vec = anchors[i].autoScaleNormal;
//                     anchors[i].gameObject.transform.localScale = vec;
//                 }
//             }
//         }
    }

	public void ShowLoading()
	{
		blackBorder.SetActive(true);
		loading.SetActive(true);
	}

	public void HideLoading()
	{
		blackBorder.SetActive(false);
		loading.SetActive(false);
	}
  
	public void ShowDialog(DialogName dlgName, object param = null)
	{
		GUIDialogBase foundDlg = listDialogs.Find( dlg => dlg.dialogName == dlgName);
		if(foundDlg == null)
		{
			Debug.LogError("Ko tim thay dialog:" + dlgName);
			return;
		}
		Debug.Log("Start show dialog:" + dlgName);
		if (!foundDlg.TryShow(param))
		{
			Debug.LogError("Ko the show dialog:" + dlgName);
		}
	}
	public void HideDialog(DialogName dlgName, object param = null)
	{
		GUIDialogBase foundDlg = listDialogs.Find( dlg => dlg.dialogName == dlgName);
		if(foundDlg == null)
		{
			Debug.LogError("Ko tim thay dialog:" + dlgName);
			return;
		}
		Debug.Log("Hide dialog:" + dlgName);
		foundDlg.Hide(param);
	}

    public GUIDialogBase GetDialog(DialogName dlgName)
    {
        return listDialogs.Find( dlg => dlg.dialogName == dlgName);
    }

	public void HideDialogAfterTime(DialogName dlgName,float delayTime)
	{
		StartCoroutine(CoroutineHideDialog(dlgName, delayTime));
	}
	private IEnumerator CoroutineHideDialog(DialogName dlgName,float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		HideDialog(dlgName);
	}


	public void HideAllDialog()
	{
		if(listDialogs == null) return;

		foreach(var dlg in listDialogs)
		{
			if(dlg != null)
				dlg.Hide(null);
		}		
	}

//
//    public static void ShowDialog(GUIDialogBase panelController)
//    {
//        ShowDialog(panelController, null);
//    }
//
//    public static void ShowDialog(GUIDialogBase panelController, object parameter)
//    {
//        if (!panelController.TryShow(parameter))
//        {
//            Debug.LogError("Error:" + panelController.name);
//        }
//    }
//
//    public static void HideDialogAfterTime(GUIDialogBase panelController,float _time)
//    {
//        AvUIManager.Instance.StartCoroutine(AvUIManager.Instance.CoroutineHideDialog(panelController, _time));
//    }
//    private IEnumerator CoroutineHideDialog(GUIDialogBase panelController,float _time)
//    {
//        yield return new WaitForSeconds(_time);
//        HideDialog(panelController);
//    }
//    public static void HideDialog(GUIDialogBase panelController)
//    {
//        HideDialog(panelController, null);
//    }
//
//    public static void HideDialog(GUIDialogBase panelController, object parameter)
//    {
//		if(panelController == null)
//		{
//			Debug.LogError("tao lao bi dao roi");
//			return;
//		}
//        panelController.Hide(parameter);
//    }

    public void CheckShowBorder()
    {
        GUIDialogBase topShow = null;
       // GUIDialogBase[] controlers = gameObject.GetComponentsInChildren<GUIDialogBase>();

        for (int i = 0; i < listDialogs.Count; i++)
        {
			if (listDialogs[i].useBlackBackground)
            {
				if (listDialogs[i].status == GUIDialogBase.GUIPanelStatus.Showed
				    || listDialogs[i].status == GUIDialogBase.GUIPanelStatus.Showing)
                {
                    if (topShow == null)
                    {
						topShow = listDialogs[i];
                    }
                    else
                    {
                        if (topShow.guiControlLocation != null && listDialogs[i].guiControlLocation != null)
                        {
							if (topShow.layer < listDialogs[i].layer)
                            {
								topShow = listDialogs[i];
                            }
                        }
                    }
                }
            }
        }
        if (topShow != null && topShow.guiControlLocation != null)
        {
            blackBorder.SetActive(true);
            blackBorder.GetComponent<UIPanel>().depth = topShow.layer-1;
        }
        else
        {
			blackBorder.SetActive(false);
        }
    }
	
}
