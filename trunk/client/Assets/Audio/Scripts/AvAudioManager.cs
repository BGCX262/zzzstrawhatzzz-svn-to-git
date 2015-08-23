
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
//using SFCLIB;


public class AvAudioManager : SingletonMono<AvAudioManager>
{
    const string soundPrefab = "Prefabs/Audio/SoundPlayer";
    const string musicPrefab = "Prefabs/Audio/MusicPlayer";

	private Dictionary<string,AudioClip>	listEffectSound = new Dictionary<string,AudioClip>();
	private GameObject						soundPlayerObject		= null;
	private SoundPlayerScript				soundPlayerInterface	= null;

	private Dictionary<string, AudioClip>	listMusic = new Dictionary<string, AudioClip>();
	private GameObject						musicPlayerObject		= null;
	private MusicPlayerScript				musicPlayerInterface	= null;

	private AudioListener mListener = null;

	void Start()
	{
		//Debug.LogWarning("START AUDIO MANAGAGER");
		mListener = GetComponent<AudioListener>();
	}

	private void CheckToAddAudioListenner()
	{
		if(mListener == null)
		{
			mListener = GameObject.FindObjectOfType(typeof(AudioListener)) as AudioListener;
			
			if (mListener == null)
			{
//				Debug.LogWarning("No audio listeneradd");
				mListener = gameObject.AddComponent<AudioListener>();
			}
		}
	}

	//===============================================
	//
	//===============================================
	public void Load()
	{
		//LoadSound();
		//LoadMusic();
	}
	//===============================================
	//
	//===============================================
	private void LoadSound()
	{

		string rootFilePathSound = Application.dataPath + "/Resources/Audio/Sound";	
		
		//
		//string[] arrayFilePathSound = Directory.GetFiles( @rootFilePathSound, "*.*", SearchOption.TopDirectoryOnly ).Where(s => s.EndsWith(".mp3") || s.EndsWith(".wav"));;
		string nameResources = "Resources";
        
		foreach (string filePathSound in System.Array.FindAll(Directory.GetFiles( @rootFilePathSound, "*.*", SearchOption.AllDirectories ), PredicateEffectSoundFileMatch ))
		{
			int beginIndex 	= filePathSound.IndexOf(nameResources) + nameResources.Length + 1;
 			int	endIndex	= filePathSound.LastIndexOf(".");
			
			string filePathForLoading = filePathSound.Substring( beginIndex, endIndex - beginIndex );
			filePathForLoading = filePathForLoading.Replace("\\", "/" );
			
			AudioClip soundClip = Resources.Load(filePathForLoading) as AudioClip;

			string[] listToken = filePathForLoading.Split('/');
            int length = listToken.Length;
			string nameSoundClip = listToken[length - 1];
			if (listToken[length - 2] != "Sound")
            {
                nameSoundClip = listToken[length - 2] + "_" + nameSoundClip;
            }
            Debug.Log("LoadSound_" + nameSoundClip);
			listEffectSound.Add(nameSoundClip, soundClip);
			//Debug.LogError(filePathForLoading);
		}
	}


	//================================================================================================
	//
	//================================================================================================
	private bool PredicateEffectSoundFileMatch(string fileName)
	{
	    if (fileName.EndsWith(".mp3"))
	        return true;
	    if (fileName.EndsWith(".wav"))
	        return true;
		if (fileName.EndsWith(".ogg"))
			return true;
	    return false;
	}
	//===============================================
	//
	//===============================================
	private void LoadMusic()
	{

        string rootFilePathSound = Application.dataPath + "/Resources/Audio/Music";
		

		//
		//string[] arrayFilePathSound = Directory.GetFiles( @rootFilePathSound, "*.*", SearchOption.TopDirectoryOnly ).Where(s => s.EndsWith(".mp3") || s.EndsWith(".wav"));;
		string nameResources = "Resources";
		foreach (string filePathSound in System.Array.FindAll(Directory.GetFiles( @rootFilePathSound, "*.*", SearchOption.AllDirectories ), PredicateMusicSoundFileMatch ))
		{
			int beginIndex 	= filePathSound.IndexOf(nameResources) + nameResources.Length + 1;
 			int	endIndex	= filePathSound.LastIndexOf(".");
			
			string filePathForLoading = filePathSound.Substring( beginIndex, endIndex - beginIndex );
			filePathForLoading = filePathForLoading.Replace("\\", "/" );
			
			AudioClip soundClip = Resources.Load(filePathForLoading) as AudioClip;

			string[] listToken = filePathForLoading.Split('/');
            int length = listToken.Length;
            string nameSoundClip = listToken[length - 1];
            if (listToken[length - 2] != "Music")
            {
                nameSoundClip = listToken[length - 2] + "_" + nameSoundClip;
            }
			
			
			listMusic.Add(nameSoundClip, soundClip);
		}
	
		
	}
	//================================================================
	//
	//================================================================
	private bool PredicateMusicSoundFileMatch(string fileName)
	{
	    if (fileName.EndsWith(".ogg"))
	        return true;
	    if (fileName.EndsWith(".mp3"))
	        return true;
	    return false;
	}
	
