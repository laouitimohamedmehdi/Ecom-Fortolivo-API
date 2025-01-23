﻿using AutoMapper;
using Ecom.API.Errors;
using Ecom.API.Extension;
using Ecom.Core.Dtos;
using Ecom.Core.Entites;
using Ecom.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _singnInManager;
        private readonly ITokenServices _tokenServices;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> singnInManager, ITokenServices tokenServices, IMapper mapper)
        {
            _userManager = userManager;
            _singnInManager = singnInManager;
            _tokenServices = tokenServices;
            _mapper = mapper;
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseCommonResponse), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Login(LoginDto dto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(dto.Email);
                if (user is null) return Unauthorized(new BaseCommonResponse(401));

                var result = await _singnInManager.CheckPasswordSignInAsync(user, dto.Password, false);
                if (result is null || result.Succeeded == false) return Unauthorized(new BaseCommonResponse(401));

                return Ok(new UserDto
                {
                    DisplayName = user.DisplayName,
                    Email = dto.Email,
                    Token =_tokenServices.CreateToken(user)
                });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseCommonResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Register(RegisterDto dto)
        {
            if(CheckEmailExist(dto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[]
                    {"This Email is Already Exist"}
                });
            }
            var user = new AppUser
            {
                DisplayName = dto.DisplayName,
                UserName = dto.Email,
                Email = dto.Email
            };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (result.Succeeded == false) return BadRequest(new BaseCommonResponse(400));
            return Ok(new UserDto
            {
                DisplayName = dto.DisplayName,
                Email = dto.Email,
                Token = _tokenServices.CreateToken(user)
            });
        }

        [Authorize]
        [HttpGet("test")]
        public ActionResult<string> Test()
        {
            return "test success";
        }

        [Authorize]
        [HttpGet("Get-Current-User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCurrentUser()
        {
            //var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            //var user = await _userManager.FindByEmailAsync(email);
            var user = await _userManager.FindEmailByClaimPrincipal(HttpContext.User);
            return Ok(new UserDto
            {
                DisplayName = user?.DisplayName,
                Email = user?.Email,
                Token = _tokenServices.CreateToken(user)
            });
        }

        [HttpGet("Check-Email-Exist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CheckEmailExist([FromQuery]string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            if (result is not null)
            {
                return true;
            }
            return false;
        }

        [Authorize]
        [HttpGet("Get-User-Address")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserAddress()
        {
            //var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            //var user = await _userManager.Users.Include(x=>x.Address).SingleOrDefaultAsync(x=>x.Email == email);
            var user = await _userManager.FindUserByClaimPrincipalWithAddress(HttpContext.User);
            return Ok(_mapper.Map<Address, AddressDto>(user.Address));
        }

        [Authorize]
        [HttpPut("Update-User-Address")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseCommonResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserAddress(AddressDto dto)
        {
            var user = await _userManager.FindUserByClaimPrincipalWithAddress(HttpContext.User);
            user.Address = _mapper.Map<AddressDto, Address>(dto);
            var res = await _userManager.UpdateAsync(user);
            if (res.Succeeded) return Ok(_mapper.Map<Address, AddressDto>(user.Address));
            return BadRequest($"Problem with updating this user {HttpContext.User}");
        }
    }
}
