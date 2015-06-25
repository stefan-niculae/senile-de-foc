using UnityEngine;
using System.Collections;

public class SoundManager : Singleton<SoundManager> 
{
	AudioSource musicSource;
	AudioSource sfxSource;
	public AudioClip matchOverSound;

	void Awake ()
	{
		musicSource = Utils.childWithName (transform, "Music")	.GetComponent <AudioSource> ();
		sfxSource	= Utils.childWithName (transform, "SFX")	.GetComponent <AudioSource> ();
	}

	const string MUSIC_KEY = "musicEnabled";
	public bool musicEnabled
	{
		get { return PlayerPrefs.GetInt (MUSIC_KEY, 1) == 1; }
		set
		{
			PlayerPrefs.SetInt (MUSIC_KEY, value ? 1 : 0);
			musicSource.mute = !value;
			UIManager.Instance.SetMusicBan (!value);
		}
	}

	const string SOUND_KEY = "soundEnabled";
	public bool soundEnabled
	{
		get { return PlayerPrefs.GetInt (SOUND_KEY, 1) == 1; }
		set
		{
			PlayerPrefs.SetInt (SOUND_KEY, value ? 1 : 0);
			AudioListener.volume = value ? 1 : 0;
			UIManager.Instance.SetSoundBan (!value);

			if (!soundEnabled && musicEnabled)
				musicEnabled = false;
		}
	}

	void Start ()
	{
//		PlayerPrefs.DeleteKey (SOUND_KEY);
//		PlayerPrefs.DeleteKey (MUSIC_KEY);
		// To restore player pref values
		musicEnabled = musicEnabled;
		soundEnabled = soundEnabled;
	}

	public void PlayClip (AudioClip clip)
	{
		sfxSource.PlayOneShot (clip);
	}
}
