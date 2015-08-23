using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GFramework;

public class ConfigManager : Singleton<ConfigManager>
{
    //public static ConfigString configString;

    public static ConfigCard configCard;
    
    private void LoadDataConfig<TConfigTable>(ref TConfigTable configTable, params string[] dataPaths) where TConfigTable : IConfigDataTable, new()
	{
		try
		{
			if (configTable == null)
			{
				configTable = new TConfigTable();

				configTable.BeginLoadAppend();
				foreach (var path in dataPaths)
				{
					configTable.LoadFromAssetPath(path);
				}
				configTable.EndLoadAppend();

				Debug.LogWarning("Config loaded:"+ configTable.GetName());
			}
		}
		catch (System.Exception ex)
		{
            Debug.LogError("Load Config Error:"+ configTable.GetName()+", "+ ex.ToString());
		}
	}

    public void Init()
    {
        InitLevel();
    }

    
    public void InitLevel()
    {
        LoadDataConfig<ConfigCard>(ref configCard, "Config/ConfigCard");
    }

	public void Init1()
	{
        //sLoadDataConfig<ConfigString>(ref configString, "Config/ConfigString");
    }

	public void UnLoad()
	{
	}
}
