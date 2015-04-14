using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (WorldItemSpawner))]
class DecalMeshHelperEditor : Editor 
{
	public override void OnInspectorGUI () 
	{
		DrawDefaultInspector();

		WorldItemSpawner spawner = (WorldItemSpawner)target;

		if (GUILayout.Button ("Spawn Border"))
			spawner.SpawnBorder ();

		if (GUILayout.Button ("Spawn Trees"))
			spawner.SpawnTrees ();
	}
}
