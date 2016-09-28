using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;


namespace Manager.Helpers
{
    public class CustomProvider : SimpleMembershipProvider
    {


        //public override System.Web.Security.MembershipUser GetUser(string username, bool userIsOnline)
        //{
        //    return base.GetUser(username, userIsOnline);
        //}


        public override bool ValidateUser(string username, string password)
        {
            Manager.Helpers.LoginUser da = new LoginUser();
            Models.Administrador tmp = da.ValidateUserAdmin(username, password);
            bool x = tmp == null ? false : true;
            //bool x =Convert.ToBoolean(da.ValidateUser(username,password,"").status);
            return x;
        }
    }
}