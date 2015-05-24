using UnityEngine;
using System.Collections;


public class TankStats
{
	public int type;
	
	public int 
		body,
		barrel,
		
		primary,
		secondary,
		
		damage,
		rate,
		armor,
		speed;
	
	
	public TankStats (int type)
	{
		this.type = type;
		
		switch (type)
		{
		case 0:
			body 		= 0;
			barrel		= 0;
			primary 	= 0;
			secondary 	= 0;
			damage 		= 4;
			rate 		= 2;
			armor		= 3;
			speed		= 3;
			break;
		case 1:
			body 		= 1;
			barrel		= 1;
			primary 	= 0;
			secondary 	= 1;
			damage 		= 5;
			rate 		= 1;
			armor		= 4;
			speed		= 2;
			break;
		case 2:
			body 		= 2;
			barrel		= 2;
			primary 	= 1;
			secondary 	= 2;
			damage 		= 2;
			rate 		= 4;
			armor		= 3;
			speed		= 3;
			break;
		case 3:
			body 		= 3;
			barrel		= 3;
			primary 	= 1;
			secondary 	= 3;
			damage 		= 1;
			rate 		= 5;
			armor		= 2;
			speed		= 4;
			break;
		}
	}
}

