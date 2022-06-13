using FriendsService.Models;
using FriendsService.Models.DataBase;
using FriendsService.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FriendsService.Controllers;

[Route("/")]
public class FriendsController : ControllerBase
{
    private readonly AppDbContext _context;
    private ILogger<FriendsController> _logger;

    private string? UserId => Request.Headers.ContainsKey("userId") ? Request.Headers["userId"].ToString() : null;

    public FriendsController(AppDbContext context, ILogger<FriendsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost("Request")]
    public async Task<IActionResult> RequestFriendship(FriendsShipRequestDto request)
    {
        if (request.UserId.Trim().Length == 0)
        {
            _logger.LogInformation("Friendship request userid is null");
            return BadRequest("UserId is required");
        }

        if (UserId == null)
        {
            _logger.LogInformation("userid header is null");
            return Unauthorized();
        }

        var lastRequest = await _context.FriendRequests
            .Where(f => !f.IsDeleted && f.From == UserId && f.To == request.UserId)
            .FirstOrDefaultAsync();
        if (lastRequest is {Responded: false})
        {
            _logger.LogInformation("User with id '{From}' already sent a request to this user with id '{To}'", UserId,
                request.UserId);
            return BadRequest("You have already requested friendship to this user");
        }

        var newRequest = new FriendRequest
        {
            From = UserId,
            To = request.UserId,
            Responded = false,
            IsAccepted = false,
            RespondDateTime = null,
        };
        _logger.LogInformation("User with id '{From}' sent a request to this user with id '{To}'", UserId,
            request.UserId);
        await _context.FriendRequests.AddAsync(newRequest);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("Accept")]
    public async Task<IActionResult> Accept(int id)
    {
        var request = await _context.FriendRequests.FindAsync(id);
        if (request is null)
        {
            _logger.LogInformation("Friendship request with id '{Id}' not found", id);
            return NotFound();
        }

        if (request.Responded)
        {
            _logger.LogInformation("Friendship request with id '{Id}' already responded", id);
            return BadRequest("Request already responded");
        }

        if (request.To != UserId)
        {
            _logger.LogInformation("User with id '{UserId} cannot respond to this request with id {RequestId}'", UserId,
                id);
            return BadRequest("You are not allowed to accept this request");
        }

        _logger.LogInformation("User with id '{UserId} accepted request with id {RequestId}'", UserId, id);
        request.IsAccepted = true;
        return Ok();
    }

    [HttpGet("Reject")]
    public async Task<IActionResult> Reject(int id)
    {
        var request = await _context.FriendRequests.FindAsync(id);
        if (request is null)
        {
            _logger.LogInformation("Friendship request with id '{Id}' not found", id);
            return NotFound();
        }

        if (request.Responded)
        {
            _logger.LogInformation("Friendship request with id '{Id}' already responded", id);
            return BadRequest("Request already responded");
        }

        if (request.To != UserId)
        {
            _logger.LogInformation("User with id '{UserId} cannot respond to this request with id {RequestId}'", UserId,
                id);
            return BadRequest("You are not allowed to reject this request");
        }

        _logger.LogInformation("User with id '{UserId} rejected request with id {RequestId}'", UserId, id);
        request.IsAccepted = false;
        return Ok();
    }
}