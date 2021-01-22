using AutoMapper;
using BloodCenterManagementSystem.Logics.Donations.DataHolders;
using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Models;
using BloodCenterManagementSystem.Web.DTO.Donation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DonationController:Controller
    {
        private readonly Lazy<IDonationLogic> _donationLogic;
        protected IDonationLogic DonationLogic => _donationLogic.Value;

        private readonly Lazy<IMapper> _mapper;
        protected IMapper Mapper => _mapper.Value;

        public DonationController(Lazy<IDonationLogic> donationLogic,
            Lazy<IMapper> mapper)
        {
            _donationLogic = donationLogic;
            _mapper = mapper;
        }

        [HttpPost,Route("AddDonation")]
        public IActionResult Post(IdHolder data)
        {
            var result = DonationLogic.AddDonation(data);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            var donationToReturn = Mapper.Map<DonationModel, ReturnDonationDTO>(result.Value);

            if (donationToReturn == null)
            {
                return BadRequest("Error during mapping with automapper");
            }

            return Ok(donationToReturn);
        }

    }
}
