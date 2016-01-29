using UnityEngine;
using UnityEngine.VR;
using System.Collections;

public class testVRFunction : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.A)) {
			Debug.Log ("reset");
			InputTracking.Recenter ();
		}
	}
}
