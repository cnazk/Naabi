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

    public AccountsController(UserManager<ApplicationUser> userManager, IMapper mapper,
        IJwtTokenFactory jwtTokenFactory)
    {
        _userManager = userManager;
        _mapper = mapper;
        _jwtTokenFactory = jwtTokenFactory;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> RegisterAsync(RegisterDto register)
    {
        var result = await _userManager.CreateAsync(new ApplicationUser
        {
            Email = register.Email,
            PhoneNumber = register.PhoneNumber,
            UserName = register.UserName
        }, register.Password);
        if (result.Succeeded) return Ok();
        return BadRequest(new
        {
            Message = string.Join(", ", result.Errors.Select(e => e.Description))
        });
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> LoginAsync(LoginDto model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password)) return Unauthorized();
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

    [Authorize]
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
        Console.WriteLine(string.Join(" |||| ", Request.Headers.Select(h => $"{h.Key}: {h.Value}")));
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        if (user == null) return Unauthorized();
        return Ok(_mapper.Map<UserInfoDto>(user));
    }
}