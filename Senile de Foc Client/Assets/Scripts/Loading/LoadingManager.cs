using UnityEngine;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
	static string loadkey = "levelToLoad";
	static string toLoad
	{
		get { return PlayerPrefs.GetString (loadkey, ""); }
		set { PlayerPrefs.SetString (loadkey, value); }
	}

	static AsyncOperation loadOp;
	public TextMesh loadText;

	IEnumerator Start ()
	{
		loadOp = Application.LoadLevelAsync (toLoad);
		yield return loadOp;
	}

	public static void StartLoading (string level)
	{
		toLoad = level;
		// Go to the 'loading' scene
		Application.LoadLevel ("Loading");
	}

	void Update ()
	{
		loadText.text = (int)(loadOp.progress * 100) + "%";
	}
}
