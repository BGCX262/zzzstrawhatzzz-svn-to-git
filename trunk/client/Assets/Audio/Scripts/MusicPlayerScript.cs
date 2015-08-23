using UnityEngine;
using System.Collections;

public class MusicPlayerScript : MonoBehaviour {
	//========================================
	//
	//========================================
	void Awake()
	{
		//DontDestroyOnLoad(gameObject);
		audio.loop = true;
	}
	//========================================
	//
	//========================================
	public void PlaySound(AudioClip inClip)
	{

		audio.clip = inClip;
		audio.Play();
	}
	//========================================
	//
	//========================================
	public void PauseSound()
	{
		audio.Pause();
	}
	//========================================
	//
	//========================================
	public void Mute( bool inIsMute )
	{
		audio.mute = inIsMute;
	}
	//========================================
	//
	//========================================	
	public bool IsMute()
	{
		return audio.mute;
	}

    public void Volume(float _volume)
    {
        
        audio.volume = _volume;
    }

    public void Stop()
    {
        audio.Stop();
    }
}
