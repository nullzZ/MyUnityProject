using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	private PlayerStateController.playerStates currState = PlayerStateController.playerStates.idle;
	public GameObject playerObj = null;
	public float cameraTackingSpeed = 1f;

	private Vector3 lastTargetPosition = Vector3.zero;
	private Vector3 currTargetPosition = Vector3.zero;
	private float currLerpDistance = 0f;

	// Use this for initialization
	void Start ()
	{
		lastTargetPosition = playerObj.transform.position;
		currTargetPosition = playerObj.transform.position;
	}

	void OnEnable ()
	{
		PlayerStateController.onStateChange += onStateChange;
	}

	void OnDisable ()
	{
		PlayerStateController.onStateChange -= onStateChange;
	}

	// Update is called once per frame
	void Update ()
	{
		onStateCycle ();
		//currLerpDistance += cameraTackingSpeed;
		transform.position = Vector3.Lerp (lastTargetPosition, currTargetPosition, cameraTackingSpeed);
	}

	void onStateChange (PlayerStateController.playerStates newstate)
	{
		currState = newstate;
	}

	void onStateCycle ()
	{
//		switch (currState) {
//		case PlayerStateController.playerStates.idle:
//			trackPlayer ();
//			break;
//		case PlayerStateController.playerStates.left:
//			trackPlayer ();
//			break;
//		case PlayerStateController.playerStates.right:
//			trackPlayer ();
//			break;
//		}
		trackPlayer ();
	}

	void trackPlayer ()
	{
		Vector3 currCamPos = transform.position;
		Vector3 currPlayerPos = playerObj.transform.position;
		lastTargetPosition = currCamPos;
		currTargetPosition = currPlayerPos;
		currTargetPosition.z = currCamPos.z;
			
	}
}
