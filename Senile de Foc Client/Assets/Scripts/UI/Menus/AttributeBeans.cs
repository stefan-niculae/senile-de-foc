using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AttributeBeans : MonoBehaviour 
{
	public Sprite filledSprite;
	public Sprite emptySprite;
	
	public int min = 0;
	public int max = 5;

	public int[] initialValues;
	Transform[] categories;
	[HideInInspector] public int[] values;


	void Awake ()
	{
		categories = new Transform[4];
		categories[0] = Utils.childWithName (transform, "Damage Beans");
		categories[1] = Utils.childWithName (transform, "Fire Rate Beans");
		categories[2] = Utils.childWithName (transform, "Armor Beans");
		categories[3] = Utils.childWithName (transform, "Speed Beans");

		values = new int[4];
	}

	void Start ()
	{
		Reset ();
	}

	public void Reset ()
	{
		for (int i = 0; i < initialValues.Length; i++) {
			values[i] = initialValues[i];
			UpdateBeans (i);
		}
	}

	public void UpdateAll (int[] newValues)
	{
		for (int i = 0; i < newValues.Length; i++) {
			values [i] = newValues [i];
			UpdateBeans (i);
		}
	}

	void UpdateBeans (int index)
	{
		Transform category = categories [index];
		int amount = values [index];


		int i;
		for (i = 0; i < amount; i++) {
			var bean = Utils.childWithName (category, "Bean " + (i + 1)).GetComponent <Image> ();
			bean.sprite = filledSprite;
		}
		for (; i < max; i++) {
			var bean = Utils.childWithName (category, "Bean " + (i + 1)).GetComponent <Image> ();
			bean.sprite = emptySprite;
		}
	}

	public void Increase (int category)
	{
		if (values [category] < max && TankCustomization.remaining > 0) {
			values [category]++;
			UpdateBeans (category);

			TankCustomization.remaining--;
		}
	}

	public void Decrease (int category)
	{
		if (values [category] > min) {
			values [category]--;
			UpdateBeans (category);

			TankCustomization.remaining++;
		}
	}
}
