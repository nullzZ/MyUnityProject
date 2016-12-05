using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Animator))]
public class PlayerStateListener : MonoBehaviour
{
	public float walkSpeed = 3f;
	private Animator playerAnimator = null;
	private PlayerStateController.playerStates currState = PlayerStateController.playerStates.idle;

	void OnEnable ()
	{
		PlayerStateController.onStateChange += onStateChange;//观察者
	}

	void  OnDisable ()
	{
		PlayerStateController.onStateChange -= onStateChange;
	}

	// Use this for initialization
	void Start ()
	{
		playerAnimator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		onStateCycle ();
	}

	void onStateChange (PlayerStateController.playerStates newState)
	{
		if (newState == currState) {
			return;
		} 

		if (!checkForValidStatePair (newState)) {
			Debug.Log ("checkForValidStatePair");
			return;
		}

		switch (newState) {
		case PlayerStateController.playerStates.idle:
			playerAnimator.SetInteger ("Dir", 0);
			break;
		case PlayerStateController.playerStates.right:
			playerAnimator.SetInteger ("Dir", 4);
			break;
		case PlayerStateController.playerStates.left:
			playerAnimator.SetInteger ("Dir", 3);
			break;

		}
		currState = newState;

	}

	void onStateCycle ()
	{
		Vector2 localScale = transform.localScale;
		switch (currState) {
		case PlayerStateController.playerStates.idle:
			//Debug.Log ("idle");
			break;
		case PlayerStateController.playerStates.left:
			transform.Translate (new Vector2 ((walkSpeed * -1.0f) * Time.deltaTime, 0));
			//Debug.Log ("left");
//			if (localScale.x > 0) {
//				localScale.x *= -1.0f;
//				transform.localScale = localScale;
//			}
			break;
		case PlayerStateController.playerStates.right:
			transform.Translate (new Vector2 ((walkSpeed * 1.0f) * Time.deltaTime, 0));
			//Debug.Log ("right");
//			if (localScale.x < 0) {
//				localScale.x *= -1.0f;
//				transform.localScale = localScale;
//			}
			break;
		}
	}

	bool checkForValidStatePair (PlayerStateController.playerStates newState)
	{
		bool returnVal = false;
		switch (currState) {
		case PlayerStateController.playerStates.idle:
			returnVal = true;
			break;
		case PlayerStateController.playerStates.left:
			returnVal = true;
			break;
		case PlayerStateController.playerStates.right:
			returnVal = true;
			break;
		}
		return returnVal;
	}
}
