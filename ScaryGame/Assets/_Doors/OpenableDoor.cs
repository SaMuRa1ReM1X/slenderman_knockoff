using UnityEngine;
using System.Collections;

public class OpenableDoor : MonoBehaviour
{	
	public Key requiredKey;
	public AudioClip lockedSound;
	public AudioClip unlockSound;
	public AudioClip slamSound;
	
	private bool _wasMoving = false;
	private Quaternion _startAngle;
	
	// Use this for initialization
	void Awake()
	{
		if (requiredKey != null)
		{
			rigidbody.isKinematic = true;
		}
		
		_startAngle = transform.rotation;
	}
	
	
	void Update()
	{
		float moveSpeed = rigidbody.angularVelocity.magnitude;
		
		if (moveSpeed > 0.1f)
		{
			_wasMoving = true;
		}
		
		if (!audio.isPlaying)
		{
			if (moveSpeed > 0.1f)
			{
				audio.Play();
			}
		}
		else
		{
			if (moveSpeed < 0.1f)
			{
				audio.Stop();
				if (_wasMoving && Quaternion.Angle(transform.rotation, _startAngle) < 5f)
				{
					audio.PlayOneShot(slamSound);
				}
			}
		}
	}
	
	
	void OnBeingDragged()
	{
		if (rigidbody.isKinematic)
		{
			Keyring playerKeyring = FindObjectOfType(typeof(Keyring)) as Keyring;
			
			if (playerKeyring.HasKey(requiredKey))
			{
				rigidbody.isKinematic = false;
				
				if (unlockSound != null)
				{
					audio.PlayOneShot(unlockSound);
				}
			}
			else
			{
				if (lockedSound != null)
				{
					if (!audio.isPlaying)
					{
						audio.PlayOneShot(lockedSound);
					}
				}
			}
		}
	}
}
