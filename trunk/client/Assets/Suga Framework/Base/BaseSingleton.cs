using UnityEngine;
using System.Collections;


namespace Framework
{
	//========================================================
	// class BaseSingleton
	//========================================================
	// - for making singleton object
	// - usage
	//		+ declare class(derived )	
	//			public class OnlyOne : BaseSingleton< OnlyOne >
	//		+ client
	//			OnlyOne.Instance.[method]
	//========================================================
	public abstract class BaseSingleton<T> where T : new()
	{
		private static T instance;
		public static T Instance
		{
			get
			{
				if (instance == null)
				{
                    Debug.Log("New of " + typeof(T));
					instance = new T();
				}
				return instance;
			}
		}
	}
}