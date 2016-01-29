using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;
public class arduino : MonoBehaviour {
	SerialPort stream = new SerialPort("COM4", 9600);
	//SerialPort stream;
	//int j=0;
	public const int INIT = -1;
	 const int STOP = 0;
	 const int START = 1;
	 const int JUMP = 2;
	 const int GLIDE = 3;
	 const int END = 4;
	private int mode = INIT;
	private int data = 0;
	private int buff = 0;
	void Start () {
		OpenConnection ();
	}
	void Update () {
		Debug.Log ("serial mode ="+mode);

		//if(false){
		if (stream.IsOpen) {
			try {
				string result = "-";
				
				result = stream.ReadLine ();
				//int buffSize =stream.BytesToRead;
				//Debug.Log (buffSize);
				Debug.Log ("received");
				Debug.Log ("data=" + result);
				
				try {
					buff = int.Parse (result);
					if(mode == INIT && buff == STOP){
						Debug.Log ("winch");
						mode = STOP;
					}
					else if (mode == STOP && buff == 1) {
						mode = START;
						//Debug.Log ("mode="+buff);
					} else if (mode == START) {
						
						mode = buff % 10;
						data = buff / 10;
						Debug.Log ("START mode=" + mode + ",data=" + data);
						
					} else if (mode == GLIDE) {
						//Debug.Log ("recieve = "+result);
						/*
						mode = buff%10;
						if(mode < 0)
							mode *= -1;
						data = buff/10;
						*/
						data = buff;
						//deleteBuffer();
						Debug.Log ("GLIDE mode="+mode+",data="+data);
						if (mode == 7) {
							Debug.Log ("error GLIDE");
						}
					}
				} catch (FormatException) {
					if(mode == INIT){
						mode = STOP;
						Debug.Log ("mode=INIT,error format ");
					}
					Debug.Log ("miss");
				}
				
				
			} catch (TimeoutException) {
				Debug.Log ("timeout");
			}
		} else {
			Debug.Log ("stream close");
		}

	}
	
	void OnApplicationQuit(){
		stream.Close();
	}
	
	void OpenConnection() {
		
		if (stream != null) {
			
			if (stream.IsOpen) {
				stream.Close ();
				Debug.LogError ("Failed to open Serial Port, already open!");
			} else {
				stream.Open ();
				stream.ReadTimeout = 10;
				
				Debug.Log ("Open Serial port");      
			}
		} else {
			String[] ports = System.IO.Ports.SerialPort.GetPortNames();
			 
			for(int i=0;i<ports.Length;i++){
				Debug.Log (ports[i]);
			}

			if(ports.Length==1){
				stream = new SerialPort(ports[0],9600);

				if(stream!=null){
					stream.Open();
					Debug.Log ("success serial open");
				}
				else{
					Debug.Log ("error serial open");
				}
			}
			else{
				Debug.Log ("error get serial port");
			}
		}
	}
	/*
	void OpenConnection(SerialPort _stream) {
		if (_steream == null) {
			String port = 
		}
		if (_stream != null) {
			
			if (stream.IsOpen) {
				stream.Close ();
				//Debug.LogError ("Failed to open Serial Port, already open!");
			} else {
				stream.Open ();
				stream.ReadTimeout = 10;
				
				//Debug.Log ("Open Serial port");      
			}
		}
	}
*/
	public int getMode(){
		//Debug.Log ("getMode()");
		return mode;
	}
	
	public int getData(){
		return data;
	}
	
	public void setMode(int _mode){
		mode = _mode;
	}
	public void sendValue(int _value){
		if (stream.IsOpen) {
			String value = Convert.ToString (_value);
			stream.Write (value);
			//mode = END;
			Debug.Log ("send = "+value);
			//stream.Close ();
		}
	}
	public void deleteBuffer(){
		stream.DiscardInBuffer ();
	}
	/*
	public void sendEnd(){
		if (stream.IsOpen) {
			String end = Convert.ToString (END);
			stream.Write ("4");
			Debug.Log ("END");
			stream.Close ();
		}
	}
	*/
	
	
	
}

