using AutoMapper;
using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Models;
using BloodCenterManagementSystem.Web.DTO.BloodDonator;
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
    public class BloodDonatorController:Controller
    {
        private readonly Lazy<IBloodDonatorLogic> _bloodDonatorLogic;
        protected IBloodDonatorLogic BloodDonatorLogic => _bloodDonatorLogic.Value;

        private readonly Lazy<IMapper> _mapper;
        protected IMapper Mapper => _mapper.Value;

        public BloodDonatorController(Lazy<IBloodDonatorLogic> bloodDonatorLogic,
            Lazy<IMapper> mapper)
        {
            _bloodDonatorLogic = bloodDonatorLogic;
            _mapper = mapper;
        }

        [HttpPost, Route("GetDonatorInformation")]
        public IActionResult Post(BloodTypeIdDTO id)
        {
            var bloodDonator = BloodDonatorLogic.ReturnDonatorInformation(id.Id);

            if (!bloodDonator.IsSuccessfull)
            {
                return BadRequest(bloodDonator.ErrorMessages);
            }

            var donatorToReturn = Mapper.Map<BloodDonatorModel, ReturnDonatorInformationDTO>(bloodDonator.Value);
            return Ok(donatorToReturn);
        }
    }
}
