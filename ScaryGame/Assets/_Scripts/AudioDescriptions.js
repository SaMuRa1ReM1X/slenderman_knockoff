var range: float = 5; // define the detection distance
private var hit: RaycastHit;

function Update(){
  if (Input.GetKeyDown("e")){
    var ray = Camera.main.ViewportPointToRay(Vector3(0.5,0.5,0));
    if (Physics.Raycast(ray, hit, range)){
      // check if the object hit has SoundScript.js:
      var ss: SoundScript = hit.transform.GetComponent(SoundScript);
      if (ss && !audio.isPlaying){ // if so, play its sound:
        audio.clip = ss.soundFX;
        audio.Play();
      
      }
    }
  }
}