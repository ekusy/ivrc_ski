using UnityEngine;
using System.Collections;

public class cam_move : MonoBehaviour {
	public GameObject obj;
	// Use this for initialization
	void Start () {
		//obj = GameObject.Find("ita");
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 pos;
		Vector3 m_pos;
		Quaternion m_rot;
		m_rot = transform.rotation;
		/*
		pos = transform.position;
		m_pos = obj.transform.position;
		m_pos.y += 0.3f;
		transform.position = m_pos;
		*/

		float sensitivity = 1.5f; // いわゆるマウス感度
		float mouse_move_x = Input.GetAxis("Mouse X") * sensitivity;
		float mouse_move_y = Input.GetAxis("Mouse Y") * sensitivity;
		//Debug.Log (m_pos.x + "," + m_pos.y + "," + m_pos.z);
		m_rot.y += mouse_move_x;
		m_rot.z += mouse_move_y;


		transform.Rotate(-mouse_move_y,mouse_move_x,0);

	}
}
