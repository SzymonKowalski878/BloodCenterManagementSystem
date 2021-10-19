using BloodCenterManagementSystem.Logics;
using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Logics.Users.DataHolders;
using BloodCenterManagementSystem.Models;
using BloodCenterManagementSystem.Web.Controllers.DataHolders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace BloodCenterManagementSystem.Web.Controllers
{
    [ApiController]
    [Route("api/auth")]
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
        public IActionResult Post(UserEmailAndPassword data)
        {
            var result = UserLogic.Login(data);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(result.Value);
        }

        [HttpPost("sendmail")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult SendMail([FromBody] SendMailHolder data)
        {
            if (data.Email == null)
            {
                data.Email = "";
            }

            var code = EmailConfirmationService.GenerateUserConfirmationToken(data.Email);

            if (!code.IsSuccessfull)
            {
                return BadRequest(code.ErrorMessages);
            }

            string link;

            if (!string.IsNullOrEmpty(data.Route))
            {
                link = data.Route + "?userEmail=" + data.Email + "&code" + code.Value.Token;
                link = HttpUtility.UrlEncode(link);
            }
            else
            {
                //link = Url.Action(nameof(VerifyEmail), "Auth", new { userEmail = email, code = code.Value.Token }, Request.Scheme, Request.Host.ToString());
                link = "http://localhost:4200/setpassword" + "?userEmail=" + data.Email + "&code=" + code.Value.Token;
            }

            var messageToSend = new MessageModel(new List<string> { data.Email }, "Blood bank email confirmation", link, null);

            var result = EmailSender.SendEmail(messageToSend);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(new { status = "ok" });
        }

        [HttpPost("verifyemail")]
        [ProducesResponseType(typeof(ReturnOk), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult VerifyEmail([FromBody] VerifyEmailParams data)
        {
            var verificationResult = EmailConfirmationService.ValidateConfirmationToken(data.Code);

            if (!verificationResult.IsSuccessfull)
            {
                return BadRequest(verificationResult.ErrorMessages);
            }

            var result = UserLogic.RegisterAccount(data.UserEmail, data.Code, data.Password);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(new { status = "ok" });
        }

        [HttpPost, Route("regeneratetoken")]
        [ProducesResponseType(typeof(UserToken), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult Post()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var loggedInUser = identity.FindFirst(ClaimTypes.Email)?.Value;

            if (loggedInUser == null)
            {
                return BadRequest(new ErrorMessage()
                {
                    Message = "Unable to extract email from header"
                });
            }

            var result = UserLogic.RenewToken(loggedInUser);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(result.Value);
        }

        [HttpPost("verifycode")]
        [ProducesResponseType(typeof(ReturnOk), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult VerifyCode([FromBody] VerifyCodeParams codeParams)
        {
            var verificationResult = EmailConfirmationService.ValidateConfirmationToken(codeParams.Code);

            if (!verificationResult.IsSuccessfull)
            {
                return BadRequest(verificationResult.ErrorMessages);
            }

            return Ok(new ReturnOk() { Status="ok" });
        }
    }
}
