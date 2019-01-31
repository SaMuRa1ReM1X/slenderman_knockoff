using UnityEngine;
using System.Collections;

[RequireComponent(typeof (AudioSource))]
public class CollectPapers : MonoBehaviour {
    //# of papers collected
    public int _papers = 0;
    //# of papers to beat the game!
    public int _papersToWin = 8;

    //Raycast method, maximum distance that raycast will detect
    public float _distanceToPaper = 5.5f;
    //Width of the sphere that is being SphereCast
    public float _sphereRadius = 1.0f;

    public AudioClip _paperSound;

    public NPCMovement _enemyScript;

    public MusicManager _musicManagerScript;

    public GameObject[] _randomPaperSpawns;

    void Start()
    {
        //audio.loop = true;
        //audio.clip = _musicClips[0];
        //audio.Play();

        if (!_enemyScript)
        {
            Debug.LogWarning("No Enemy Script in the Inspector!");

            GameObject name = GameObject.Find("Enemy");

            _enemyScript = (NPCMovement) name.GetComponent(typeof(NPCMovement));
        }

        if (!_musicManagerScript)
        {
            Debug.LogWarning("No Music Script in the Inspector!");

            GameObject mscmgr = GameObject.Find("MusicManager");

            _musicManagerScript = (MusicManager)mscmgr.GetComponent(typeof(MusicManager));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (/*Input.GetMouseButtonDown(0) ||*/ Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;

            Ray rayOrigin = Camera.main.ScreenPointToRay
                (new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));

            if (Physics.SphereCast(rayOrigin, _sphereRadius, out hit, _distanceToPaper))
            {
                Debug.Log(hit.collider.gameObject.name);

                if (hit.collider.gameObject.tag == "Paper")
                {
                    _papers++;

                    AudioSource.PlayClipAtPoint(_paperSound, hit.point);

                    Destroy(hit.collider.gameObject);

                    //Tell enemy to follow closer
                    _enemyScript.ReduceDistance();

                    //Change Background music based on # of papers collected
                    if (_papers == 2)
                    {
                        _musicManagerScript.PlayMusicTrack(1);
                    }
                    else if (_papers == 4)
                    {
                        _musicManagerScript.PlayMusicTrack(2);
                    }
                    else if (_papers == 6)
                    {
                        _musicManagerScript.PlayMusicTrack(3);
                    }
                    else if (_papers == _papersToWin)
                    {
                        Debug.Log("YOU HAS COLLECTED ALL ZE PAPERS!");
                        Application.LoadLevel("GameWin");
                    }
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.F5)) 
        {
            Application.LoadLevel(Application.loadedLevelName);
        }

    }

    //
}
