using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessageBox;


public class GUIMessageDialog : GUIDialogBase
{
    #region Dialog Button Control
    public class DialogBtnControl
    {
        public Transform tranformBtn;
        public UILabel text;
        public DialogResult result;
        public DialogBtnControl(Transform _tran, UILabel _labelText)
        {
            tranformBtn = _tran;
            text = _labelText;
        }
        public void Reset()
        {
            result = DialogResult.None;
            text.text = "";
            tranformBtn.gameObject.SetActive(false);
        }
        public void SetInfomation(DialogResult _result, int _textId, Vector3 _location)
        {
            tranformBtn.gameObject.SetActive(true);
            result = _result;
            //text.text = AvLocalizationManager.GetString(_textId);
            tranformBtn.localPosition = _location;
        }
    }
    #endregion

    #region delegate
    public ButtonClick bt1_click;
    public ButtonClick bt2_click;
    public ButtonClick bt3_click;
    #endregion

    

    // content
    private UILabel contentMessage;
    private UILabel captionText;

    // image title 
    private UISprite imageTitle;

    // location btn first save
    public Vector3 location1Btn;
    public Vector3 location2Btn;
    public Vector3 location3Btn;

    public float distance = 125.0f;
    // current Result for three button
    private DialogBtnControl[] btnDialog = new DialogBtnControl[3];

    // control static
    private static List<MessageItem> items = new List<MessageItem>();

    private List<SpriteRenderer> listShow = new List<SpriteRenderer>();
//    private static GUIMessageDialog messageHandler;
    public void Awake()
    {
        //messageHandler = this;
    }


    //********************* Overide *********************//
    public override GameObject OnInit()
    {
        //Debug.Log("dialog messagebox on init");

        GameObject obj = Util.LoadPreafab(GUI_PATH_PREFAB + "UIMessageDialog", gameObject); // AvGameObjectUtils.LoadGameObject(gameObject.transform, );
        obj.name = "UIMessageDialog";
        guiControlLocation = obj.transform.Find("UIContent").gameObject;
		guiControlLocation.transform.localPosition = new Vector3(0, 1000, -100.0f);
        // find object
        contentMessage = guiControlLocation.transform.FindChild("messageText").GetComponent<UILabel>();
        captionText = guiControlLocation.transform.Find("captionText").GetComponent<UILabel>();
        for (int i = 0; i < 3; i++)
        {
            Transform _tran = guiControlLocation.transform.Find("Btn0" + (i + 1).ToString()); 
            _tran.gameObject.GetComponent<UIForwardEvents>().target = gameObject;
            UILabel _label = _tran.Find("staticText").GetComponent<UILabel>();
            btnDialog[i] = new DialogBtnControl(_tran, _label);
            btnDialog[i].Reset();
        }
        layer =20;
        UIPanel panel = obj.GetComponent<UIPanel>();
        if (panel != null)
            panel.depth = layer;
        /*UIWidget[] wiget = gameObject.GetComponentsInChildren<UIWidget>();
        //Debug.LogError(wiget.Length);
        for (int i = 0; i < wiget.Length; i++)
        {
            wiget[i].depth += (layer +50);
        }*/
        return obj;
    }

