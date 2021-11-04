using AutoMapper;
using BloodCenterManagementSystem.Logics;
using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Logics.Users.DataHolders;
using BloodCenterManagementSystem.Models;
using BloodCenterManagementSystem.Web.Controllers.DataHolders;
using BloodCenterManagementSystem.Web.DTO;
using BloodCenterManagementSystem.Web.DTO.BloodDonator;
using BloodCenterManagementSystem.Web.DTO.User;
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
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly Lazy<IBloodDonatorLogic> _bloodDonatorLogic;
        protected IBloodDonatorLogic BloodDonatorLogic => _bloodDonatorLogic.Value;

        private readonly Lazy<IUserLogic> _userLogic;
        protected IUserLogic UserLogic => _userLogic.Value;

        private readonly Lazy<IMapper> _mapper;
        protected IMapper Mapper => _mapper.Value;

        private readonly Lazy<IEmailConfirmationService> _emailConfirmationService;
        protected IEmailConfirmationService EmailConfirmationService => _emailConfirmationService.Value;

        public UserController(Lazy<IBloodDonatorLogic> bloodDonatorLogic,
            Lazy<IMapper> mapper,
            Lazy<IUserLogic> userLogic,
            Lazy<IEmailConfirmationService> emailConfirmationService)
        {
            _bloodDonatorLogic = bloodDonatorLogic;
            _mapper = mapper;
            _userLogic = userLogic;
            _emailConfirmationService = emailConfirmationService;
        }

        [Authorize(Policy = "Authenticated")]
        [HttpPatch]
        [ProducesResponseType(typeof(ReturnOk), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult Patch(UpdateUserData data)
        {
            if (data == null)
            {
                return BadRequest("UpdateUserData was null");
            }

            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var loggedInUserId = identity.FindFirst("UserId")?.Value;
            var loggedInUserRole = identity.FindFirst("Role")?.Value;
            if (!string.IsNullOrEmpty(loggedInUserRole) && loggedInUserRole == "Donator")
            {
                if (!string.IsNullOrEmpty(loggedInUserId) && loggedInUserId != data.Id.ToString())
                {
                    return BadRequest(Result.Error<BloodDonatorModel>("Wypierdalaj").ErrorMessages);
                }
            }

            var result = BloodDonatorLogic.UpdateUserData(data);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(new ReturnOk { Status = "ok" });
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("worker")]
        [ProducesResponseType(typeof(ReturnOk), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult RegisterWorker(AddWorkerDTO data)
        {
            var userToAdd = Mapper.Map<AddWorkerDTO, UserModel>(data);

            var result = UserLogic.RegisterWokrer(userToAdd,"Worker");

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(new ReturnOk { Status = "ok" });
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("admin")]
        [ProducesResponseType(typeof(ReturnOk), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult RegisterAdmin(AddWorkerDTO data)
        {
            var userToAdd = Mapper.Map<AddWorkerDTO, UserModel>(data);

            var result = UserLogic.RegisterWokrer(userToAdd,"Admin");

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(new ReturnOk { Status = "ok" });
        }

        [Authorize(Policy ="Admin")]
        [HttpDelete("{userid}")]
        [ProducesResponseType(typeof(ReturnOk), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult Delete([FromRoute]int userid)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var loggedInUser = identity.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(loggedInUser) || loggedInUser == userid.ToString())
            {
                return BadRequest("Problem with readiny identity");
            }

            var result = UserLogic.DeleteAccount(userid);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(new ReturnOk { Status = "ok" });
        }

        [Authorize(Policy ="Admin")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ReturnWorkerAccountsDTO>), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult Get()
        {
            var result = UserLogic.ReturnAllWorkers();

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            var listToReturn = new List<ReturnWorkerAccountsDTO>();

            foreach(var x in result.Value)
            {
                listToReturn.Add(Mapper.Map<UserModel, ReturnWorkerAccountsDTO>(x));
            }

            return Ok(listToReturn);
        }

        [Authorize(Policy = "Admin")]
        [HttpGet("{userid}")]
        [ProducesResponseType(typeof(ReturnWorkerAccountsDTO), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult ReturnWorkersAndAdmins([FromRoute]int userid)
        {
            var result = UserLogic.ReturnWorkerById(userid);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            var toReturn = Mapper.Map<UserModel, ReturnWorkerAccountsDTO>(result.Value);

            return Ok(toReturn);
        }
    }
}

