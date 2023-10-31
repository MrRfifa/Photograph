using AutoMapper;
using Backend.Dtos.requests;
using Backend.Exceptions;
using Backend.Helper;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;

        //private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository,
                              ITokenRepository tokenRepository
         //  IMapper mapper
         )
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            //_mapper = mapper;
        }

        [HttpPut("{userId}/account/email")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ChangeEmail(int userId, [FromBody] ChangeEmailRequest changeEmailRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var changeEmailResult = await _userRepository.ChangeEmail(userId, changeEmailRequest.NewEmail, changeEmailRequest.CurrentPassword);
            var user = await _userRepository.GetUserById(userId);

            if (user.EmailChangeToken != null)
            {
                string recipientName = user.LastName + " " + user.FirstName;
                string emailTemplate = new EmailTemplate().GetEmailChangeConfirmationTemplate(recipientName, user.EmailChangeToken);

                var sendEmailRequest = new SendEmailRequest
                {
                    To = changeEmailRequest.NewEmail,
                    Subject = "Email verification request",
                    Body = emailTemplate
                };

                if (changeEmailResult)
                {
                    _tokenRepository.SendEmail(sendEmailRequest);
                    return Ok("A verification mail is sent to the new address, the token will expire in 15 minutes");
                }
            }
            else
            {
                return BadRequest("Error when handling token");
            }

            return BadRequest(changeEmailResult);
        }

        [AllowAnonymous]
        [HttpGet("verify")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> VerifyEmailChange(string token)
        {
            var user = await _userRepository.GetUserByEmailChangeToken(token);
            if (user == null || user.EmailChangeTokenExpires < DateTime.Now)
            {
                return BadRequest("Invalid token.");
            }

            user.Email = user.NewEmail;
            user.NewEmail = string.Empty;
            user.EmailChangeToken = null;
            user.EmailChangeTokenExpires = null;
            await _userRepository.Save();

            return Ok("Email change successfully verified.");
        }



        [HttpPut("{userId}/account/password")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ChangePassword(int userId, [FromBody] ChangePasswordRequest changePasswordRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var changePasswordResult = await _userRepository.ChangePassword(userId, changePasswordRequest.CurrentPassword, changePasswordRequest.Password, changePasswordRequest.ConfirmPassword);

            if (changePasswordResult)
            {
                return NoContent();
            }

            return BadRequest(changePasswordResult);
        }

        [HttpPut("{userId}/account/names")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ChangeNames(int userId, [FromBody] ChangeNamesRequest changeNamesRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var changeNamesResult = await _userRepository.ChangeNames(userId, changeNamesRequest.NewFirstname, changeNamesRequest.NewLastname, changeNamesRequest.CurrentPassword);

            if (changeNamesResult)
            {
                return NoContent();
            }

            return BadRequest(changeNamesResult);
        }


        [HttpDelete("{userId}/account/delete")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> DeleteAccount(int userId, [FromBody] string currentPassword)
        {
            try
            {
                bool deleted = await _userRepository.DeleteAccount(userId, currentPassword);
                if (deleted)
                {
                    return NoContent(); // 204 No Content
                }
                return NotFound(); // 404 Not Found
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("info")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public IActionResult GetUserInfo()
        {
            var user = HttpContext.User;

            if (user.Identity.IsAuthenticated)
            {
                var claims = user.Claims.Select(c => new { Type = c.Type, Value = c.Value }).ToList();
                return Ok(
                     claims
                );
            }

            return Ok("null");
        }
    }
}