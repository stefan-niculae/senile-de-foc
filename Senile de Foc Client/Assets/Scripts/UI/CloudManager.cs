using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CloudManager : Singleton<CloudManager> 
{
	public GameObject cloudPrefab;
	public Sprite[] sprites;

	const float OVERFLOW_PER = .11f;
	const int CLOUD_COUNT = 7;

	const float SCALE_VARIANCE = .15f;
	const float ROT_VARIANCE = 10;

	const float SPEED_MEAN = 9f;
	const float SPEED_VARIANCE = 6f;


	Vector2 botLeft;
	Vector2 topRight;
	[HideInInspector] public float maxLeft, maxRight;

	void Awake ()
	{
		var bl = Utils.childWithName (transform, "Bot Left") as RectTransform;
		var tr = Utils.childWithName (transform, "Top Right") as RectTransform;
		botLeft  = bl.anchoredPosition;
		topRight = tr.anchoredPosition;
		 
		var diff = topRight.x - botLeft.x;
		maxLeft  = botLeft.x  - diff * OVERFLOW_PER;
		maxRight = topRight.x + diff * OVERFLOW_PER;
	}

	void Start ()
	{
		// Spawning the initial clouds
		for (int i = 0; i < CLOUD_COUNT; i++) {

			var pos = new Vector2 (
				Random.Range (maxLeft, maxRight),
				Random.Range (botLeft.y, topRight.y)
			);

			var scale = new Vector3 (
				1 + Random.Range (-SCALE_VARIANCE, SCALE_VARIANCE),
				1 + Random.Range (-SCALE_VARIANCE, SCALE_VARIANCE),
				1
			);

			var rot = Quaternion.Euler (
				0,
				0,
				Random.Range (-ROT_VARIANCE, ROT_VARIANCE)
			);

			var cloud = Instantiate (cloudPrefab, pos, rot) as GameObject;
			cloud.transform.localScale = scale;
			cloud.transform.SetParent (transform, false);

			cloud.GetComponent <Image> ().sprite = Utils.randomFrom (sprites);
			cloud.GetComponent <Cloud> ().speed = SPEED_MEAN + Random.Range (-SPEED_VARIANCE, SPEED_VARIANCE);
		}
	}
}
