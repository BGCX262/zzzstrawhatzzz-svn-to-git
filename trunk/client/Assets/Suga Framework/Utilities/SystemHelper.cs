using UnityEngine;
using System.Collections;
using GFramework;
using System;



public class SystemHelper {

	private static string _deviceUniqueID;

	public static string deviceUniqueID
	{
		get
		{
            try
            {
                if (_deviceUniqueID == null)
                    computeDeviceUniqueID();
            }
            catch (Exception ex)
            {
                _deviceUniqueID = "Default";
            }
			return _deviceUniqueID;
		}
	}

	private static void computeDeviceUniqueID()
	{
		string systemID = "";
		systemID = SystemInfo.deviceUniqueIdentifier;
		_deviceUniqueID = systemID.Replace("-", "").ToLower();
	}
	
}