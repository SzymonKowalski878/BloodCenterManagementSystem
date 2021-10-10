using BloodCenterManagementSystem.Logics;
using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Logics.Users.DataHolders;
using BloodCenterManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace BloodCenterManagementSystem.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly Lazy<IUserLogic> _userLogic;
        protected IUserLogic UserLogic => _userLogic.Value;

        private readonly Lazy<IEmailConfirmationService> _emailConfirmationService;
        protected IEmailConfirmationService EmailConfirmationService => _emailConfirmationService.Value;

        private readonly Lazy<IEmailSender> _emailSender;
        protected IEmailSender EmailSender => _emailSender.Value;

        public AuthController(Lazy<IUserLogic> userLogic,
            Lazy<IEmailConfirmationService> emailConfirmationService,
            Lazy<IEmailSender> emailSender)
        {
            _userLogic = userLogic;
            _emailConfirmationService = emailConfirmationService;
            _emailSender = emailSender;
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserToken), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult Post(UserIdAndPassword data)
        {
            var result = UserLogic.Login(data);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(result.Value);
        }

        [HttpPost("/sendmail")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult SendMail([FromQuery] string email,[FromQuery] string route)
        {
            if (email == null)
            {
                email = "";
            }

            var code = EmailConfirmationService.GenerateUserConfirmationToken(email);

            if (!code.IsSuccessfull)
            {
                return BadRequest(code.ErrorMessages);
            }

            string link;

            if (!string.IsNullOrEmpty(route))
            {
                link = route + "?userEmail=" + email + "&code" + code.Value.Token;
                link = HttpUtility.UrlEncode(link);
            }
            else
            {
                link = Url.Action(nameof(VerifyEmail), "Auth", new { userEmail = email, code = code.Value.Token }, Request.Scheme, Request.Host.ToString());

            }

            var messageToSend = new MessageModel(new List<string> { email }, "Blood bank email confirmation", link, null);

            var result = EmailSender.SendEmail(messageToSend);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(link);
        }

        [HttpPost("/verifyemail")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult VerifyEmail(string userEmail, string code, string password)
        {
            var verificationResult = EmailConfirmationService.ValidateConfirmationToken(code);

            if (!verificationResult.IsSuccessfull)
            {
                return BadRequest(verificationResult.ErrorMessages);
            }

            var result = UserLogic.RegisterAccount(userEmail, code, password);

            if (!result.IsSuccessfull)
            {
                BadRequest(result.ErrorMessages);
            }

            return Ok(userEmail);
        }

        [HttpPost, Route("RegenerateToken")]
        [ProducesResponseType(typeof(UserToken), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
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
