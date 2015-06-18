using UnityEngine;
using System.Collections;

public class InstantiateAndFollow : MonoBehaviour 
{
	NetworkView netView;

	Transform body;
	Transform barrel;
	Transform pointer;
	Transform tankContainer;

	TankTracksManager trackManager;
	TankInfo tankInfo;
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
		tankInfo = tank.GetComponent <TankInfo> ();
		var playerInfo = MarkerManager.Instance.PlayerFromSpawnPos (transform.position);

		// We assign each player a spawn position
		// and we use that to uniquely identify them before initialization
		tankInfo.Initialize (playerInfo, 
                             netView.isMine);

		GameServer.Instance.orderNrToTankInfo [playerInfo.orderNumber] = tankInfo;

		var trackManagerObj = Instantiate (References.Instance.trackManagerPrefab) as GameObject;
		trackManager = tankInfo.movement.tracks = trackManagerObj.GetComponent <TankTracksManager> ();

		body = tank.transform;
		barrel = Utils.childWithName (body, "Barrel");

		isFollowing = true;
	}

	float lastMove;
	const float PERIOD_TO_STOP = 1;
	void Update ()
	{
		if (isFollowing && pointer != null) {
			if (netView.isMine) {
				transform	.position = body		.position;
				pointer		.position = barrel		.position;

				transform	.rotation = body		.rotation;
				pointer		.rotation = barrel		.rotation;
			}
			else {
				if (body.position != transform.position) {
					lastMove = Time.time;
					tankInfo.sounds.tracksVolume = 1;
				} 
				else if (Time.time >= lastMove + PERIOD_TO_STOP)
					tankInfo.sounds.tracksVolume = 0;
				

				body		.position = transform	.position;
				barrel		.position = pointer		.position;
				
				body		.rotation = transform	.rotation;
				barrel		.rotation = pointer		.rotation;

				trackManager.Show (transform.position, transform.rotation);
			}
		}
	}

}
