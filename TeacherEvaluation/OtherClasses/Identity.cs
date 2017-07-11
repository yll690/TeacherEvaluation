using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherEvaluation
{
    public class Identity
    {
        IdentityEnum identityE;
        string username;
        object iObject;

        public IdentityEnum IdentityE { get => identityE; set => identityE = value; }
        public string Username { get => username; set => username = value; }
        public object IObject { get => iObject; set => iObject = value; }

        public Identity(IdentityEnum identity,string username)
        {
            IdentityE = identity;
            Username = username;
        }


    }
}
