using AutoMapper;
using Core;
using Core.Interfaces;
using DatingApi.DTOS;
using DatingApi.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApi.Controllers
{
   
    public class AccountController : BaseController
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _token;
        private readonly IMapper _mapper;
        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, IMapper mapper,ITokenService token)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _token = token;
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
        public async Task<ActionResult<UserDto>> getUser(string id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x=>x.Id == id);
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
                UserName = model.FirstName + "_" + model.SecondName
            };
           
            var created = await _userManager.CreateAsync(uesr,model.Password);
            if (!created.Succeeded) {

                return BadRequest(new ApiResponse(400, string.Join(",", created.Errors.Select(x => x.Description))));
            }
            await _userManager.AddToRoleAsync(uesr, "Admin");
            
            return StatusCode(StatusCodes.Status201Created);

        }
    }
}
