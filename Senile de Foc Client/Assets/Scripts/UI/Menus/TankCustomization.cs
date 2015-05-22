using UnityEngine;
using System.Collections;

public class TankCustomization : MonoBehaviour 
{
	public void BackToSelection ()
	{
		SplashMenus.currentStep = SplashMenus.Steps.selection;
	}

	public void Lockin ()
	{
		Server.RegisterCustom (0, 0, 0, 0, 0, 0);
		SplashMenus.currentStep = SplashMenus.Steps.lobby;
	}
}
