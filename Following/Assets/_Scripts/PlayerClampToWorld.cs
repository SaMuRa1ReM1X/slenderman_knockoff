using UnityEngine;
using System.Collections;

public class PlayerClampToWorld : MonoBehaviour 
{

    public float _worldSizeMinX = 0.0f;
    public float _worldSizeMaxX = 600.0f;
    public float _worldSizeMinZ = 0.0f;
    public float _worldSizeMaxZ = 600.0f;

    public float _edgeOfWorldBuffer = 10.0f;

    Transform _myTransform;

	// Use this for initialization
	void Start () 
    {
        _myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () 
    {
        Vector3 newPos = new Vector3();

        newPos.x += 
            Mathf.Clamp(_myTransform.position.x, _worldSizeMinX + _edgeOfWorldBuffer, _worldSizeMaxX - _edgeOfWorldBuffer);
        newPos.z +=
            Mathf.Clamp(_myTransform.position.z, _worldSizeMinZ + _edgeOfWorldBuffer, _worldSizeMaxZ - _edgeOfWorldBuffer);

        _myTransform.position = newPos;
    }
}
