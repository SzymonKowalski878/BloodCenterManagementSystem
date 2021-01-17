using BloodCenterManagementSystem.Logics.Interfaces;
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

        [HttpGet,Route("ReturnAllBloodTypes")]
        public IActionResult Get()
        {
            var result = BloodTypeLogic.GetAllBloodTypes();

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(result.Value);
        }

        [HttpPost,Route("ReturnBloodTypeById")]
        public IActionResult Post(BloodTypeIdDTO data)
        {
            var result = BloodTypeLogic.GetById(data.Id);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(result);
        }
    }
}
