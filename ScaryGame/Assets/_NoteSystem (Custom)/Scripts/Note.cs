using UnityEngine;
using System.Collections;

public class Note : MonoBehaviour {
		
	//The Skin/Background of the GUIStyle
	 public GUISkin mycustomSkin;
	//The Text Of The Note
	public string Text = "Insert Your Text Here!";
	
	void Start () {
	
		//AutoSet the Name
		transform.name = "Note";
		
		//If there is no collider on the note add one
		if (collider == null) {
			
			Debug.LogError ("No Collider On Note " + name + ". Add A Collider!");
			
		}
		
	}
		
}
