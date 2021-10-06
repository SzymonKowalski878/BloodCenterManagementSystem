using BloodCenterManagementSystem.Logics.Repositories;
using BloodCenterManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BloodCenterManagementSystem.DataAccess
{
    public class ResultOfExaminationRepository:Repository<ResultOfExaminationModel>,IResultOfExaminationRepository
    {
        public ResultOfExaminationRepository(Lazy<DataContext> dataContext)
            :base(dataContext)
        {

        }

        public override ResultOfExaminationModel GetById(int id)
        {
            return DataContext.ResultsOfExamination.FirstOrDefault(m=>m.DonationId==id);
        }
    }
}
