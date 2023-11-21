using Ecom.Jarad.API.Errors;
using Ecom.Jarad.Core.DTOS;
using Ecom.Jarad.Core.Entities;
using Ecom.Jarad.Core.Interfaces;
using Ecom.Jarad.Core.Services;
using Ecom.Jarad.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecom.Jarad.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUsers> _userManager;
        private readonly SignInManager<AppUsers> _signInManager;
        private readonly IEmailSender _emailSender;
        public AccountController(ApplicationDbContext context, ITokenService tokenService, SignInManager<AppUsers> signInManager, UserManager<AppUsers> userManager, IEmailSender emailSender)
        {
            _context = context;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
        }
        [HttpPost("Register")]
        public async Task<ActionResult> CreateUser([FromBody] registerDto RegisterDto)
        {
            // check if the Dto is null
            if (RegisterDto is null)
            {
                return BadRequest(new BaseResponse(404, "the item is null !"));
            }

            // check if the email already Exist =>
            // if return false that mean he can not register because Email exsit
            bool check_the_email = NoneEmailExist(RegisterDto.Email).Result.Value;

            if (check_the_email is true)
            {
                return BadRequest(new BaseResponse(400, $"This Email = {RegisterDto.Email} already Exist !"));
            }


            // check if the UserName already Exist =>
            // if return false that mean he can not register because user-name exsit
            bool check_the_UserName = NoneUserNameExist(RegisterDto.UserName).Result.Value;

            if (check_the_UserName is true)
            {
                return BadRequest(new BaseResponse(400, $"This userName = {RegisterDto.UserName} already Exist !"));
            }

            // map the item in AppUsers

            AppUsers user = new()
            {
                UserName = RegisterDto.UserName,

                Email = RegisterDto.Email,

                TimeRefreshToken = DateTime.Now.AddDays(5),

                refreshToken = _tokenService.CreateRefreshToken(),
            };

            // Create the user in DataBase
            var result = await _userManager.CreateAsync(user, RegisterDto.Password);
            if (result.Succeeded is false)
            {
                return Unauthorized(new BaseResponse(401, result.Errors.ToList()[0].Description));
            }

            return Ok(new ResponeIdentity(_tokenService.GetAndCreateToken(user), user.refreshToken));
        }


        [HttpPost("Login")]
        public async Task<IActionResult> RegisterUser(SignDto signDto)
        {
            //check the user if he Register ??
            AppUsers user = await _userManager.FindByEmailAsync(signDto.EmailOrUserName);

            if (user is null)
            {
                user = await _userManager.FindByNameAsync(signDto.EmailOrUserName);
                if (user is null)
                {
                    return BadRequest(new BaseResponse(401, "this user not register or sign up ??"));
                }

            }


            //let check the password is correct or not
            var result = await _signInManager.PasswordSignInAsync(user, signDto.password, true, true);

            if (result is null || result.Succeeded == false)
            {

                return Unauthorized(new BaseResponse(401, "password or username incorrect"));
            }
            user.refreshToken = _tokenService.CreateRefreshToken();
            await _userManager.UpdateAsync(user);
            return Ok(new ResponeIdentity(_tokenService.GetAndCreateToken(user), user.refreshToken));
        }

      
        [HttpPost("refresh")]
        public async Task<ActionResult> Refresh([FromBody] ResponeIdentity tokenApiDto)
        {
            //check the Token if null
            if (tokenApiDto is null)
                return BadRequest(new BaseResponse(400, "Invalid Client Request"));

            // 
            string accessToken = tokenApiDto.Token;

            string refreshToken = tokenApiDto.RefreshToken;
            // Get the Name from Authoriaztion
            var principal = _tokenService.GetPrincipalFromRefreshToken(accessToken);
            var username = principal.Identity.Name;

            // Check the user if he sign up
            var user = await _userManager.FindByNameAsync(username);
            if (user is null || user.refreshToken != refreshToken || user.TimeRefreshToken <= DateTime.Now)
                return BadRequest(new BaseResponse(400, "Invalid Request"));

            //Create Token And RefreshToken
            var newAccessToken = _tokenService.GetAndCreateToken(user);
            var newRefreshToken = _tokenService.CreateRefreshToken();

            //Update the Refresh Token in the database
            user.refreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);


            return Ok(new ResponeIdentity(newAccessToken, newRefreshToken));
        }


        [HttpGet("reset-password")]
        public async Task<IActionResult> forgetPassword(string Email)
        {
            // Get the user by Email
            AppUsers user = await _userManager.FindByEmailAsync(Email);

            //locked the account 
            user.LockoutEnd = DateTime.Now.AddDays(7);
            if (user is null)
            {
                return BadRequest(new BaseResponse(404, "something  went wrong,check your email not found"));
            }
            // generate random code verification
            string randomtoken = DateTime.Now.ToFileTimeUtc().ToString();

            string emailTokne = randomtoken.Substring(9, 6);

            user.ResetPasswordToken = emailTokne.ToString();

            // Add Expier to this Code
            user.ResetPasswordTokenExpier = DateTime.Now.AddMinutes(5);

            await _userManager.UpdateAsync(user);

            EmailModelDTO emaliModel = new EmailModelDTO(Email, "Reset Password", EmailBody.EmailStringBody(emailTokne));
            _emailSender.sendEmail(emaliModel);
            return Ok(new
           BaseResponse(200, "check your Email Please"));
        }


        [HttpPost("check-verifi-code")]
        public async Task<IActionResult> checkCode(CheckCodeDTO codeDTO)
        {
            //get the user and check if code Expire
            AppUsers user = await _userManager.FindByEmailAsync(codeDTO.Email);

            if (user is null)
            {
                return BadRequest(new BaseResponse(404, "something  went wrong,check your email not found"));
            }


            if (user.ResetPasswordTokenExpier <= DateTime.Now || codeDTO.code != user.ResetPasswordToken)
            {
                return BadRequest(new BaseResponse(401, "your code is Expire :("));
            }

            // open lock the account
            user.ResetPasswordToken = string.Empty;
            user.LockoutEnd = DateTime.Now;
            await _userManager.UpdateAsync(user);


            return Ok(new BaseResponse(200));
        }


        [HttpPost("new-password")]
        public async Task<IActionResult> newPassword(updatePassword updatePassword)
        {
            // Get the user by Email
            AppUsers user = await _userManager.FindByEmailAsync(updatePassword.Email);
            if (user is null)
            {
                return BadRequest(new BaseResponse(404, "something  went wrong,check your email not found"));
            }

            //check if the password match confirm password

            if (updatePassword.password != updatePassword.ConfirmPassword)
            {
                return BadRequest(new BaseResponse(404, "something  went wrong,password not matched"));
            }



            // check the code if he Expier
            if (user.ResetPasswordTokenExpier < DateTime.Now)
            {
                return BadRequest(new BaseResponse(400, "something went worng"));
            }


            //update the password and check if password correct
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, token, updatePassword.password);
            if (result.Succeeded is false)
            {
                return BadRequest(new BaseResponse(400, "something went wrong !"));
            }


            // 
            user.ResetPasswordToken = string.Empty;
            user.ResetPasswordTokenExpier = DateTime.Now;
            await _userManager.UpdateAsync(user);


            return Ok(new BaseResponse(200));
        }



        [HttpGet("check-email-exist")]
        public async Task<ActionResult<bool>> NoneEmailExist(string email)
        => Ok(await _userManager.FindByEmailAsync(email));



        [HttpGet("check-username-exist")]
        public async Task<ActionResult<bool>> NoneUserNameExist(string username)
          => Ok(await _userManager.FindByNameAsync(username));



        [HttpGet("get-all-user")]
        [Authorize]
        public async Task<IActionResult> usercurrent()
        => Ok(await _userManager.Users.ToListAsync());
    }
}
