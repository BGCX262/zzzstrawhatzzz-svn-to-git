using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class AudioToolEditor : ScriptableObject
{
    [MenuItem("Tools/Audio/CreateAudioEnum")]
    static void CreateTool()
    {
		string outStr = "public enum eSoundName {\n";
        string nameResources = "Resources";
       
       //effect
        string rootFilePathSound = Application.dataPath + "/Resources/Audio/Sound";
        foreach (string filePathSound in System.Array.FindAll(Directory.GetFiles(@rootFilePathSound, "*.*", SearchOption.AllDirectories), predicateMusicSoundFileMatch))
        {
            int beginIndex = filePathSound.IndexOf(nameResources) + nameResources.Length + 1;
            int endIndex = filePathSound.LastIndexOf(".");

            string filePathForLoading = filePathSound.Substring(beginIndex, endIndex - beginIndex);
            filePathForLoading = filePathForLoading.Replace("\\", "/");

           

            string[] listToken = filePathForLoading.Split('/');
            int length = listToken.Length;
            string nameSoundClip = listToken[length - 1];
			if (listToken[length - 2] != "Sound")
            {
                nameSoundClip = listToken[length - 2] + "_" + nameSoundClip;
            }

            outStr += "\t" + nameSoundClip + ",\n";
        }
        outStr += " };\n";
        
        //music
		outStr += "public enum MusicName {\n";
        rootFilePathSound = Application.dataPath + "/Resources/Audio/Music";
        foreach (string filePathSound in System.Array.FindAll(Directory.GetFiles(@rootFilePathSound, "*.*", SearchOption.AllDirectories), predicateMusicSoundFileMatch))
        {
            int beginIndex = filePathSound.IndexOf(nameResources) + nameResources.Length + 1;
            int endIndex = filePathSound.LastIndexOf(".");

            string filePathForLoading = filePathSound.Substring(beginIndex, endIndex - beginIndex);
            filePathForLoading = filePathForLoading.Replace("\\", "/");           

            string[] listToken = filePathForLoading.Split('/');
            int length = listToken.Length;
            string nameSoundClip = listToken[length - 1];
            if (listToken[length - 2] != "Music")
            {
                nameSoundClip = listToken[length - 2] + "_" + nameSoundClip;
            }

            outStr += "\t" + nameSoundClip + ",\n";
        }
        outStr += " };\n";
        //
        Debug.Log(outStr);
        FileStream fs = new FileStream(Application.dataPath + "/Scripts/Audio/DefineAudio.cs", FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);
        sw.Write(outStr);
        sw.Flush();
        sw.Close();
        fs.Close();
        EditorApplication.RepaintProjectWindow();
    }

    static bool predicateMusicSoundFileMatch(string fileName)
    {
        if (fileName.EndsWith(".ogg"))
            return true;
        if (fileName.EndsWith(".mp3"))
            return true;
        return false;
    }

    static bool predicateEffectSoundFileMatch(string fileName)
    {
        if (fileName.EndsWith(".mp3"))
            return true;
        if (fileName.EndsWith(".wav"))
            return true;
        return false;
    }
    

    //[MenuItem("Tools/Audio/Auto Add Click sound for button")]
    //static void AutoSetSound()
    //{
    //    AudioClip clip = Resources.Load("Audio/Sound/Click") as AudioClip;

    //    UIButton[] allBtn = GameObject.FindObjectsOfType<UIButton>();
    //    foreach(var btn in allBtn)
    //    {
    //        UIPlaySound sound = btn.GetComponent<UIPlaySound>();
    //        if(sound == null)
    //        {
    //            btn.gameObject.AddComponent<UIPlaySound>();
    //        }
    //    }

    //    UIPlaySound[] allSoundBtn = GameObject.FindObjectsOfType<UIPlaySound>();
    //    foreach(var soundBtn in allSoundBtn)
    //    {
    //        if(soundBtn.audioClip == null)
    //        {
    //            soundBtn.audioClip = clip;
    //        }
    //    }
    //}
}
