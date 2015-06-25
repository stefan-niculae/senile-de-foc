using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class UIManager : Singleton<UIManager>
{
	Transform musicBan;
	Transform soundBan;


	void Awake ()
	{
		musicBan 	= Utils.childWithName (transform, "Music Ban");
		soundBan 	= Utils.childWithName (transform, "Sound Ban");

		AwakeRferences ();
	}

	public virtual void AwakeRferences ()
	{ }

	// Public because ingamesettings also uses this
	public void SetVisibility (bool visible, params Transform[] elements)
	{
		Array.ForEach (elements,
			elem => {
				var pos = elem.position;
				if (visible) {
					if (pos.y < -Constants.HIDDEN.y / 2)
						pos.y += Constants.HIDDEN.y;
				}
				else {
					if (pos.y > -Constants.HIDDEN.y / 2)
						pos.y -= Constants.HIDDEN.y;
				}
				elem.position = pos;

				OnVisibilityChange (elem, visible);
			});
	}
	public virtual void OnVisibilityChange (Transform elem, bool visible)
	{ }

	public void SetMusicBan (bool value)
	{
		SetVisibility (value, musicBan);
	}
	public void SetSoundBan (bool value)
	{
		SetVisibility (value, soundBan);
	}
}
