using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (BorderSpawner))]
class BorderSpawnerMenu : Editor 
{
	public override void OnInspectorGUI () 
	{
		DrawDefaultInspector ();
		
		BorderSpawner spawner = (BorderSpawner)target;
		
		if (GUILayout.Button ("Spawn Border"))
			spawner.Spawn ();
	}
}

[CustomEditor (typeof(EnvironmentItemSpawner))]
class ItemSpawnerMenu : Editor
{
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();

		EnvironmentItemSpawner spawner = (EnvironmentItemSpawner)target;

		if (GUILayout.Button ("Spawn Items"))
			spawner.SpawnItems ();
	}
}