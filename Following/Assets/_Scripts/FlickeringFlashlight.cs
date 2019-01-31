using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Light))]
public class FlickeringFlashlight : MonoBehaviour {

    enum FlashLight
    {
        TurnedOff,
        TurnedOn,
        Flickering,
        Fading,
        Resetting
    }

    FlashLight _flashLightState;

    public float _flashLightTimer = 0.0f;
    public float _FlashLightTimerMax = 180.0f;
    public float _flickerRate = 0.1f;

    float _startIntensity = 1.0f;
    public float _maxRandomIntensity = 0.8f;

    public bool _useFlicker = false;

    float _fadeIntensity = 1.0f;
    public float _fadeRate = 1.0f;

    public bool _useBatteries = false;
    public int _batteryCount = 3;
    //SFC for turning flashlight on/off
    public AudioClip _flashLightOnSound;
    public AudioClip _flashLightOffSound;

	// Use this for initialization
	void Start () 
    {
        light.type = LightType.Spot;

        _startIntensity = light.intensity;
        _fadeIntensity = _startIntensity;

        _flashLightState = FlashLight.TurnedOn;
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Check for Keyboard input every frame...
        CheckForInput();
        //Then run the FlashLight State Machine.
        RunFlashFlight();
	}

    void CheckForInput()
    {
        //Default Key Input to Turn Flashflight on is F!
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_flashLightState == FlashLight.TurnedOff && _batteryCount > 0)
            {//Turn on flashlight if light is off...
                _flashLightState = FlashLight.TurnedOn;
                AudioSource.PlayClipAtPoint(_flashLightOnSound, transform.position);
            }
            else if (_flashLightState == FlashLight.TurnedOn || 
                     _flashLightState == FlashLight.Flickering)
            {//What if the player's flashlight is already on? or if it's Flickering?
                _flashLightState = FlashLight.TurnedOff;
                AudioSource.PlayClipAtPoint(_flashLightOffSound, transform.position);
            }
            else if (_flashLightState == FlashLight.Fading)
            {
                light.intensity = _startIntensity;
                _fadeIntensity = _startIntensity;
                _flashLightState = FlashLight.TurnedOff;
                AudioSource.PlayClipAtPoint(_flashLightOffSound, transform.position);
            }
        }

        //Key to "Reset" the Flashlight!
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (_useBatteries)
            {
                _batteryCount--;
            }

            _flashLightTimer = 0.0f;
            _flashLightState = FlashLight.Resetting;
        }
        
    }

    void RunFlashFlight()
    {
        switch (_flashLightState)
        {
            case FlashLight.Flickering:
                _flashLightTimer += Time.deltaTime;

                if (_flashLightTimer > _flickerRate )
                {
                    float LightIntensity = Random.Range(0.0f, _maxRandomIntensity);
                    light.intensity = LightIntensity;

                    _flashLightTimer = 0.0f;
                }
                break;

            case FlashLight.Fading:
                _fadeIntensity -= _fadeRate * Time.deltaTime;

                if (_fadeIntensity < 0.0)
                {
                    _fadeIntensity = 0.0f;
                }

                light.intensity = _fadeIntensity;

                break;

            case FlashLight.Resetting:
                _flashLightTimer += Time.deltaTime;

                //RSimulates a Resetting Effect for the Flashlight.
                if (_flashLightTimer > 0.75f)
                {
                    light.intensity = _startIntensity;
                    _fadeIntensity = _startIntensity;
                    _flashLightTimer = 0.0f;
                    _flashLightState = FlashLight.TurnedOn;
                }
                else if (_flashLightTimer > 0.55f)
                {
                    light.intensity = 0.0f;
                }
                else if (_flashLightTimer > 0.35f)
                {
                    light.intensity = _startIntensity;
                }
                else if (_flashLightTimer > 0.15f)
                {
                    light.intensity = 0.0f;
                }

                break;

            case FlashLight.TurnedOff:
                //Turn off the Light!
                light.enabled = false;
                break;

            case FlashLight.TurnedOn:
                //Turn on the Light
                light.enabled = true;
                //Add on to the current timer!
                _flashLightTimer += Time.deltaTime;

                if (_flashLightTimer >= _FlashLightTimerMax)
                {
                    _flashLightTimer = 0.0f;

                    if (_useFlicker)
                    {
                        _flashLightState = FlashLight.Flickering;
                    }
                    else
                    {
                        _flashLightState = FlashLight.Fading;
                    }
                }
                break;
        }
    }

    void Resetting()
    {

    }
}
