using UnityEngine;
using System.Collections;

public class PlayerRunAndFootsteps : MonoBehaviour 
{
    public AudioClip[] _walkSounds;

    public float _walkAudioSpeed = 0.0f;

    private float _walkAudioTimer = 0.0f;

    public bool _isWalking = false;

    CharacterController _chCtrl;

	// Use this for initialization
	void Start () 
    {
        _chCtrl = (CharacterController) GetComponent("CharacterController");
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (_chCtrl.isGrounded)
        {
            PlayFootsteps();
        }
        else
        {
            _walkAudioTimer = 1000.0f;
        }
	}

    void PlayFootsteps()
    {
        if ( Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Vertical") > 0)
        {
            _isWalking = true;
        }
        else
        {
            _isWalking = false;
        }

        if (_isWalking)
        {
            if (_walkAudioTimer > _walkAudioSpeed)
            {
                audio.Stop();
                audio.clip = _walkSounds[Random.Range(0, _walkSounds.Length)];
                audio.Play();
                _walkAudioTimer = 0.0f;
            }
        }
        else
        {
            audio.Stop();
        }

        _walkAudioTimer += Time.deltaTime;
    }
}
