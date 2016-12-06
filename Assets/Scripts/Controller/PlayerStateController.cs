using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
	private float fireTime = 0;
	public float fireInterval = 0.3f;
	// Use this for initialization
	void Start ()
	{
		
	}

	public enum playerStates
	{
		idle = 0,
		up,
		down,
		left,
		right,
		dead,
		resurrect,
		fire,
		//重生
	}

	public delegate void playerStateHandler (PlayerStateController.playerStates newState);

	public static event playerStateHandler onStateChange;

	void Update ()
	{
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");

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
		} else if (vertical != 0.0f) {
			if (vertical < 0.0f) {
				if (onStateChange != null) {
					onStateChange (PlayerStateController.playerStates.down);
				}
			} else {
				if (onStateChange != null) {
					onStateChange (PlayerStateController.playerStates.up);
				}
			}
		} else {
			if (onStateChange != null) {
				onStateChange (PlayerStateController.playerStates.idle);
			}
		}

		float firing = Input.GetAxis ("Fire1");
		if (firing > 0 && fireTime < Time.time) {
			if (onStateChange != null) {
				onStateChange (PlayerStateController.playerStates.fire);
			}
			fireTime = Time.time + fireInterval;
		}

	}
}
