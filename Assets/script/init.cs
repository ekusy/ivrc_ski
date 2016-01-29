using UnityEngine;
using System.Collections;

public class init : MonoBehaviour {
	Vector3 POS;	//スタート位置
	Vector3 m_pos;	//初期位置
	Vector3 speed1;	//ウインチで引き上げられるスピード
	Vector3 speed2;	//スタート位置へ横移動するスピード
	arduino ar;

	float time1 = 2.0f;	//ウインチの稼働時間予定
	float time2 = 2.0f;	//登り切ってからスタート位置へ移動する時間
	float timer = 0;	//経過時間
	bool setup = false;	//移動を始めるかどうか
	bool start = false;	//スタート位置にいるかどうか

	//ウインチで引き上げられる前の初期位置
	float posX = -24.0f;	//初期位置
	float posY = 0.0f;		//初期位置
	float posZ = 0.0f;	//初期位置

	// Use this for initialization
	void Start () {
		ar = GameObject.Find ("ARDUINO").GetComponent<arduino> ();

		POS = transform.localPosition;	//スタート位置保存
		m_pos = POS;		//
		m_pos.x += posX;	//移動
		m_pos.z += posY;	//移動
		m_pos.y += posZ;	//移動
		transform.localPosition = m_pos;	//移動を反映
		speed1 = new Vector3 (4.0f,0.0f,0.0f);	//ウインチでの移動速度を決定
		Debug.Log ("speed1="+speed1);
		speed2 = new Vector3 (0.0f, 0.0f, 2.5f);	//登ってからスタート位置への移動速度を決定
		Debug.Log ("speed2="+speed2);

		ar.setMode (-1);
		
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("");
		if(Input.GetKeyDown(KeyCode.S)){	//Sキーを押したら
			Debug.Log ("setup");
			setup = true;	//移動開始フラグオン
			ar.sendValue(0);	//arduinoにウインチ起動信号を送信
			//ar.setMode(0);
		}
		if (ar.getMode () == -1 && setup == true) {	//準備中かつ移動開始フラグオン
			Debug.Log ("wait");
			m_pos+=speed1*Time.deltaTime;
			transform.localPosition = m_pos;	//移動を反映
		}
		if(ar.getMode() == 0 || Input.GetKeyDown(KeyCode.D) ){
			ar.setMode(0);	//arduinoのモードをスタート待ち状態へ
			//ar.sendValue(0);
			Debug.Log ("start OK");
			//timer = 0.0f;	//経過時間初期化
		}
	}
}
