using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class NPCMovement : MonoBehaviour 
{
    enum NPC //FSM for NPCs!
    { Idle, Freeroam, Chasing, RunningAway }

    #region Basic NPC Variables
    //Movement
    Transform _myTransform; //Slenderman
    Rigidbody _myRigidBody;

    public Transform _myTarget; //Poor Player

    public PlayerHealth _playerHealthScript;

    public GameObject[] _randomSpawnPoints;

    //Movement & Rotation of NPC
    public float _moveSpeed = 6.0f;
    public float _turnSpeed = 3.0f;

    Vector3 _desiredVelocity;
    public bool _isGrounded = false;
    public float _rayDistance = 5.0f;

    //Detection Ranges for the NPCs
    public float _minRange = 35.0f;
    public float _maxRange = 35.0f;
    float _minRangeSqr = 0.0f;
    float _maxRangeSqr = 0.0f;

    //Timer to control free roam random movement
    public float _freeRoamTimer = 0.0f;
    public float _freeRoamTimerMax = 5.0f;
    public float _freeRoamTimeMaxRange = 1.5f;
    public float _freeRoamTimeMaxAdjusted = 5.0f;
    Vector3 _calcDirectionVector = Vector3.forward;
    #endregion Basic NPC Variables

    #region Slender
    //Variables used specifically for the Slender Character
    public bool _isSLENDER = true;
    public bool _isNPCChasing = true;
    public bool _isVisible = false;
    public float _offScreenDot = 0.8f;
    float _reduceDistanceAmount = 0.0f;
    public float _increaseSpeedAmount = 0.5f;

    NPC _myState;

    public AudioClip _enemySightedSound;
    bool _hasPlayedSeenSound = false;

    //public GameObject[] _spawnPoints;
    #endregion Slender

    public void ReduceDistance()
    {
        _minRange -= _reduceDistanceAmount;
        _minRangeSqr = _minRange * _minRange;

        _moveSpeed += _increaseSpeedAmount;
    }

    // Use this for initialization
	void Start () 
    {
        _reduceDistanceAmount = (_maxRange - 4.0f) / 7.0f;

        _myTransform = transform;
        _myRigidBody = rigidbody;
        _myRigidBody.freezeRotation = true;

        _freeRoamTimer = 1000.0f;

        if (_isSLENDER)
        {
            //InvokeRepeating("TeleportEnemy", 60.0f, 20.0f); //Actual Game Options
            //InvokeRepeating("TeleportEnemy", 5.0f, 10.0f); //Test Values

            InvokeRepeating("TeleportEnemy", 30.0f, 20.0f);

            _minRange = _maxRange;

            GameObject targetObject = GameObject.Find("Player");

            if (targetObject)
            {
                _myTarget = targetObject.transform;
                //Using Scripts as Components in the Game!
                _playerHealthScript = (PlayerHealth) _myTarget.GetComponent( typeof(PlayerHealth) );
            }
            else
            {
                Debug.Log("No object named Player found!");
            }

            //used to check if player has been added to program!
            if (!_myTarget)
            {
                _myTarget = GameObject.Find("Player").transform;
            }
            
            if(!_playerHealthScript)
            {
                _myTarget.GetComponent("PlayerHealth");
            }
        }

        _minRangeSqr = _minRange * _minRange;
        _maxRangeSqr = _maxRange * _maxRange;
	}

    //Old and ineeficient teleporting algorithm
    void TeleportEnemyOld()
    {//Teleport the Slenderman at random intervals
        Debug.Log("TeleportEnemy() has been called!");

        CheckIfVisible();

        if (!_isVisible)
        {
            float sqrDist = (_myTarget.position - _myTransform.position).sqrMagnitude;

            if (sqrDist > _maxRangeSqr + 25.0f)
            {
                float teleportDistance = _maxRange + 15.0f;
                //Find position left or right of the player target
                int randomDirection = Random.Range(0, 2);

                if (randomDirection == 0)
                {
                    randomDirection = -1;
                }

                //This would randomize the direction Slenderman would appear based on Player's PoV
                Vector3 terrainPosCheck = _myTarget.position +
                    (randomDirection * _myTarget.right * teleportDistance);

                terrainPosCheck.y = 5000.0f;

                RaycastHit hit;

                if (Physics.Raycast(terrainPosCheck, -Vector3.up, out hit, Mathf.Infinity))
                {//safe region for Slenderman to teleport to...
                    if (hit.collider.gameObject.name == "Terrain")
                    {
                        _myTransform.position = hit.point + new Vector3(0.0f, 0.25f, 0.0f);
                    }
                }
            }
        }
    }

    void TeleportEnemy()
    {//Teleport the Slenderman at random intervals
        Debug.Log("TeleportEnemyNew!");

        CheckIfVisible();

        if (!_isVisible)
        {
            //Find position left or right of the player target
            int randomDirection = Random.Range(0, 2);

            if (randomDirection == 0)
            {
                randomDirection = -1;
            }

            //This would randomize the direction Slenderman would appear based on Player's PoV
            Vector3 terrainPosCheck = _myTarget.position + 
                (randomDirection * _myTarget.right * _minRange);
                
            //Ensure nothing is below the slenderman as he teleports closer and closer to player.
            terrainPosCheck.y = 5000.0f;

            //raycast to check if position on the terrain is free/open
            RaycastHit hit; 

            if (Physics.Raycast( terrainPosCheck, -Vector3.up, out hit, Mathf.Infinity))
            {//safe region for Slenderman to teleport to...
                if (hit.collider.gameObject.name == "Terrain")
                {
                    _myTransform.position = hit.point + new Vector3(0.0f, 0.25f, 0.0f);
                }
            }
        }
    }

    void CheckIfVisible()
    {//Check if Visisble from the perspective of the player!
        Vector3 fwd = _myTarget.forward;
        Vector3 other = (_myTransform.position - _myTarget.position).normalized;

        float dotProduct = Vector3.Dot(fwd, other);

        _isVisible = false;

        if ( dotProduct > _offScreenDot)
        {
            _isVisible = true;
        }
    }

    void DecideSlenderBehavior()
    {
        CheckIfVisible();

        float sqrDist = (_myTarget.position - _myTransform.position).sqrMagnitude;

        if (_isVisible) //if slender is visisble...
        {
            //CHECK THE RANGE
            if (sqrDist > _maxRangeSqr)
            {//If the player is too far away...
                _myState = NPC.Chasing;
                _playerHealthScript.IncreaseHealth();
            }
            else
            {
                RaycastHit hit;

                if (Physics.Linecast(_myTransform.position, _myTarget.position, out hit))
                {
                    Debug.DrawLine(_myTransform.position, _myTarget.position, Color.green);

                    if (hit.collider.gameObject.name == _myTarget.name)
                    {
                        _myState = NPC.Idle;

                        //Decrease Player Health!
                        _playerHealthScript.DecreaseHealth();

                        //Add Slender Sound Effect
                        if (!_hasPlayedSeenSound)
                        {
                            AudioSource.PlayClipAtPoint(_enemySightedSound, _myTarget.position);
                            _hasPlayedSeenSound = true;
                        }
                    }
                    else
                    {
                        _myState = NPC.Chasing;
                        _playerHealthScript.IncreaseHealth();
                    }

                }
            }
        }
        else //If he is NOT visible...
        {
            if (sqrDist > _minRangeSqr)
            {
                _myState = NPC.Chasing;
            }
            else
            {
                _myState = NPC.Idle;
            }
            //Increase health as player runs away!
            _playerHealthScript.IncreaseHealth();

            _hasPlayedSeenSound = false;
        }
    }

	// Update is called once per frame
	void Update () 
    {
        if (_isSLENDER) //Checks to see if NPC is slender or not...
        {
            DecideSlenderBehavior();
        }
        else
        {
            DecideNPCBehavior();
        }


        switch (_myState)
        {
            case NPC.Idle:
                //_myTransform.LookAt(_myTarget); //LOOK AT THE PLAYER~~~~
                Vector3 targetLookRot = //Looking Towards Player but upright!
                    new Vector3(_myTarget.position.x, _myTransform.position.y, _myTarget.position.z);

                _myTransform.LookAt(targetLookRot);
                //Only use the gravity y velocity
                _desiredVelocity = new Vector3(0, _myRigidBody.velocity.y, 0);
                break;

            case NPC.Freeroam:
                //Roam around for now if not doing anything!
                _freeRoamTimer += Time.deltaTime;

                if (_freeRoamTimer > _freeRoamTimeMaxAdjusted)
                {
                    _freeRoamTimer = 0.0f;
                    _freeRoamTimeMaxAdjusted =
                        _freeRoamTimerMax + Random.Range(-_freeRoamTimeMaxRange, _freeRoamTimeMaxRange);

                    _calcDirectionVector = Random.onUnitSphere;
                    _calcDirectionVector.y = 0.0f; // _myTransform.forward.y;
                }

                Moving ( _calcDirectionVector );
                break;

            case NPC.Chasing:
                //Move towards the target/playet
                Moving( (_myTarget.position - _myTransform.position).normalized );
                break;

            case NPC.RunningAway:
                //Move away from the target/player
                Moving( (_myTransform.position - _myTarget.position).normalized );
                break;
        }
       
	}

    void DecideNPCBehavior()
    {
        float sqrDist = (_myTarget.position - _myTransform.position).sqrMagnitude;

        if (sqrDist > _maxRangeSqr)
        {//Determine if NPC is chasing player if closeby...
            if (_isNPCChasing)
            {
                _myState = NPC.Chasing;
            }
            else
            {
                _myState = NPC.Freeroam;
            }
        }
        else if (sqrDist < _minRangeSqr)
        {//If the NPX is close to the player...
            if (_isNPCChasing)
            {
                _myState = NPC.Idle;
            }
            else
            {
                _myState = NPC.RunningAway;
            }
        }
        else
        {//If the NPC is in the middle of the ranges...
            if (_isNPCChasing)
            {
                _myState = NPC.Chasing;
            }
            else
            {
                _myState = NPC.RunningAway;
            }
        }
    }

    void FixedUpdate()
    {
        if (_isGrounded)
        {
            _myRigidBody.velocity = _desiredVelocity;
        }
    }

    void Moving(Vector3 lookDirection)
    {
        RaycastHit hit;

        //To ensure that raycast doesn't only work on center of NPC...
        float shoulderMultiplier = 0.75f; 

        Vector3 leftRayPos = _myTransform.position - (_myTransform.right * shoulderMultiplier);
        Vector3 rightRayPos = _myTransform.position + (_myTransform.right * shoulderMultiplier);

        //Raycast checks to see if any obstacles are in front of the NPC!
        //Handles the left side of the NPC
        if (Physics.Raycast(leftRayPos, _myTransform.forward, out hit, _rayDistance))
        {
            //To ensure that the collider doesn't wonk out on raycasting the terrain!
            if (hit.collider.gameObject.name != "Terrain")
            {
                Debug.DrawLine(leftRayPos, hit.point, Color.red);

                lookDirection += hit.normal * 20.0f;
            }
        }
        //Handles the rightside of the NPC
        else if (Physics.Raycast(rightRayPos, _myTransform.forward, out hit, _rayDistance))
        {
            //To ensure that the collider doesn't wonk out on raycasting the terrain!
            if (hit.collider.gameObject.name != "Terrain")
            {
                Debug.DrawLine(rightRayPos, hit.point, Color.red);

                lookDirection += hit.normal * 20.0f;
            }
        }
        else
        {//Purely for DEBUGGING PURPOSES!!!
            Debug.DrawRay(leftRayPos, _myTransform.forward * _rayDistance, Color.yellow);
            Debug.DrawRay(rightRayPos, _myTransform.forward * _rayDistance, Color.yellow);
        }

        //Slenderman avoids the trees~~~~
        if (_myRigidBody.velocity.sqrMagnitude < 1.75f)
        {
            lookDirection += _myTransform.right * 20.0f;
        }
        
        //Alters NPC to look like they are looking towards a target
        Quaternion lookRot = Quaternion.LookRotation(lookDirection);

        _myTransform.rotation = Quaternion.Slerp(_myTransform.rotation, lookRot, _turnSpeed * Time.deltaTime);

        //Movement
        _desiredVelocity = _myTransform.forward * _moveSpeed;
        _desiredVelocity.y = _myRigidBody.velocity.y;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.name == "Terrain" || 
            collision.collider.gameObject.name == "Floor")
        {
            _isGrounded = true;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.collider.gameObject.name == "Terrain" ||
            collision.collider.gameObject.name == "Floor")
        {
            _isGrounded = true;
        }
    }

    //Used to determine if NPC is touching the floor.
    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.name == "Terrain" ||
            collision.collider.gameObject.name == "Floor")
        {
            _isGrounded = false;
        }
    }
}
