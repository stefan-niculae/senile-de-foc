using UnityEngine;
using System.Collections;

public class BorderSpawner : MonoBehaviour 
{
	public GameWorld world;
	public GameObject borderPrefab;
	public Transform borderContainer;
	public float borderInterval;
	public int borderNumber;

	public void Spawn ()
	{
		// First delete the old border
		Utils.ClearContainer (borderContainer);
		
		// Get the boundaries
		world.BoundariesByEdges ();
		
		// For each edge
		for (int k = 1; k <= 4; k++) {
			for (int i = 0; i < borderNumber; i++) {
				
				Vector3 pos = transform.position;
				
				switch (k) {
					
					// Top border
				case 1: 
					pos.y = GameWorld.maxTop;
					pos.x = GameWorld.maxLeft + borderInterval * i;
					break;
					
					// Bottom
				case 2: 
					pos.y = GameWorld.maxBot;
					pos.x = GameWorld.maxLeft + borderInterval * i;
					break;
					
					// Left
				case 3:
					pos.x = GameWorld.maxLeft;
					pos.y = GameWorld.maxBot + borderInterval * i;
					break;
					
					// Right
				case 4: 
					pos.x = GameWorld.maxRight;
					pos.y = GameWorld.maxBot + borderInterval * i;
					break;
					
				default: 
					break;
				}
				
				
				GameObject spawnedObj = Instantiate (
					borderPrefab,
					pos,
					Utils.random2DRotation ()
					) as GameObject;
				
				spawnedObj.transform.parent = borderContainer;
			}
		}
	}
}
