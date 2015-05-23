using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MirrorImage : MonoBehaviour 
{
	Image self;
	public Image target;

	void Awake ()
	{
		self = GetComponentInChildren <Image> ();
	}

	void Update ()
	{
		if (target != null) {
			self.sprite = target.sprite;
			self.color = target.color;
		}
	}
}
