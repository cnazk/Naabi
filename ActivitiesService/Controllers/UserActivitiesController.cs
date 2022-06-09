using ActivitiesService.Models;
using ActivitiesService.Models.DataBase;
using ActivitiesService.Models.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ActivitiesService.Controllers;

[ApiController]
[Route("/[controller]")]
public class UserActivitiesController: ControllerBase
{
    private string UserId => Request.Headers.ContainsKey("userId") ? Request.Headers["userId"].ToString().Trim() : "";
    
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public UserActivitiesController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<IActionResult> UserActivities(int take=-1, int skip=-1, int? activityId = null)
    {
        if (UserId == "")
            return NotFound();
        var query = _context.UserActivities.Where(activity => activity.UserId == UserId);
        if (skip != -1)
            query = query.Skip(skip);
        if (take != -1)
            query = query.Take(take);
        if (activityId != null)
            query = query.Where(a => a.ActivityId == activityId);
        return Ok(await query.ToListAsync());
    }

    [HttpPost("AddUserActivity")]
    public async Task<IActionResult> AddUserActivity(UserActivityWriteDto userActivity)
    {
        var entity = _mapper.Map<UserActivity>(userActivity);
        entity.UserId = UserId;
        await _context.UserActivities.AddAsync(entity);
        await _context.SaveChangesAsync();
        return Ok(entity);
    }
}