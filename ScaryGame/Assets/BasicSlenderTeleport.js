var player : Transform;      // the Object the player is controlling
var spawnOrgin : Vector3;     // this will be the bottom right corner of a square we will use as the spawn area
var maximum : Vector3;        // max distance in the x, y, and z direction the enemy can spawn
var spawnRate : float;        // how often the enemy will respawn
var distanceToPlayer : float; // how close the enemy has to be to the player to play music

private var nearPlayer : boolean = false; // use this to stop the teleporting if near the player
private var nextTeleport : float = 0.0f; // will keep track of when we to teleport next

function Start ()
{
    nextTeleport = spawnRate;
}

function Update ()
{
    if (!nearPlayer)     // only teleport if we are not close to the player
    {
        if (Time.time > nextTeleport)   // only teleport if enough time has passed
        {
            transform.position = Vector3( Random.Range(spawnOrgin.x, maximum.x), Random.Range(spawnOrgin.y, maximum.y), Random.Range(spawnOrgin.z, maximum.z) );   // teleport
            nextTeleport += spawnRate;    // update the next time to teleport
        }
    }
    if (Vector3.Distance(transform.position, player.position) <= distanceToPlayer)
    {
         if (audio && audio.clip && !audio.isPlaying)     // play the audio if it isn't playing
              audio.Play();
         nearPlayer = true;
    }
    else
    {
         if (audio)
            audio.Stop();
         nearPlayer = false;
    }
}