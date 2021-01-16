using AutoMapper;
using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Logics.Users.DataHolders;
using BloodCenterManagementSystem.Models;
using BloodCenterManagementSystem.Web.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpPost,Route("RegisterDonator")]
        public IActionResult Post(AddBloodDonatorDTO data)
        {
            var donatorToAdd = Mapper.Map<AddBloodDonatorDTO, BloodDonatorModel>(data);

            var result = BloodDonatorLogic.RegisterBloodDonator(donatorToAdd);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(data);
        }

        [HttpPost,Route("RegisterAccount")]
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
        public IActionResult Post(UserIdAndPassword data)
        {
            var result = UserLogic.Login(data);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(result.Value);
        }

    }
}
