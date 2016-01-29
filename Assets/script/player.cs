using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {
	private const float jumpPowerY = 350.0f;	//ジャンプ時のY方向の力
	private const float jumpPowerX = -350.0f;	//ジャンプ時のX方向の力
	private const float jump = 10.0f;
	private const int STOP = 0;		//スタート待ち状態
	private const int MOVE = 1;		//ジャンプ台滑走中
	private const int JUMP = 2;		//ジャンプする瞬間
	private const int GLIDE = 3;	//滑空中
	private const int END = 4;		//着地、終了
	private bool chakuti = false;	//Arduinoに着地判定を送ったか
	private int mode;	//現在の状態
	private int get;	//Arduinoから送られてきた信号
	public GameObject A;	//Arduinoのスクリプトを含むオブジェクト
	const float jumpYoko = 0.5f;	//ジャンプ中の横方向への移動速度
	int count = 0;		//現在用途なし
	bool jumpFlg = false;	//ジャンプしたかどうか？現在用途なし

	public AudioClip SE1;	//ジャンプするときの音再生
	public AudioClip Planing;	//滑走の音再生
	public AudioClip wind;		//風の音再生
	public AudioClip Chakuti;	//着地の音再生
	
	float timer = 0.0f;	//タイマー
	
	AudioSource se1_Clip;	//ジャンプするときの音の音源
	AudioSource Planing_Clip;	//滑走の音源
	AudioSource wind_Clip;		//風の音源
	AudioSource Chakuti_Clip;	//着地オンの音源
	Rigidbody rb;		//プレイヤーのrigidbody操作用
	
	arduino ar;
	particleFunc pa;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();	//滑る物体のrigidbody取得
		mode = STOP;	//状態をスタート待ちに
		A = GameObject.Find ("ARDUINO");	//arduinoのオブジェクト取得
		ar = A.GetComponent<arduino> ();	//arduinoのスクリプトを取得
		pa = GameObject.Find ("particle").GetComponent<particleFunc> ();
		rb.constraints = RigidbodyConstraints.FreezePosition;	//移動しないように凍結

		//ここから　音源の準備
		se1_Clip = gameObject.GetComponent<AudioSource> ();
		se1_Clip.clip = SE1;
		
		Planing_Clip = gameObject.GetComponent<AudioSource> ();
		Planing_Clip.clip = Planing;
		
		Chakuti_Clip = gameObject.GetComponent<AudioSource> ();
		Chakuti_Clip.clip = Chakuti;
		
		wind_Clip = gameObject.GetComponent<AudioSource> ();
		wind_Clip.clip = wind;
		//ここまで

		wind_Clip.Play ();	//風の音再生
		
	}
	
	// Update is called once per frame
	void Update () {
		
		try{
			if(mode < GLIDE)	//滑走より前の状態なら
				get = ar.getMode ();	//arduinoからの値取得
		}catch{	//arduinoのすくイプとを取得できていない
			Debug.Log ("errror Arduino");	//エラー表示	
			ar = GetComponent<arduino> ();	//再度arduinoのスクリプトを取得
			get = mode;	//状態維持
		}
		
		//get = MOVE;
		
		//Debug.Log ("mode = " + mode);
		if (Input.GetKeyDown (KeyCode.S) && get ==0) {	//スタート待ち状態でSキーを押したら
			get = MOVE;	//arduinoからスタート信号を受け取った状態に
		}
		if (Input.GetKeyDown ("space")) {	//スペースキーが押されたら
			get=JUMP;	//arduinoからジャンプの信号を受け取った状態に
			Debug.Log ("jump");
		}
		//Debug.Log ("get=" + get + "move=" + mode);

		switch(get){
		case STOP:
			break;
		case MOVE:	//スタートの信号を受け取ったら
			if(mode == STOP){	//現在の状態がスタート待ち状態なら
				rb.AddForce(-100,0,0);	//初速として少し押し出す
				mode = MOVE;	//状態を滑走中に
				rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;	//変な回転をしないように固定
				Planing_Clip.PlayOneShot (Planing);	//滑走音を再生
			}
			break;
		case JUMP:	//ジャンプする信号を受け取ったら
			if(mode == MOVE){	//今の状態が滑走中ならば
				//if(Input.GetKeyDown(KeyCode.Space)){
				Planing_Clip.Stop();	//滑走音を停止
				wind_Clip.Play ();	//風の音を再生
				se1_Clip.PlayOneShot (SE1);	//ジャンプ音を再生	
				jumpFlg = true;		//ジャンプした
				mode = JUMP;	//状態をジャンプに
				Debug.Log ("JUMP!");
				rb.constraints = RigidbodyConstraints.FreezeRotationZ;
				/*
				if(ar.getData () < 100 )	//受け取ったジャンプの強さが一定以下なら
					rb.AddForce(jumpPowerX,jumpPowerY,0.0f);	//最低の力でジャンプ
				else{
					rb.AddForce(jumpPowerX,jumpPowerY*((float)ar.getData ()/100.0f),0.0f);	//ジャンプの強さを補正する
				}
				*/
				int data = ar.getData ();
				float plus = jump*data;
				float minus = jump*data*-1;
				rb.AddForce(jumpPowerX+minus,jumpPowerY+plus,0.0f);	//ジャンプの強さを補正する
				ar.deleteBuffer();	//arduinoからあの値を消去
				mode = GLIDE;	//現在の状態を滑空中に
				get = GLIDE;	//
				ar.setMode (GLIDE);	//arduinoの状態を滑空中に
			}
			break;
		case GLIDE:	//滑空中の信号を受け取ったら	
			if(mode == GLIDE){	//今の状態が滑空中なら
				//float data = (float)ar.getData();
				float data = ar.getData ();	//arduinoから値を取得
				//Debug.Log ("data + mode = "+data+get);
				
				if(Input.GetKey(KeyCode.A)){	
					data = 0.02f;	//
				}
				else if(Input.GetKey(KeyCode.D)){
					data = -0.02f;
				}
				
				if(data>0){
					//data = 1;
					Debug.Log ("plus");
				}
				else if(data<0){
					//data = -1;
					Debug.Log ("minus");
				}
				else{
					//data = 0;
					Debug.Log ("center");
				}
				transform.Translate(0,0,-jumpYoko*data);
			}
			break;
		default:
			break;
		}
		if (mode == MOVE) {
			timer += Time.deltaTime;
			if( timer > 4.0f ){
				//Debug.Log ("planing");
				timer = 0.0f;
				Planing_Clip.PlayOneShot (Planing);
			}
		}
	}
	private void OnCollisionEnter(Collision collision){
		Debug.Log ("collision name = " + collision.gameObject.name);
		if (collision.gameObject.name == "chakuti" && chakuti == false) {
			Debug.Log("chakuti");
			
			pa.positionSet(transform.position);
			pa.awake();
			chakuti = true;
			rb.constraints = RigidbodyConstraints.FreezeRotationX;
			mode = END;
			ar.sendValue(END);
			ar.setMode(END);
			Chakuti_Clip.PlayOneShot(Chakuti);
		}

	}
	private void OnTriggerEnter(Collider collision){
		Debug.Log ("collider name = " + collision.gameObject.name);
		if (collision.gameObject.name == "jumpTrigger" && mode == MOVE) {
			mode = GLIDE;
			ar.setMode(GLIDE);
			ar.sendValue (GLIDE);
			Planing_Clip.Stop();
			Debug.Log ("miss_Jump");
		}
		if (collision.gameObject.name == "chakutiTrigger1"
		    || collision.gameObject.name == "chakutiTrigger2"
		    || collision.gameObject.name == "chakutiTrigger3") {
			Destroy (GameObject.Find("chakutiTrigger1"));
			Destroy (GameObject.Find("chakutiTrigger2"));
			Destroy (GameObject.Find("chakutiTrigger3"));
			chakuti = true;
			rb.constraints = RigidbodyConstraints.FreezeRotationX;
			mode = END;
			ar.sendValue(END);
			ar.setMode(END);
			Debug.Log ("chakuti trigger");
		}
	}
}
