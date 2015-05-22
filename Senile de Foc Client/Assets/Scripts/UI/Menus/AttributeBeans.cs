using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AttributeBeans : MonoBehaviour 
{
	Transform damageBeans;
	Transform fireRateBeans;
	Transform armorBeans;
	Transform speedBeans;
	Transform[] beans;

	public Sprite filledSprite;
	public int[] rates;

	void Awake ()
	{
		beans = new Transform[4];
		beans[0] = Utils.childWithName (transform, "Damage Beans");
		beans[1] = Utils.childWithName (transform, "Fire Rate Beans");
		beans[2] = Utils.childWithName (transform, "Armor Beans");
		beans[3] = Utils.childWithName (transform, "Speed Beans");
	}

	void Start ()
	{
		for (int i = 0; i < 4; i++)
			FillBeans (beans [i], rates[i]);
	}

	void FillBeans (Transform beans, int number)
	{
		for (int i = 0; i < number; i++) {
			var bean = Utils.childWithName (beans, "Bean " + (i + 1)).GetComponent <Image> ();
			bean.sprite = filledSprite;
		}
	}

}
