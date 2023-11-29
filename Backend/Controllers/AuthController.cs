using AutoMapper;
using Backend.Dtos;
using Backend.Dtos.requests;
using Backend.Exceptions;
using Backend.Helper;
using Backend.Interfaces;
using Backend.Models.classes;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthRepository _authRepository;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(IUserRepository userRepository, IAuthRepository authRepository, ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
            _userRepository = userRepository;
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto userCreated)
        {
            try
            {
                if (userCreated == null)
                {
                    ModelState.AddModelError("", "Invalid user data.");
                    return BadRequest(ModelState);
                }

                User existingUser = new User();
                try
                {
                    existingUser = await _userRepository.GetUserByEmail(userCreated.Email);
                }
                catch (UserNotFoundException)
                {
                    // Handle the UserNotFoundException here if needed
                }

                if (existingUser.Id != 0) // Assuming User has an 'Id' property that is not 0 for an empty user
                {
                    ModelState.AddModelError("", "This email already exists");
                    return StatusCode(422, ModelState);
                }

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!await _authRepository.Register(userCreated))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }

                User registredUser = await _userRepository.GetUserByEmail(userCreated.Email);
                if (registredUser.VerificationToken is not null)
                {
                    string recipientName = userCreated.LastName + " " + userCreated.FirstName;
                    var sendEmailRequest = new SendEmailRequest
                    {
                        To = userCreated.Email,
                        Subject = "Email verification request",
                        Body = new EmailTemplate().GetEmailConfirmationTemplate(recipientName, registredUser.VerificationToken)
                    };

                    _tokenRepository.SendEmail(sendEmailRequest);

                    return Ok(new { status = "success", message = "Successfully Created." });
                }
                else
                {
                    throw new Exception("Token not generated yet, retry in few minutes.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong: " + ex.Message);
            }
        }


        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login([FromBody] LoginUserDto userLogged)
        {
            try
            {
                if (userLogged == null)
                {
                    return BadRequest("Invalid request. Please provide valid login credentials.");
                }

                var token = await _authRepository.Login(userLogged);

                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest("Authentication failed. Please check your credentials.");
                }

                return Ok(new { message = "success", Token = token });
            }
            catch (UserNotFoundException)
            {
                return BadRequest("User not found. Please check your credentials.");
            }
            catch (Exception ex)
            {
                // Log the exception for internal investigation, but provide a generic error message to the client
                return StatusCode(400, ex.Message);
            }
        }



        [HttpGet("verify")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Verify(string token)
        {
            var user = await _authRepository.GetUserByVerificationToken(token);
            if (user == null)
            {
                return BadRequest("Invalid token.");
            }

            user.VerifiedAt = DateTime.Now;
            await _authRepository.Save();

            return Ok(new { status = "success", message = "User verified! :)" });
        }


        [HttpPost("forgot-password")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            var user = await _userRepository.GetUserByEmail(email);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            if (await _authRepository.ForgetPassword(email))
            {
                if (user.PasswordResetToken is not null)
                {
                    string recipientName = $"{user.LastName} {user.FirstName}";
                    string emailTemplate = new EmailTemplate().GetEmailResetPasswordTemplate(recipientName, user.PasswordResetToken);

                    var sendEmailRequest = new SendEmailRequest
                    {
                        To = user.Email,
                        Subject = "Reset Password request",
                        Body = emailTemplate
                    };

                    _tokenRepository.SendEmail(sendEmailRequest);

                    return Ok(new { status = "success", message = "A confirmation mail was sent to the provided mail" });
                }
                else
                {
                    return BadRequest("Something went wrong");
                }
            }

            return BadRequest("Something went wrong");
        }


        [HttpPost("reset-password")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            try
            {
                var user = await _authRepository.GetUserByResetToken(resetPasswordRequest.Token);

                if (user == null)
                {
                    return BadRequest("User not found.");
                }

                if (user.ResetTokenExpires < DateTime.Now)
                {
                    return BadRequest("Invalid Token.");
                }

                _tokenRepository.CreatePasswordHash(resetPasswordRequest.Password, out byte[] passwordHash, out byte[] passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.PasswordResetToken = null;
                user.ResetTokenExpires = null;

                await _authRepository.Save();

                return Ok(new { status = "success", message = "Password successfully reset." });
            }
            catch (UserNotFoundException)
            {
                return BadRequest("User not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

    }
}