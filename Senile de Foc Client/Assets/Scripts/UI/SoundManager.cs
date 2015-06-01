using UnityEngine;
using System.Collections;

public class SoundManager : Singleton<SoundManager> 
{
	AudioSource musicSource;
	AudioSource sfxSource;

	void Awake ()
	{
		musicSource = Utils.childWithName (transform, "Music")	.GetComponent <AudioSource> ();
		sfxSource	= Utils.childWithName (transform, "SFX")	.GetComponent <AudioSource> ();
	}

	bool musicEnabled = true;
	public void ToggleMusic ()
	{
		musicEnabled = !musicEnabled;
		musicSource.mute = !musicEnabled;
		UIManager.Instance.SetMusicBan (!musicEnabled);
	}

	bool soundEnabled = true;
	public void ToggleSound ()
	{
		soundEnabled = !soundEnabled;
		sfxSource.mute = !soundEnabled;
		UIManager.Instance.SetSoundBan (!soundEnabled);

		if (soundEnabled != musicEnabled)
			ToggleMusic ();
	}

	public void PlayClip (AudioClip clip)
	{
		sfxSource.PlayOneShot (clip);
	}
}
