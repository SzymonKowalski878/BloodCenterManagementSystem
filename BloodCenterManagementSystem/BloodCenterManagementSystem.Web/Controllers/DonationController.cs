using AutoMapper;
using BloodCenterManagementSystem.Logics.Donations.DataHolders;
using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Models;
using BloodCenterManagementSystem.Web.DTO.Donation;
using BloodCenterManagementSystem.Web.DTO.ResultOfExamination;
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

        [HttpPost,Route("ReturnDonatorsAllDonations")]
        public IActionResult ReturnAllDonatorDonations(IdHolder userId)
        {
            var result = DonationLogic.ReturnAllDonatorDonations(userId);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            var list = new List<ReturnDonationDTO>();

            foreach(var x in result.Value)
            {
                list.Add(Mapper.Map<DonationModel, ReturnDonationDTO>(x));
            }

            return Ok(result);
        }

        [HttpPost,Route("ReturnDonationDetails")]
        public IActionResult ReturnDonationDetails(IdHolder donationId)
        {
            var result = DonationLogic.ReturnDonationDetails(donationId);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            //returns 204 if donation exists but there are no details results of examination

            var toReturn = Mapper.Map<ResultOfExaminationModel, ResultOfExaminationDTO>(result.Value.ResultOfExamination);

            return Ok(toReturn);
        }

        [HttpPost,Route("UpdateDonationStage")]
        public IActionResult UpdateDonationStage(UpdateDonationStage data)
        {
            var result = DonationLogic.UpdateDonationStage(data);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(data);
        }

        [HttpPost,Route("ReturnQueue")]
        public IActionResult ReturnQueue(string stage)
        {
            var result = DonationLogic.ReturnQueue(stage);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            var list = new List<ReturnDonationInQueueDTO>();

            foreach (var x in result.Value)
            {
                list.Add(Mapper.Map<DonationModel, ReturnDonationInQueueDTO>(x));
            }

            return Ok(list);
        }
    }
}