    protected override float OnBeginShow(object parameter)
    {
        // to do
        if (listShow.Count > 0)
        {
            for (int i = 0; i < listShow.Count; i++)
            {
                listShow[i].enabled = true;
            }
        }
        listShow.Clear();
        Transform parent = gameObject.transform.parent;
        SpriteRenderer[] _sprites= parent.gameObject.GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < _sprites.Length; i++)
        {
            if (_sprites[i].enabled == true)
            {
                listShow.Add(_sprites[i]);
            }
        }
        for (int i = 0; i < listShow.Count; i++)
        {
            listShow[i].enabled = false;
        }
        guiControlDlg.SetActive(true);
        //MessageItem item1=new MessageItem();
        if (parameter != null)
        {
            MessageItem item = (MessageItem)parameter;
            SetupDisplayButtons(item);
            contentMessage.text = item.message;
            captionText.text = item.caption;
            guiControlLocation.transform.localPosition = new Vector3(0, 0, -100.0f);
        }
        else
        {
            Debug.LogError("Method to open Message Dialog is not exactly");
        }
        return base.OnBeginShow(parameter);
    }

    protected override float OnBeginHide(object parameter)
    {
        return base.OnBeginHide(parameter);
    }

    protected override void OnEndHide(bool isDestroy)
    {
        for (int i = 0; i < listShow.Count; i++)
        {
            listShow[i].enabled = true;
        }
        listShow.Clear();
    }

    protected override void OnEndShow()
    {
       
    }

    //********************  End override ****************//
    void OnClick()
    {
        switch (UICamera.selectedObject.name)
        {
            case "Btn01":
                {
                    //Debug.LogError("111");
                    OnBtnClick(0);
                    if (bt1_click != null) bt1_click();
                }
                break;

            case "Btn02":
                {
                    //Debug.LogError("222");
                    OnBtnClick(1);
                    if (bt2_click != null) bt2_click();
                }
                break;

            case "Btn03":
                {
                    //Debug.LogError("333");
                    OnBtnClick(2);
                    if (bt3_click != null) bt3_click();
                }
                break;
        }
    }

    public void OnBtnClick(int i)
    {

        bool close = true;

        if (items[items.Count - 1].callback != null)
            close = items[items.Count - 1].callback(btnDialog[i].result);

        if (!close)
            return;

        items.RemoveAt(items.Count - 1);

        if (close && !CheckShowMessageDialog())
            AvUIManager.instance.HideDialog(DialogName.MessageBox);
    }

    public bool CheckShowMessageDialog()
    {
        if (items.Count > 0)
        {
            MessageItem _item=items[items.Count - 1];
            AvUIManager.instance.ShowDialog(DialogName.MessageBox, _item);
            return true;
        }

        return false;
    }


    #region simulator message same with .Net
    private void ResetMessageState()
    {
        for (int i = 0; i < 3; i++)
        {
            btnDialog[i].Reset();
        }
    }

    private void SetupDisplayButtons(MessageItem item)
    {
        ResetMessageState();
        switch (item.buttons)
        {
            case Buttons.OK:
			btnDialog[0].SetInfomation(DialogResult.Ok, 9017, location1Btn);
                break;

            case Buttons.OKCancel:
			btnDialog[0].SetInfomation(DialogResult.Ok, 9017,  new Vector3(location2Btn.x+260,location2Btn.y,location2Btn.z));
			btnDialog[1].SetInfomation(DialogResult.Cancel, 31,location2Btn);
                break;

            case Buttons.AbortRetryIgnore:
                btnDialog[0].SetInfomation(DialogResult.Abort, 34, location3Btn);
                btnDialog[1].SetInfomation(DialogResult.Retry, 35, new Vector3(location3Btn.x + 200, location2Btn.y, location2Btn.z));
                btnDialog[2].SetInfomation(DialogResult.Ignore, 36, new Vector3(location3Btn.x + 200*2, location2Btn.y, location2Btn.z));
                break;

            case Buttons.YesNoCancel:
                btnDialog[0].SetInfomation(DialogResult.Yes, 32, location3Btn);
                btnDialog[1].SetInfomation(DialogResult.No, 33, new Vector3(location3Btn.x + 200, location2Btn.y, location2Btn.z));
                btnDialog[2].SetInfomation(DialogResult.Cancel, 31, new Vector3(location3Btn.x + 200 * 2, location2Btn.y, location2Btn.z));
                break;

            case Buttons.YesNo:
			btnDialog[0].SetInfomation(DialogResult.Yes, 32, new Vector3(location2Btn.x + 260, location2Btn.y, location2Btn.z));
			btnDialog[1].SetInfomation(DialogResult.No, 33, location2Btn);
                break;

            case Buttons.RetryCancel:
                btnDialog[0].SetInfomation(DialogResult.Retry, 35, location2Btn);
                btnDialog[1].SetInfomation(DialogResult.Cancel, 31, new Vector3(location2Btn.x + 260, location2Btn.y, location2Btn.z));
                break;

            default:
                btnDialog[0].SetInfomation(DialogResult.None, 37, location1Btn);
                break;
        }
    }

	public static void Show( string message)
	{
		Show(null, message, string.Empty, Buttons.OK, Icon.None, MessageBox.DefaultButton.Button1);
	}

    public static void Show(MessageCallback callback, string message)
    {
        Show(callback, message, string.Empty, Buttons.OK, Icon.None, MessageBox.DefaultButton.Button1);
    }

    public static void Show(MessageCallback callback, string message, string caption)
    {
        Show(callback, message, caption, Buttons.OK, Icon.None, MessageBox.DefaultButton.Button1);
    }

    public static void Show(MessageCallback callback, string message, string caption, Buttons buttons)
    {
        Show(callback, message, caption, buttons, Icon.None, MessageBox.DefaultButton.Button1);
    }

    public static void Show(MessageCallback callback, string message, string caption, Buttons buttons, Icon icon)
    {
        Show(callback, message, caption, buttons, icon, MessageBox.DefaultButton.Button1);
    }

    public static void Show(MessageCallback callback, string message, string caption, Buttons buttons, Icon icon, MessageBox.DefaultButton defaultButton)
    {
		//if(string.IsNullOrEmpty(caption))
			//caption = AvLocalizationManager.GetString(6);
        MessageItem item = new MessageItem
        {
            caption = caption,
            buttons = buttons,
            defaultButton = defaultButton,
            callback = callback
        };
        item.message = message;
        switch (icon)
        {
            case Icon.Hand:
            case Icon.Stop:
            case Icon.Error:
                //item.message.image = messageHandler.error;
                break;

            case Icon.Exclamation:
            case Icon.Warning:
                //item.message.image = messageHandler.warning;
                break;

            case Icon.Asterisk:
            case Icon.Information:
                //item.message.image = messageHandler.info;
                break;
        }
        if (items.Count > 2)
        {
            items.RemoveAt(0);
        }
        items.Add(item);

		AvUIManager.instance.ShowDialog(DialogName.MessageBox, item);
    }
    #endregion


}
