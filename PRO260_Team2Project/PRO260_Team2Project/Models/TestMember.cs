using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PRO260_Team2Project.Models
{
    /**
     * This class is a substitute for member while the Database's member class
     * is being made. I needed a member class with some information to make the partial
     * view of the MemberDirectory page thing i'm doing.
     **/
    public class TestMember
    {
        public int MemberID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int AccountBalance { get; set; }
        public bool IsAdmin { get; set; }
    }
}