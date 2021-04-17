using AutoMapper;
using BloodCenterManagementSystem.Logics;
using BloodCenterManagementSystem.Logics.BlodStorage.DataHolders;
using BloodCenterManagementSystem.Logics.Donations.DataHolders;
using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Models;
using BloodCenterManagementSystem.Web.DTO.BloodStorage;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BloodStorageController:Controller
    {
        private readonly Lazy<IBloodStorageLogic> _bloodStorageLogic;
        protected IBloodStorageLogic BloodStorageLogic => _bloodStorageLogic.Value;

        private readonly Lazy<IMapper> _mapper;
        protected IMapper Mapper => _mapper.Value;

        public BloodStorageController(Lazy<IBloodStorageLogic> bloodStorageLogic,
            Lazy<IMapper> mapper)
        {
            _bloodStorageLogic = bloodStorageLogic;
            _mapper = mapper;
        }

        [HttpPost,Route("AddBloodUnitToStorage")]
        [ProducesResponseType(typeof(ReturnAddedUnitDTO), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult AddBloodUnitToStorage(AddBloodUnitToStorage data)
        {
            var result = BloodStorageLogic.AddBloodUnit(data);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            var toReturn = Mapper.Map<BloodStorageModel, ReturnAddedUnitDTO>(result.Value);

            return Ok(toReturn);
        }

        [HttpPost,Route("AddForeignBloodUnitToStorage")]
        [ProducesResponseType(typeof(ReturnAddedUnitDTO), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult AddForeingBloodUnitToStorage(AddForeignBloodUnitToStorage data)
        {
            var result = BloodStorageLogic.AddForeignBloodUnit(data);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            var toReturn = Mapper.Map<BloodStorageModel, ReturnAddedUnitDTO>(result.Value);

            return Ok(toReturn);
        }

        [HttpGet,Route("ReturnAllAvailableBloodUnits")]
        [ProducesResponseType(typeof(List<ReturnAddedUnitDTO>), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult ReturnAllAvailableBloodUnits()
        {
            var result = BloodStorageLogic.ReturnAllAvailableBloodUnits();

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            var list = new List<ReturnAddedUnitDTO>();

            foreach(var x in result.Value)
            {
                list.Add(Mapper.Map<BloodStorageModel, ReturnAddedUnitDTO>(x));
            }

            return Ok(list);
        }

        [HttpPost,Route("ChangeBloodUnitToUnavailable")]
        [ProducesResponseType(typeof(ReturnAddedUnitDTO), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult ChangeBloodUnitToUnavailable(IdHolder bloodUnitId)
        {
            var result = BloodStorageLogic.ChangeBloodUnitToUnavailable(bloodUnitId);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            var toReturn = Mapper.Map<BloodStorageModel, ReturnAddedUnitDTO>(result.Value);

            return Ok(toReturn);
        }
    }
}
