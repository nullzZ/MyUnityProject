using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Animator))]
public class PlayerStateListener : MonoBehaviour
{
	public GameObject palyerRespawnPoint = null;
	public GameObject bulletObj = null;//子弹

	public Transform bulletSpawnTransform;
	//发射子弹位置
	//重生
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
		case PlayerStateController.playerStates.up:
			playerAnimator.SetInteger ("Dir", 1);
			break;
		case PlayerStateController.playerStates.down:
			playerAnimator.SetInteger ("Dir", 2);
			break;
		case PlayerStateController.playerStates.right:
			playerAnimator.SetInteger ("Dir", 4);
			break;
		case PlayerStateController.playerStates.left:
			playerAnimator.SetInteger ("Dir", 3);
			break;
		case PlayerStateController.playerStates.dead:
			playerAnimator.SetInteger ("Dir", 0);
			break;
		case PlayerStateController.playerStates.resurrect:
			transform.position = palyerRespawnPoint.transform.transform.position;
			transform.rotation = Quaternion.identity;
			playerAnimator.SetInteger ("Dir", 0);
			break;

		case PlayerStateController.playerStates.fire:
			GameObject newBullet = Instantiate (bulletObj);
			newBullet.transform.position = bulletSpawnTransform.position;
			PlayerBulletController bullCon = newBullet.GetComponent<PlayerBulletController> ();
			bullCon.playerObj = gameObject;
			bullCon.launchBullet (newState);
			break;
		
		}
		currState = newState;

	}

	void onStateCycle ()
	{
		//Vector2 localScale = transform.localScale;
		switch (currState) {
		case PlayerStateController.playerStates.idle:
			//Debug.Log ("idle");
			break;
		case PlayerStateController.playerStates.up:
			transform.Translate (new Vector2 (0, (walkSpeed * 1.0f) * Time.deltaTime));
			break;
		case PlayerStateController.playerStates.down:
			transform.Translate (new Vector2 (0, (walkSpeed * -1.0f) * Time.deltaTime));
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
		case PlayerStateController.playerStates.dead:
			onStateChange (PlayerStateController.playerStates.resurrect);
			break;

		case PlayerStateController.playerStates.resurrect:
			onStateChange (PlayerStateController.playerStates.idle);
			break;
		case PlayerStateController.playerStates.fire:
			//onStateChange (PlayerStateController.playerStates.idle);
			break;
		}
	}

	bool checkForValidStatePair (PlayerStateController.playerStates newState)
	{
		bool returnVal = false;
		switch (currState) {
		case PlayerStateController.playerStates.dead:
			if (newState == PlayerStateController.playerStates.resurrect) {
				return true;
			} else {
				return false;
			}
			break;
		case PlayerStateController.playerStates.resurrect:
			if (newState == PlayerStateController.playerStates.idle) {
				return true;
			} else {
				return false;
			}
			break;
		case PlayerStateController.playerStates.idle:
			returnVal = true;
			break;
		case PlayerStateController.playerStates.up:
			returnVal = true;
			break;
		case PlayerStateController.playerStates.down:
			returnVal = true;
			break;
		case PlayerStateController.playerStates.left:
			returnVal = true;
			break;
		case PlayerStateController.playerStates.right:
			returnVal = true;
			break;
		case PlayerStateController.playerStates.fire:
			returnVal = true;
			break;
		}
		return returnVal;
	}

	void hitDeathTrigger ()
	{
		onStateChange (PlayerStateController.playerStates.dead);

	}
}