	//===============================================
	//
	//===============================================
	public AvAudioManager()
	{
		//Load();
	}
	//===============================================
	//
	//===============================================

    public bool PlaySound(SoundName sound, bool playLoop = false, float timeDelay = 0f)
    {
        return PlaySound(sound.ToString(), playLoop, timeDelay);
    }

	public bool PlaySound(string inName, bool playLoop, float timeDelay)
	{
		Debug.Log("PlaySound " + inName);
		CheckToAddAudioListenner();

		SoundPlayerScript player = GetSoundPlayerInterface();

		if (listEffectSound.ContainsKey(inName))
		{
			AudioClip soundClip = listEffectSound[inName];
			if (soundClip != null)
			{
                Debug.Log("Call PlaySound");
				if(!playLoop)
					player.PlaySound(soundClip);
				else
					player.PlaySoundLoop(soundClip, timeDelay);
			}
			else
			{
				Debug.Log("No Found Sound File");
				return false;
			}
		}
		else
		{
			string filePath = "Audio/Sound/" + inName;
			// create
			AudioClip soundClip = Resources.Load(filePath) as AudioClip;
			if( soundClip != null )
			{
				listEffectSound.Add(inName, soundClip);
				if(!playLoop)
					player.PlaySound(soundClip);
				else
					player.PlaySoundLoop(soundClip, timeDelay);
				return true;
			}
			Debug.LogError("No Found SOUND File Name!!!" + filePath);
			return false;
		}
		return true;
	}

	public bool StopSound(SoundName soundName)
	{
		return StopSound(soundName.ToString());
	}

	public bool StopSound(string inName)
	{	
		SoundPlayerScript player = GetSoundPlayerInterface();
		if (listEffectSound.ContainsKey(inName))
		{
			AudioClip soundClip = listEffectSound[inName];
			if(soundClip != null)
				player.StopSound(inName);
		}
		return true;
	}

	//===============================================
	//
	//===============================================
    public bool PlayMusic(MusicName music)
    {
        return PlayMusic(music.ToString());
    }

	public bool PlayMusic(string inName)
	{		
		Debug.Log("PlayMusic " + inName);
		CheckToAddAudioListenner();

		MusicPlayerScript player = GetMusicPlayerInterface();        
		//
		if (listMusic.ContainsKey(inName))
		{
            //Debug.LogError(player.gameObject.name);
			AudioClip musicClip = listMusic[inName];
			if (musicClip != null)
			{
				player.PlaySound(musicClip);
			}
			else
			{
				Debug.Log("No Found Music File");
				return false;
			}
		}
		else
		{
			string filePath = "Audio/Music/" + inName;
			// create
			AudioClip musicClip = Resources.Load(filePath) as AudioClip;
			if( musicClip != null )
			{
				listMusic.Add(inName, musicClip);
				player.PlaySound(musicClip);
				return true;
			}
			Debug.LogError("No Found Music File Name!!!" + filePath);
			return false;
		}
		return true;
	}
	//===============================================
	//
	//===============================================
	public bool MuteSound( bool inIsMute )
	{
		SoundPlayerScript player = GetSoundPlayerInterface();

		player.Mute(inIsMute);

		return false;
	}
	//===============================================
	//
	//===============================================
	public bool MuteMusic( bool inIsMute )
	{
		MusicPlayerScript player = GetMusicPlayerInterface();

		player.Mute(inIsMute);

		return false;
	}

    public void StopMusic()
    {
        MusicPlayerScript player = GetMusicPlayerInterface();
        player.Stop();
    }
    public void StopMusicDelay(float _volume = 1)
    {
		StartCoroutine(StopMusicDelayRoutine(_volume));
    }

    private IEnumerator StopMusicDelayRoutine( float _volume = 1)
    {       
        yield return new WaitForSeconds(0.05f);
        
        MusicPlayerScript player = GetMusicPlayerInterface();
        _volume -= 0.05f;
        if (_volume < 0)
        {
            _volume = 1;
            player.Stop();
            player.Volume(_volume);
        }
        else
        {
            player.Volume(_volume);
            
			StartCoroutine(StopMusicDelayRoutine(_volume));
        }
    }

	//===============================================
	//
	//===============================================
	public bool PauseMusic()
	{
		if (musicPlayerInterface != null)
		{
			musicPlayerInterface.PauseSound();
		}
		return false;
	}
	//===============================================
	//
	//===============================================
	private SoundPlayerScript GetSoundPlayerInterface()
	{
		if (soundPlayerObject == null)
		{
			soundPlayerObject = GameObject.Instantiate(Resources.Load(soundPrefab)) as GameObject;
			soundPlayerInterface = soundPlayerObject.GetComponent<SoundPlayerScript>();
		}
		return soundPlayerInterface;
	}
	//===============================================
	//
	//===============================================
	private MusicPlayerScript GetMusicPlayerInterface()
	{
		if (musicPlayerObject == null)
		{
			Debug.Log("GetMusicPlayerInterface");
			musicPlayerObject = GameObject.Instantiate(Resources.Load(musicPrefab)) as GameObject;
			musicPlayerInterface = musicPlayerObject.GetComponent<MusicPlayerScript>();
		}
		return musicPlayerInterface;
	}

}
