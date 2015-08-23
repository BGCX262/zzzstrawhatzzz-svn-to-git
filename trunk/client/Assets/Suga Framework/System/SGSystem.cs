using UnityEngine;
using System;
using System.Collections;

public enum SGPlayerMode
{
    None = -1,
    Single = 0,
    Multi = 1,
    Online = 2
}

public class SGSystem : SingletonMono<SGSystem>
{
    public System.Random randomGenerator = new System.Random((int)DateTime.Now.Ticks & 0x0000FFFF);

    private bool enableCheat = true;

    public float boundBottom = 0.0f;
    public float boundLeft = 0.0f;
    public float boundTop = 0.0f;
    public float boundRight = 0.0f;

	void Awake()
	{
		Application.targetFrameRate = 60;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}

    public void CalculateBound()
    {
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, Camera.main.transform.position.y));
        boundBottom = bottomLeft.z;
        boundLeft = bottomLeft.x;

        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, Camera.main.transform.position.y));
        boundTop = topRight.z;
        boundRight = topRight.x;
    }

    public bool IsEnableCheat()
    {
        return enableCheat;
    }
}
