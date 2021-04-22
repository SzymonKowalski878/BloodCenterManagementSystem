using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Models
{
    public class UserModel:BaseModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        public virtual int? BloodDonatorId { get; set; }
        public virtual BloodDonatorModel BloodDonator { get; set; }
    }
}
