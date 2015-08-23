using UnityEngine;
using System.Collections;

public class SoundPlayerScript : MonoBehaviour
{
	public void PlaySound(AudioClip inClip )
	{
		//DontDestroyOnLoad(gameObject);
		audio.PlayOneShot(inClip);
	}

	public void PlaySoundLoop(AudioClip inClip, float timeDelay = 0)
	{
		audio.loop = true;
		audio.clip = inClip;
		//audio.Play();
		audio.PlayDelayed(timeDelay);
	}

	public void StopSound(string clipName)
	{
		if(audio.isPlaying && audio.clip.name.Equals(clipName))
			audio.Stop();
	}

	//========================================
	//
	//========================================
	public void Mute(bool inIsMute)
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

    
}
