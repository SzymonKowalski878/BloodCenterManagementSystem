using BloodCenterManagementSystem.Logics.BloodTypes.DataHolders;
using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Logics.Repositories;
using BloodCenterManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BloodCenterManagementSystem.Logics.BloodTypes
{
    public class BloodTypeLogic:IBloodTypeLogic
    {
        private readonly Lazy<IBloodTypeRepository> _bloodTypeRepository;
        protected IBloodTypeRepository BloodTypeRepository => _bloodTypeRepository.Value;

        public BloodTypeLogic(Lazy<IBloodTypeRepository> bloodTypeRepository)
        {
            _bloodTypeRepository = bloodTypeRepository;
        }

        public Result<IEnumerable<BloodTypeModel>> GetAllBloodTypes()
        {
            var data = BloodTypeRepository.GetAll();

            if (data.Count() != 8)
            {
                return Result.Error<IEnumerable<BloodTypeModel>>("Unable to find all 8 blood types");
            }

            return Result.Ok(data);
        }

        public Result<BloodTypeModel> GetById(int id)
        {
            var data = BloodTypeRepository.GetById(id);

            if(data == null)
            {
                return Result.Error<BloodTypeModel>("Unable to find blood type with given id");
            }

            return Result.Ok(data);
        }

        public Result<BloodTypeModel>GetByName(BloodTypeName bloodTypeName)
        {
            if (string.IsNullOrEmpty(bloodTypeName.BloodType))
            {
                return Result.Error<BloodTypeModel>("Data was null");
            }

            var bloodType = BloodTypeRepository.GetByBloodTypeName(bloodTypeName.BloodType);

            if (bloodType == null)
            {
                return Result.Error<BloodTypeModel>("Unable to find blood type");
            }

            return Result.Ok(bloodType);
        }
    }
}
