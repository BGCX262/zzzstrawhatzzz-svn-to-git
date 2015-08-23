using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FileHelpers;
using GFramework;

[DelimitedRecord("\t")]
[IgnoreFirst(1)]
[IgnoreCommentedLines("//")]
[IgnoreEmptyLines(true)]

public class ConfigCardRecord
{
    public int id;    
    public string name;
    public string icon;
    public int attack;
    public int defent;
    public string effect;
    public string note;

    public string GetSpritePath()
    {
        return "Sprite/Card/" + name;
    }
}

public class ConfigCard : GConfigDataTable<ConfigCardRecord>
 {
    public ConfigCard()
        : base("ConfigCard")
	{
	}
	
	protected override void OnDataLoaded()
	{
		//rebuild index to get
		RebuildIndexField<int>("id");
	}

    public ConfigCardRecord GetCardByID(int id)
	{
		return FindRecordByIndex<int>("id", id);
	}

    public List<ConfigCardRecord> GetAllCard()
	{
		return records;
	}

}
