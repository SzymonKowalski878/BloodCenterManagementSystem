using BloodCenterManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Logics.Interfaces
{
    public interface IResultOfExaminationLogic:ILogic
    {
        Result<ResultOfExaminationModel> AddResults(ResultOfExaminationModel examination);
        Result<ResultOfExaminationModel> UpdateResults(ResultOfExaminationModel examination);
        Result<ResultOfExaminationModel> UpdateBloodExaminationResult(ResultOfExaminationModel examination);
    }
}
