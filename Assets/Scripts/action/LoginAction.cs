using System;
using UnityEngine;

namespace Action
{
    public class LoginAction : IAction
    {
        public void excute(byte[] data)
        {
            LoginResponse res = LoginResponse.Parser.ParseFrom(data);
            
            if (res.State == 1)
            {
                ulong myRoleId = res.FightRoleId;
                Role myr = new Role();   
				RoleManager.Instance.create ();
                Debug.Log(myRoleId + "登录成功");
                foreach (FightRoleInfo fri in res.FightRoleInfo)
                {
                    Role r = new Role();
                    r.id = fri.FightRoleId;
                    r.roleName = fri.Name;
					RoleManager.Instance.putRole(r);
                }
				UserState.isLogin = true;
            }
        }
    }
}

