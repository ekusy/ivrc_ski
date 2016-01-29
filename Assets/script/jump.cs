using UnityEngine;
using System.Collections;

public class jump : MonoBehaviour {
	Rigidbody rb;
	// Use this for initialization
	void Start () {
		/*rb = GetComponent<Rigidbody> ();
		rb.AddForce (-100.0f, 0.0f, 0.0f);
		*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnTriggerEnter(Collider collision){
		if (collision.gameObject.name == "jumpTrigger") {
			Debug.Log ("collision");
			rb.constraints = RigidbodyConstraints.FreezeRotationZ;
		}
		if (collision.gameObject.name == "asobi") {
			Debug.Log ("asobi");
			rb.constraints = RigidbodyConstraints.None;
		}
	}
}
