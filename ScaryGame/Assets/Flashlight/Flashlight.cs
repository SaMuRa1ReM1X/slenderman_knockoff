using UnityEngine;
using System.Collections;

public class Flashlight : MonoBehaviour
{
	public static float BatteryLife = 100;
	public static GameObject HeadlightMount;
	
	
	public AudioClip on;
	public AudioClip off;
	
	public Light HeadLight;
	
	
	
	
	public float BatteryReductionSpeed = 3.0f;
	
	private AudioSource audioSource;
	
	
	void Awake()
	{
		audioSource = GameObject.Find("headlamp").GetComponent<AudioSource>();
		HeadlightMount = GameObject.FindWithTag("Headlamp");
		HeadLight.enabled = false;
		
	 	
	    
	}
	
	void Update()
	{
		if (HeadLight.enabled)
		{
			BatteryLife = BatteryLife - (BatteryReductionSpeed * Time.deltaTime);
			
		}
		if (Input.GetKeyDown(KeyCode.F) && HUD.HasFlashlight && !HeadLight.enabled)
		{
			audioSource.clip = on;
		   	audioSource.Play();
			
			if(BatteryLife <= 0 && HUD.BatteryCount > 0)
			{
				HUD.BatteryCount--;
				BatteryLife = 100;
			}
			
			HeadLight.enabled = true;
		}
		else if(Input.GetKeyDown(KeyCode.F) && HUD.HasFlashlight && HeadLight.enabled)
		{
			audioSource.clip = off;
			audioSource.Play();
			
			
			HeadLight.enabled = false;
		}
		
		if(BatteryLife <= 0)
		{
			BatteryLife = 0;
			HeadLight.enabled = false;
			
		}
	}
}