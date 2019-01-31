using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour 
{
	public enum Item
	{
		Flashlight,
		Battery
	}
	
	
	
	public Item item;
	
	void OnTriggerEnter()
	{
		if(item == Item.Flashlight)
		{
		Flashlight.HeadlightMount.SetActiveRecursively(false);
		Flashlight.HeadlightMount.active = true;
			
			HUD.HasFlashlight = true;
		}
		else
		
			HUD.BatteryCount++;
		
		Destroy(gameObject);
		}
		
		
		
	}