using System;
using System.Collections.Generic;
using Action;
using UnityEngine;

namespace Action
{
	public class ActionManager
	{
		public Dictionary<short,IAction> actions = new Dictionary<short,IAction> ();

		private ActionManager ()
		{
			
		}

		private static readonly ActionManager _instance = new ActionManager ();

		public static ActionManager Instance { 
			get { 
				return _instance; 
			} 
		}

		public void init ()
		{
			actions.Add (CMD.LOGIN, new LoginAction ());

			actions.Add (CMD.START_FIGHT, new StartFightAction ());

			Debug.Log ("加载ACTION-----------");

		}

		public IAction getAction (short cmd)
		{
			return actions [cmd];
		}

		public bool isHasCmd (short cmd)
		{
			return this.actions.ContainsKey (cmd);
		}
	}
}

