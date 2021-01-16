using BloodCenterManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Logics.Repositories
{
    public interface IBloodDonatorRepository:IRepository<BloodDonatorModel>
    {
        BloodDonatorModel ReturnDonatorInfo(int id);
    }
}
