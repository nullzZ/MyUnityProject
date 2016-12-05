using System;
using UnityEngine;

namespace Action
{
	public class StartFightAction : IAction
	{

		public void excute (byte[] data)
		{
			StartFightResonpse res = StartFightResonpse.Parser.ParseFrom (data);
			RoleScript.roleState = RoleState.start_fight;
			Debug.Log ("可以开始战斗了---------------");
		}

	}
}

