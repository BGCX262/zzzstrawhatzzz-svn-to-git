//using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Reflection;
using JsonFx.Json;

namespace Framework.Network
{
	/// <summary>
/// Request parameters base class
/// </summary>
	public class RequestParams
	{
		public JSONObject ConvertToJSON()
		{
			JSONObject paramsList = new JSONObject(JSONObject.Type.OBJECT);
			
			foreach (FieldInfo field in this.GetType().GetFields())
			{
               // UnityEngine.Debug.Log("field.Name:" + field.Name);
				string val = JsonWriter.Serialize(field.GetValue(this));
				
				paramsList.AddField(field.Name, new JSONObject(val));
			}
			
			return paramsList;
		}
		
		public string HashParams(string key)
		{
			StringBuilder sb = new StringBuilder();
			System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create();
			
			foreach (FieldInfo field in this.GetType().GetFields())
			{
				var val = field.GetValue(this).ToString();
		    	byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(key + val);
				byte[] hashSha256 = sha256.ComputeHash(inputBytes);
		    	for (int i = 0; i < hashSha256.Length; i++)
		    	{
					sb.Append(hashSha256[i].ToString("X2"));
		    	}
			}
			
	    	return sb.ToString();
		}
	}
	
	/// <summary>
	/// Response parameters. Base class
	/// </summary>
	public class ResponseParams
	{
        
	}
}
