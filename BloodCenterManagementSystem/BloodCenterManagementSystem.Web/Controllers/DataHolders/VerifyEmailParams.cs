using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.Controllers.DataHolders
{
    public class VerifyEmailParams
    {
        public string UserEmail { get; set; }
        public string Code { get; set; }
        public string Password { get; set; }
    }
}
