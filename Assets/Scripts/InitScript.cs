using UnityEngine;
using System.Collections;
using System.IO;
using Google.Protobuf;
using MY_NET;
using Action;
using MYUDP;

public class InitScript : MonoBehaviour
{

	private float heratTime;
	public  bool isConnected = false;

	public  MySocket socket;
	public  MyUDP myUdp;


	// Use this for initialization
	void Start ()
	{
		ActionManager.Instance.init ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (heratTime >= 10) {
			heratTime = 0;
			heart ();
		}
		heratTime += Time.deltaTime;
	}



	void OnGUI ()
	{
		//开始按钮  
		if (GUI.Button (new Rect (0, 10, 50, 30), "连接")) {
			socket = new MySocket ();
			bool ret = socket.onConnet ("127.0.0.1", 41000);
			if (ret) {
				isConnected = true;
			}
		}
		if (GUI.Button (new Rect (0, 50, 50, 30), "登录")) {
			if (!isConnected) {
				Debug.Log ("socket未连接");
				return;
			}
			login ();

		}
		if (GUI.Button (new Rect (0, 90, 50, 30), "准备")) {
			if (!UserState.isLogin) {
				Debug.Log ("未登录");
				return;
			}

			readState ();
			myUdp = new MyUDP ();
		}


	}



	public void login ()
	{

		LoginRequest req = new LoginRequest ();
		req.SessionId = RoleScript.session;
		req.UdpPort = 60000;

		Debug.Log (string.Format ("The Msg is ( SessionId:{0},UdpPort:{1} )", req.SessionId, req.UdpPort));

		using (MemoryStream ms = new MemoryStream ()) {//相当于try cath
			BinaryWriter bs = new BinaryWriter (ms);
			byte[] msg = req.ToByteArray ();
			short l = (short)(2 + msg.Length);
			short len = System.Net.IPAddress.HostToNetworkOrder (l);
			short cmd = System.Net.IPAddress.HostToNetworkOrder (CMD.LOGIN);
			bs.Write (len);
			bs.Write (cmd);
			bs.Write (msg);
			socket.SendMessage (ms.ToArray ());
		}

	}

	public void SocketRecallSuccess ()
	{
		
	}



	//准备状态
	public void readState ()
	{
		if (UserState.readyState == 100) {
			return;
		}
		UserState.readyState = 100;
		ReadyStateRequest req = new ReadyStateRequest ();
		req.ReadyState = UserState.readyState;
		socket.send (CMD.READY, req.ToByteArray ());
		Debug.Log ("准备--------");
	}



	//心跳
	public void heart ()
	{
		if (isConnected) {
			using (MemoryStream ms = new MemoryStream ()) {//相当于try cath
				BinaryWriter bs = new BinaryWriter (ms);
				short l = (short)(2);
				short len = System.Net.IPAddress.HostToNetworkOrder (l);
				short cmd = System.Net.IPAddress.HostToNetworkOrder (CMD.HEART);
				bs.Write (len);
				bs.Write (cmd);
				socket.SendMessage (ms.ToArray ());
				//Debug.Log ("发送心跳");
			}
		}
	}



}
