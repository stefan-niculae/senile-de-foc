using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RespawnFrame : MonoBehaviour 
{
	Transition bgBotTrans;
	Vector3 bgBotUp;
	Vector3 bgBotDown;

	Transition bgMidTrans;
	Vector3 bgMidSmall 	= new Vector3 (.99f, .1f, 1);
	Vector3 bgMidBig = Vector3.one;

	Transition arrowTrans;
	Vector3 pointingUp = Vector3.zero;
	Vector3 pointingDown = new Vector3 (0, 0, 180);

	GameObject hitters;
	bool isExpanded = true;

	void Awake ()
	{
		// References setup
		bgBotTrans 		= Utils.childWithName (transform, "Background Bot")	.GetComponent <Transition> ();
		bgMidTrans 		= Utils.childWithName (transform, "Background Mid")	.GetComponent <Transition> ();
		arrowTrans 		= Utils.childWithName (transform, "Toggler Arrow")	.GetComponent <Transition> ();
		hitters 		= Utils.childWithName (transform, "Hitters")		.gameObject;

		bgBotDown 	= bgBotTrans.transform.localPosition;
		bgBotUp 	= bgBotDown + new Vector3 (0, 218.1f, 0);
		bgBotTrans	.Initialize (Transition.Properties.position, 	bgBotDown, 	Constants.SMALL_DURATION);
		bgMidTrans	.Initialize (Transition.Properties.scale,		bgMidBig,	Constants.SMALL_DURATION);
		arrowTrans	.Initialize (Transition.Properties.rotation, 	pointingUp, Constants.SMALL_DURATION);
	}

	public void Toggle ()
	{
		if (isExpanded) {
			bgMidTrans.TransitionTo (bgMidSmall);
			bgBotTrans.TransitionTo (bgBotUp);
			arrowTrans.TransitionTo (pointingDown);
		}
		else {
			bgMidTrans.TransitionTo (bgMidBig);
			bgBotTrans.TransitionTo (bgBotDown);
			arrowTrans.TransitionTo (pointingUp);
		}

		isExpanded = !isExpanded;
		hitters.SetActive (isExpanded);
	}

}
