using UnityEngine;
using System.Collections;

public class Util 
{
    public static System.Random rand = new System.Random();

    public static GameObject LoadPreafab(string prefab)
    {
        return (GameObject)Resources.Load(prefab, typeof(GameObject)); 
    }

    public static GameObject LoadPreafab(string prefab, GameObject parent)
    {
        GameObject prefabObj = Resources.Load(prefab, typeof(GameObject)) as GameObject;
        GameObject go = GameObject.Instantiate(prefabObj) as GameObject;
        if (go == null)
        {
            Debug.Log("Can't load " + prefab);
            return null;
        }
        go.transform.parent = parent.transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;
        return go;
    }

    public static GameObject LoadPreafab(GameObject prefab)
    {
        if (prefab == null) return null;
        return GameObject.Instantiate(prefab) as GameObject;
    }

    public static GameObject LoadPreafab(GameObject prefab, GameObject parent)
    {
        if (prefab == null) return null;
        GameObject go = GameObject.Instantiate(prefab) as GameObject;
        go.transform.parent = parent.transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;
        return go;
    }


    public static Sprite LoadSprite(string sprite)
    {
        return Resources.Load(sprite, typeof(Sprite)) as Sprite;
    }

    public static void RunTweenMove(GameObject target, float x, float y, float time)
    {
        iTween.MoveTo(target, iTween.Hash(
                               "x", x,
                               "y", y,
                               "time", 0.5f,
                               "isLocal", true           
                               )
                           );
    }

    public static void RunTweenMove(GameObject target, float x, float y, float time, string completeFun)
    {
        iTween.MoveTo(target, iTween.Hash(
                               "x", x,
                               "y", y,
                               "time", 0.5f,
                               "isLocal", true,
                                "onCompleteTarget", target,
                                "onComplete", completeFun
                               )
                           );
    }
}
