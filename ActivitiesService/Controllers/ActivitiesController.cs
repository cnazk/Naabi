using ActivitiesService.Models;
using ActivitiesService.Models.DataBase;
using ActivitiesService.Models.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ActivitiesService.Controllers;

[ApiController]
[Route("/")]
public class ActivitiesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ActivitiesController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("Categories")]
    public async Task<List<CategoryReadDto>> Categories() =>
        _mapper.Map<List<CategoryReadDto>>(await _context.ActivityCategories.Where(a => !a.Disabled && !a.IsDeleted)
            .ToListAsync());

    [HttpPost("Categories")]
    public async Task<IActionResult> CreateCategory(CategoryWriteDto category)
    {
        var entity = _mapper.Map<ActivityCategory>(category);
        await _context.ActivityCategories.AddAsync(entity);
        await _context.SaveChangesAsync();
        return CreatedAtAction("Category", new {categoryId = entity.Id});
    }

    [HttpGet("Categories/{categoryId}")]
    public async Task<IActionResult> Category(int categoryId)
    {
        if (await _context.ActivityCategories.FindAsync(categoryId) is {IsDeleted: false, Disabled: false} category)
            return Ok(_mapper.Map<CategoryReadDto>(category));
        return NotFound();
    }

    [HttpGet("Categories/{categoryId}/Activities")]
    public async Task<IActionResult> Activities(int categoryId)
    {
        var category = await _context.ActivityCategories.FindAsync(categoryId);
        if (category == null || category.IsDeleted || category.Disabled) return NotFound();
        return Ok(_mapper.Map<List<ActivityReadDto>>(await _context.Activities.Where(a => !a.Disabled && !a.IsDeleted)
            .Where(a => a.ActivityCategoryId == categoryId)
            .ToListAsync()));
    }

    [HttpPost("Categories/{categoryId}/Activities")]
    public async Task<IActionResult> CreateActivity(ActivityWriteDto activity, int categoryId)
    {
        var category = await _context.ActivityCategories.FindAsync(categoryId);
        if (category == null || category.IsDeleted) return NotFound();
        var entity = _mapper.Map<Activity>(activity);
        entity.ActivityCategoryId = categoryId;
        await _context.Activities.AddAsync(entity);
        await _context.SaveChangesAsync();
        return CreatedAtAction("Activity", new {categoryId = entity.Id});
    }

    [HttpGet("Categories/{categoryId}/Activities/{activityId}")]
    public async Task<IActionResult> Activity(int categoryId, int activityId)
    {
        var category = await _context.ActivityCategories.FindAsync(categoryId);
        if (category is {IsDeleted: false, Disabled: false} &&
            await _context.Activities.FindAsync(activityId) is {Disabled: false, IsDeleted: false} activity &&
            activity.ActivityCategoryId == categoryId)
            return Ok(_mapper.Map<ActivityReadDto>(activity));
        return NotFound();
    }
}