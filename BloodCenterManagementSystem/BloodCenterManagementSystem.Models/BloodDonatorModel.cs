using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Models
{
    public class BloodDonatorModel : BaseModel
    {
        public string Pesel { get; set; }
        public string HomeAdress { get; set; }
        public string PhoneNumber { get; set; }
        public int AmmountOfBloodDonated { get; set; }
        public virtual UserModel User { get; set; }
        public virtual int BloodTypeId { get; set; }
        public virtual BloodTypeModel BloodType { get; set; }

    }
}
