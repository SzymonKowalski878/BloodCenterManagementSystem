using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.DTO.User
{
    public class ReturnWorkerAccountsDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
    }
}
