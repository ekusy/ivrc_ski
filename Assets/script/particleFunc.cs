using UnityEngine;
using System.Collections;

public class particleFunc : MonoBehaviour {
	ParticleSystem chakuti;
	// Use this for initialization
	void Start () {
		Debug.Log ("start particleFunc");
		try{
			chakuti = GameObject.Find ("chakutiEffect").GetComponent<ParticleSystem>();
		}catch{
			Debug.Log ("particle");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.P)){
		   awake();
		}
		if(Input.GetKeyDown(KeyCode.O)){
		   stop();
		}
	}

	public void awake(){
		Debug.Log ("play");
		chakuti.Play ();
	}
	public void stop(){
		Debug.Log ("stop");
		chakuti.Stop ();
	}
	public void positionSet(Vector3 _pos){
		_pos.y += 2.0f;
		transform.position = _pos;
	}
}
