using UnityEngine;
using System.Collections;

public class ControladorBaseSonido : MonoBehaviour
{

	public static ControladorBaseSonido Instance;
	public AudioClip[] GameSounds;
	private int totalSounds;
	private ArrayList soundObjectList;
	private SoundObject tempSoundObj;
	public float volume= 1;
	public string gamePrefsName= "DefaultGame";

	public void Awake()
	{
 		Instance= this;
	}
	
	void Start ()
	{
		volume= PlayerPrefs.GetFloat(gamePrefsName+"_SFXVol");
 		Debug.Log ("BaseSoundController gets volume from prefs "+gamePrefsName+"_SFXVol at "+volume);
 		soundObjectList=new ArrayList();
 		
 		foreach(AudioClip theSound in GameSounds)
 		{
			tempSoundObj= new SoundObject(theSound,theSound.name, volume);
 			soundObjectList.Add(tempSoundObj);
 			totalSounds++;
 		}
	}
	public void PlaySoundByIndex(int anIndexNumber, Vector3 aPosition)
	{
		if(anIndexNumber>soundObjectList.Count)
 		{
			Debug.LogWarning("BaseSoundController>Trying to do PlaySoundByIndex with invalid index number. Playing last sound in array, instead.");
			anIndexNumber= soundObjectList.Count-1;
 		}
 		tempSoundObj= (SoundObject)soundObjectList[anIndexNumber];
 		tempSoundObj.PlaySound(aPosition);
	}
}