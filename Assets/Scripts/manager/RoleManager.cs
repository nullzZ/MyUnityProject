using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//其他玩家的管理类
public class RoleManager : MonoBehaviour
{
	public GameObject RolePrefab;
	private Dictionary<ulong, Role> roles = new Dictionary<ulong, Role> ();
	private static readonly RoleManager instance = new RoleManager ();

	private RoleManager ()
	{
	}

	public static RoleManager Instance { 
		get { 
			return instance; 
		} 
	}

	public void create ()
	{ //创建角色图片
		GameObject obj = Instantiate (RolePrefab);
		Debug.Log ("创建Rolepre");
	}

	public void putRole (Role r)
	{
		this.roles.Add (r.id, r);
	}

	public Role getRole (ulong id)
	{
		return roles [id];
	}

}

