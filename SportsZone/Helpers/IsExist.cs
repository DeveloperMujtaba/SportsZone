using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsZone.Helpers
{
    public class IsExist
    {
        private Entities context = new Entities();
        internal bool User(string email)
        {
            var isthere = (from e in context.users where e.email == email select e);
            if (isthere == null) return false;
            else return true;
        }
    }
}