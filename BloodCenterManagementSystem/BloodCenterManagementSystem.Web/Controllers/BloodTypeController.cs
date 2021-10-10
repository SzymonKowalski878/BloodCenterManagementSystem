using BloodCenterManagementSystem.Logics;
using BloodCenterManagementSystem.Logics.BloodTypes.DataHolders;
using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Models;
using BloodCenterManagementSystem.Web.DTO.BloodType;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BloodTypeController:Controller
    {
        private readonly Lazy<IBloodTypeLogic> _bloodTypeLogic;
        protected IBloodTypeLogic BloodTypeLogic => _bloodTypeLogic.Value;

        public BloodTypeController(Lazy<IBloodTypeLogic> bloodTypeLogic)
        {
            _bloodTypeLogic = bloodTypeLogic;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BloodTypeModel>), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult Get()
        {
            var result = BloodTypeLogic.GetAllBloodTypes();

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BloodTypeModel), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult Get([FromRoute] int id)
        {
            var result = BloodTypeLogic.GetById(id);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(result.Value);
        }
    }
}
