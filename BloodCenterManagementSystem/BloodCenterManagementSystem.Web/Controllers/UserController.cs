using AutoMapper;
using BloodCenterManagementSystem.Logics;
using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Logics.Users.DataHolders;
using BloodCenterManagementSystem.Models;
using BloodCenterManagementSystem.Web.DTO;
using BloodCenterManagementSystem.Web.DTO.BloodDonator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController:Controller
    {
        private readonly Lazy<IBloodDonatorLogic> _bloodDonatorLogic;
        protected IBloodDonatorLogic BloodDonatorLogic => _bloodDonatorLogic.Value;

        private readonly Lazy<IUserLogic> _userLogic;
        protected IUserLogic UserLogic => _userLogic.Value;

        private readonly Lazy<IMapper> _mapper;
        protected IMapper Mapper => _mapper.Value;

        public UserController(Lazy<IBloodDonatorLogic> bloodDonatorLogic,
            Lazy<IMapper> mapper,
            Lazy<IUserLogic>userLogic)
        {
            _bloodDonatorLogic = bloodDonatorLogic;
            _mapper = mapper;
            _userLogic = userLogic;
        }

        [Authorize(Policy ="Worker")]
        [HttpPost,Route("RegisterDonator")]
        [ProducesResponseType(typeof(ReturnDonatorInformationDTO), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult Post(AddBloodDonatorDTO data)
        {
            var donatorToAdd = Mapper.Map<AddBloodDonatorDTO, BloodDonatorModel>(data);

            var result = BloodDonatorLogic.RegisterBloodDonator(donatorToAdd);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            var donatorToReturn = Mapper.Map<BloodDonatorModel, ReturnDonatorInformationDTO>(result.Value);

            return Ok(donatorToReturn);
        }

        [HttpPost,Route("RegisterAccount")]
        [ProducesResponseType(typeof(UserEmailAndPassword), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult Post(UserEmailAndPassword data)
        {
            var result = UserLogic.RegisterAccount(data);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(data);
        }

        [HttpPost, Route("Login")]
        [ProducesResponseType(typeof(UserToken),200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>),400)]
        public IActionResult Post(UserIdAndPassword data)
        {
            var result = UserLogic.Login(data);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(result.Value);
        }

        [Authorize(Policy = "Authenticated")]
        [HttpPost,Route("UpdateUserdata")]
        [ProducesResponseType(typeof(UpdateUserData), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult Post(UpdateUserData data)
        {
            if(data == null)
            {
                return BadRequest("UpdateUserData was null");
            }

            var result = BloodDonatorLogic.UpdateUserData(data);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(data);
        }

        [Authorize(Policy ="Authenticated")]
        [HttpPost,Route("RegenerateToken")]
        [ProducesResponseType(typeof(UserToken),200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>),400)]
        public IActionResult Post()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var loggedInUser = identity.FindFirst(ClaimTypes.Name)?.Value;

            if (loggedInUser == null)
            {
                return BadRequest("Unable to extract id from header");
            }

            var result = UserLogic.RenewToken(Int32.Parse(loggedInUser));

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(result.Value);
        }
    }
}
