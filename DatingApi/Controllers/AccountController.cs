using AutoMapper;
using Core;
using Core.Interfaces;
using DatingApi.DTOS;
using DatingApi.Errors;
using DatingApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace DatingApi.Controllers
{
   
    public class AccountController : BaseController
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _token;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, IMapper mapper,ITokenService token, IUnitOfWork uow)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _token = token;
            _uow = uow;
        }

       
        [HttpGet("getUsers")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserDto>>> getUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var usersDto = _mapper.Map<List<UserDto> >(users);
            return Ok(usersDto);
        }
        
        [HttpGet("getUser/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> getUser(string id)
        {
            var user = await _userManager.Users.Include(x=>x.Photos).FirstOrDefaultAsync(x=>x.Id == id);
            if(user == null)
            {
                return NotFound(new ApiResponse(404));
            }
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserInfoDto>> login(UserLoginDto login)
        {

            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null) { 
                return Unauthorized(new ApiResponse(401));
            }
            
            var res = await _signInManager.CheckPasswordSignInAsync(user, login.Password,false);
            
            if (!res.Succeeded)
            {
                return Unauthorized(new ApiResponse(401));
            }
            user.LastActive = DateTime.Now;
           await _uow.SaveChangesAsync();
            return Ok(new UserInfoDto()
            {
                Id = user.Id,
                Token = _token.GetToken(user),
                UserName = user.UserName

            });
        }

        [HttpPost("register")]
        public async Task<ActionResult> register(UserRegisterDto model)
        {

            var res = await _userManager.FindByEmailAsync(model.Email);
            if (res != null) { 
                return BadRequest(new ApiResponse(400 , "Email is Already exist"));
            }

            var uesr = new AppUser()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                SecondName = model.SecondName,
                UserName = model.FirstName + "_" + model.SecondName,
                City = model.City,
                Country = model.Country,
                Created = DateTime.Now,
                DOB = model.DOB,
                Gender = model.Gender,
                Interests = model.Interests,
                PhoneNumber = model.PhoneNumber
            };
           
            var created = await _userManager.CreateAsync(uesr,model.Password);
            if (!created.Succeeded) {

                return BadRequest(new ApiResponse(400, string.Join(",", created.Errors.Select(x => x.Description))));
            }
            await _userManager.AddToRoleAsync(uesr, "Admin");
            
            return StatusCode(StatusCodes.Status201Created);

        }

        [HttpPut("update")]
        [Authorize]
        public async Task<ActionResult> update(UserUpdateDto model)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (user == null)
            {
                return NotFound(new ApiResponse(404));
            }
            user.FirstName = model.FirstName;
            user.SecondName = model.SecondName;
            user.City = model.City;
            user.Country = model.Country;
            user.Gender = model.Gender;
            user.Interests = model.Interests;
            user.Created = model.Created;
           await _userManager.UpdateAsync(user);
            return Ok();
        }

    }
}
