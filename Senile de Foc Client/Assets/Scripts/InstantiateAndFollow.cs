using UnityEngine;
using System.Collections;

public class InstantiateAndFollow : MonoBehaviour 
{
	NetworkView netView;

	Transform body;
	Transform barrel;
	Transform pointer;
	Transform tankContainer;

	bool isFollowing;

	void Awake ()
	{
		netView = GetComponent <NetworkView> ();
		transform.parent = MarkerManager.Instance.transform;
		tankContainer = GameObject.Find ("Tanks").transform;

		pointer = Utils.childWithName (transform, "Pointer Marker");
		isFollowing = false;

		Build ();
	}

	public void Build ()
	{
		var tank = Instantiate (References.Instance.tankPrefab, 
		                        new Vector3 (transform.position.x, transform.position.y, 0),
		                        transform.rotation) as GameObject;
		tank.transform.parent = tankContainer;
		var tankInfo = tank.GetComponent <TankInfo> ();
		var playerInfo = MarkerManager.Instance.PlayerFromSpawnPos (transform.position);

		// We assign each player a spawn position
		// and we use that to uniquely identify them before initialization
		tankInfo.Initialize (playerInfo, 
                             netView.isMine);

		GameServer.Instance.orderNrToTankInfo [playerInfo.orderNumber] = tankInfo;

		var trackManager = Instantiate (References.Instance.trackManagerPrefab) as GameObject;
		tankInfo.movement.tracks = trackManager.GetComponent <TankTracksManager> ();

		body = tank.transform;
		barrel = Utils.childWithName (body, "Barrel");
		
		if (netView.isMine)
			tank.GetComponent <PlayerInput> ().enabled = true;

		isFollowing = true;
	}

	void Update ()
	{
		if (isFollowing) {
			if (netView.isMine) {
				transform	.position = body		.position;
				pointer		.position = barrel		.position;

				transform	.rotation = body		.rotation;
				pointer		.rotation = barrel		.rotation;
			}
			else {
				body		.position = transform	.position;
				barrel		.position = pointer		.position;
				
				body		.rotation = transform	.rotation;
				barrel		.rotation = pointer		.rotation;
			}
		}
	}

}
