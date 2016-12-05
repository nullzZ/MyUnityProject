using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		
	}

	public enum playerStates
	{
		idle = 0,
		left,
		right,

	}

	public delegate void playerStateHandler (PlayerStateController.playerStates newState);

	public static event playerStateHandler onStateChange;

	void Update ()
	{
		float horizontal = Input.GetAxis ("Horizontal");
		if (horizontal != 0.0f) {
			if (horizontal < 0.0f) {
				if (onStateChange != null) {
					onStateChange (PlayerStateController.playerStates.left);
				}
			} else {
				if (onStateChange != null) {
					onStateChange (PlayerStateController.playerStates.right);
				}
			}
		} else {
			if (onStateChange != null) {
				onStateChange (PlayerStateController.playerStates.idle);
			}
		}
	}
}
