using UnityEngine;
using System.Collections;
using System.IO;
using Google.Protobuf;
using MY_NET;
using Action;
using MYUDP;

public class RoleScript : MonoBehaviour
{
   
	// Use this for initializationy
	public static RoleState roleState = RoleState.none;
	public static string session = "1";
	private float speed = 3.0f;

	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void FixedUpdate ()
	{

		if (roleState == RoleState.start_fight) {
			move ();
		}

	}

	public void move ()
	{

		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");
		if (h > 0 || v > 0) {
			Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D> ();
			rigidbody2D.MovePosition (transform.position + new Vector3 (h, 0, v) * speed * Time.deltaTime);



			StateInfoRequest req = new StateInfoRequest ();
			StateInfo info = new StateInfo ();
			info.X = transform.position.x;
			info.Y = transform.position.y;
			req.StateInfo = info;
			//InitScript.Instance.myUdp.send (session, CMD.STATE_INFO, req.ToByteArray ());
			Debug.Log ("当前坐标|x:" + transform.position.x + ",y:" + transform.position.y);
		}

	}




}
