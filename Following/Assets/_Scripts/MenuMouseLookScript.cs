using UnityEngine;
using System.Collections;

public class MenuMouseLookScript : MonoBehaviour 
{
    public float _xSpeed = 1.0f;
    public float _ySpeed = 1.0f;

    public float _xMinLimit = -55.0f;
    public float _xMaxLimit =551.0f;

    public float _yMinLimit = -35.0f;
    public float _yMaxLimit = 25.0f;

    float _rotX = 0.0f;
    float _rotY = 0.0f;
    float _rotZ = 0.0f;

    Transform _cameraTransform;
    GameObject _lastHitObject;

    public Color _selectedColour;
    public Color _deselectedColour;

    public bool _isFading = false;

    public Vector3 _lastMousePos;
    public float _fadeTime;
    private float _timer;
    public Renderer _fadeObject;

	// Use this for initialization
	void Start () 
    {
        _cameraTransform = transform;

        _rotX = _rotY = _rotZ = 0.0f;
        _cameraTransform.localRotation = Quaternion.identity;

        _lastMousePos = Input.mousePosition;

        Screen.showCursor = true;

        if (!_fadeObject)
        {
            _fadeObject = GameObject.Find("FadeObject").renderer;
        }
        _fadeObject.material.color = new Color(0, 0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () 
    {
        RaycastHit hit;

        if (!_isFading)
        {

            if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out hit, 100.0f))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    if (_lastHitObject)
                    {
                        if (_lastHitObject.name != hit.collider.gameObject.name)
                        {
                            _lastHitObject.renderer.material.color = _deselectedColour;

                            _lastHitObject = hit.collider.gameObject;

                            _lastHitObject.renderer.material.color = _selectedColour;
                        }
                    }
                    else
                    {
                        _lastHitObject = hit.collider.gameObject;

                        _lastHitObject.renderer.material.color = _selectedColour;
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("Input Received!");
                        switch (hit.collider.gameObject.name)
                        {
                            case "BtnMainMenu":
                                FadeAndLoadLevel("MainMenu");
                                break;

                            case "BtnOptions":
                                Debug.Log("Options Not Implemented!");
                                //Application.LoadLevel("OptionsMenu");
                                //FadeAndLoadLevel("OptionsMenu");
                                break; 

                            case "BtnPlay":
                                Debug.Log("Play Button hit!");
                                //int randomLvl = 0;//Random.Range(0, 2);
                                //if (randomLvl == 0)
                                FadeAndLoadLevel("SlenderLevel1");
                                //else if (randomLvl == 1)
                                //    Application.LoadLevel("SlenderLevel2");
                                break;

                            case "BtnQuit":
                                Application.Quit();
                                Debug.Log("Quit does not work in Debugger! beware!");
                                break;
                        }
                    }
                }
                else
                {
                    if (_lastHitObject)
                    {
                        _lastHitObject.renderer.material.color = _deselectedColour;
                        _lastHitObject = null;
                    }
                }

            }

            else
            {
                if (_lastHitObject)
                {
                    _lastHitObject.renderer.material.color = _deselectedColour;
                    _lastHitObject = null;
                }
            }
        }
	}

    void FadeAndLoadLevel(string level)
    {
        _isFading = true;
        //Fade Out!
        while (_timer < _fadeTime)
        {
            _timer += Time.deltaTime;

            float fadeAnt = _timer / _fadeTime;

            _fadeObject.material.color = new Color(1.0f - fadeAnt, 1.0f - fadeAnt, 1.0f - fadeAnt, fadeAnt);

        }

        //Then Load the Level!
        Application.LoadLevel(level);
    }

    void LateUpdate()
    {
        _rotY += Input.GetAxis("Mouse X") * _xSpeed;
        _rotX -= Input.GetAxis("Mouse Y") * _xSpeed;

        _rotY = ClampRotation(_rotY, _xMinLimit, _xMaxLimit);
        _rotX = ClampRotation(_rotX, _yMinLimit, _yMaxLimit);

        _cameraTransform.localRotation = Quaternion.Euler(_rotX, _rotY, _rotZ);
    }

    //Limit the rotation of the menu view scene, where the player can 
    //control where they look around the screen.
    float ClampRotation(float rot, float minRot, float maxRot)
    {
        if (rot < -360.0f)
        {
            rot += 360.0f;
        }
        else if (rot > 360.0f)
        {
            rot -= 360.0f;
        }

        rot = Mathf.Clamp(rot, minRot, maxRot);

        return rot;
    }


}
