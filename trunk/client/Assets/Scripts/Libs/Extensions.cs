using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;

public static class MyExtensions
{
	#region EXTENSION FOR JSONOBJECT
	
	/// <summary>
	/// Determines whether this instance is null or empty the specified obj.
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance is null or empty the specified obj; otherwise, <c>false</c>.
	/// </returns>
	/// <param name='obj'>
	/// If set to <c>true</c> object.
	/// </param>
	public static bool IsNullOrEmpty(this JSONObject obj)
	{
		if(obj == null)
			return true;
		if(obj.type == JSONObject.Type.NULL)
			return true;
		
		if(obj.type == JSONObject.Type.ARRAY && (obj.list ==  null || obj.list.Count == 0))
			return true;
		
		return false;
	}
	
	/// <summary>
	/// Gets the int.
	/// </summary>
	/// <returns>
	/// The int.
	/// </returns>
	/// <param name='obj'>
	/// Object.
	/// </param>
	/// <param name='defaultVal'>
	/// Default value.
	/// </param>
	public static int GetInt(this JSONObject obj, int defaultVal = 0)
	{
		if(obj.IsNullOrEmpty())
			return defaultVal;
		
		int ret = 0;
		if(int.TryParse(obj.GetStr(),out ret))
			return ret;
		else
			return 0;
	}
	
	/// <summary>
	/// Gets the string.
	/// </summary>
	/// <returns>
	/// The string.
	/// </returns>
	/// <param name='obj'>
	/// Object.
	/// </param>
	/// <param name='defaultVal'>
	/// Default value.
	/// </param>
	public static string GetStr(this JSONObject obj, string defaultVal = "")
	{
		if(obj.IsNullOrEmpty())
			return defaultVal;
		
		string resultsStr = defaultVal;
		switch(obj.type)
		{
			case JSONObject.Type.STRING:   
				Regex regex = new Regex(@"\\[uU]([0-9A-F]{4})", RegexOptions.IgnoreCase);
     			resultsStr = regex.Replace(obj.str, match => ((char)int.Parse(match.Groups[1].Value, System.Globalization.NumberStyles.HexNumber)).ToString());
			break;
            case JSONObject.Type.NUMBER:   resultsStr = obj.n + ""; 
			break;
            case JSONObject.Type.BOOL:     resultsStr = obj.b + ""; 
			break;
			default:
				return resultsStr;
		}
		return resultsStr;
	}
	
	/// <summary>
	/// Gets the bool.
	/// </summary>
	/// <returns>
	/// The bool.
	/// </returns>
	/// <param name='obj'>
	/// If set to <c>true</c> object.
	/// </param>
	/// <param name='defaultVal'>
	/// If set to <c>true</c> default value.
	/// </param>
	public static bool GetBool(this JSONObject obj, bool defaultVal = false)
	{
		if(obj.IsNullOrEmpty())
			return defaultVal;
		
		return obj.b;
	}
	
	public static ArrayList GetList(this JSONObject obj)
	{
		if(obj.IsNullOrEmpty())
			return new ArrayList(0);
		
		return obj.list;
	}
	
	
	#endregion
}