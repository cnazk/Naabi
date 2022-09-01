using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using IdentityService.Helpers;
using IdentityService.Models.DataBase;
using IdentityService.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Controllers;

[ApiController]
[Route("/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenFactory _jwtTokenFactory;
    private readonly IMapper _mapper;
    private readonly ILogger<AccountsController> _logger;

    public AccountsController(UserManager<ApplicationUser> userManager, IMapper mapper,
        IJwtTokenFactory jwtTokenFactory, ILogger<AccountsController> logger)
    {
        _userManager = userManager;
        _mapper = mapper;
        _jwtTokenFactory = jwtTokenFactory;
        _logger = logger;
    }

    [HttpPut("Edit")]
    [Authorize]
    public async Task<IActionResult> Edit(EditProfileDto registerDto)
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        if (user == null) return Unauthorized();
        _logger.LogInformation("Editing user {0}", user.UserName);
        user.FirstName = registerDto.FirstName;
        user.LastName = registerDto.LastName;
        user.PhoneNumber = registerDto.PhoneNumber;
        user.BirthDate = registerDto.BirthDate;
        await _userManager.UpdateAsync(user);
        _logger.LogInformation("User {0} edited", user.UserName);
        return await GetUserInfoAsync();
    }

    [HttpPost("Register")]
    public async Task<IActionResult> RegisterAsync(RegisterDto register)
    {
        _logger.LogInformation("Registering user {0}", register.UserName);
        var result = await _userManager.CreateAsync(new ApplicationUser
        {
            Email = register.Email,
            PhoneNumber = register.PhoneNumber,
            UserName = register.UserName,
            FirstName = register.FirstName,
            LastName = register.LastName,
            BirthDate = register.BirthDate,
        }, register.Password);
        if (result.Succeeded)
        {
            _logger.LogInformation("User {0} registered", register.UserName);
            return Ok();
        }

        _logger.LogError("Error registering user {0}, error: {1}", register.UserName, result.Errors);
        return BadRequest(new
        {
            Message = string.Join(", ", result.Errors.Select(e => e.Description))
        });
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> LoginAsync(LoginDto model)
    {
        _logger.LogInformation("Logging in as '{0}' with password '{1}'", model.UserName, model.Password);
        ApplicationUser? user;
        user = (await _userManager.FindByNameAsync(model.UserName) ??
                await _userManager.FindByEmailAsync(model.UserName)) ??
               await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == model.UserName);
        if (user == null)
        {
            _logger.LogInformation("User could not be found...");
            return Unauthorized(new {message = "نام کاربری یا رمز عبور اشتباه است"});
        }

        if (!await _userManager.CheckPasswordAsync(user, model.Password))
        {
            _logger.LogInformation("Password is wrong for user {0} with id {1}", user.UserName, user.Id);
            return Unauthorized(new {message = "نام کاربری یا رمز عبور اشتباه است"});
        }

        _logger.LogInformation("User {0} logged in", user.UserName);
        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

        var token = _jwtTokenFactory.GetToken(authClaims);

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo
        });
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<List<UserInfoDto>> GetUsersAsync()
    {
        return _mapper.Map<List<UserInfoDto>>(await _userManager.Users.ToListAsync());
    }

    [Authorize]
    [ProducesResponseType(typeof(UserInfoDto), 200)]
    [ProducesResponseType(401)]
    [HttpGet("Profile")]
    public async Task<IActionResult> GetUserInfoAsync()
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        if (user == null)
        {
            _logger.LogError("User {0} not found", User.Identity.Name);
            return Unauthorized();
        }

        _logger.LogInformation("User {0} found", user.UserName);
        return Ok(_mapper.Map<UserInfoDto>(user));
    }
}