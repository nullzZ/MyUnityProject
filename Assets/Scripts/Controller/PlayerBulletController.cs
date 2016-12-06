using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletController : MonoBehaviour
{

	public GameObject playerObj = null;
	public float bulletSpeed = 15.0f;
	private float selfDestructTimer = 0.0f;
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (selfDestructTimer < Time.time) {
			Destroy (gameObject);
		}
	}

	public void launchBullet (PlayerStateController.playerStates newState)
	{
		Vector2 bulletForce = new Vector2 (bulletSpeed, 0.0f);

		changeDir ();
		switch (newState) {
		case PlayerStateController.playerStates.left:
			bulletForce = new Vector2 (bulletSpeed * -1.0f, 0.0f);
			break;
		case PlayerStateController.playerStates.right:
			bulletForce = new Vector2 (bulletSpeed, 0.0f);
			break;
		}

		GetComponent<Rigidbody2D> ().velocity = bulletForce;//子弹的动力
		selfDestructTimer = Time.time + 1;
	}

	//改变子弹方向
	private void changeDir ()
	{
		Vector2 localScale = transform.localScale;
		localScale.x *= -1;
		transform.localScale = localScale;
	}
}
