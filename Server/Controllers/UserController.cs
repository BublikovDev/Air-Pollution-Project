﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Shared.Models.User;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _dataContext;

        public UserController(UserManager<ApplicationUser> userManager, AppDbContext dataContext)
        {
            _userManager = userManager;
            _dataContext = dataContext;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("[action]/{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                ApplicationUser user = await _userManager.FindByIdAsync(id);

                if (user == null)
                    return NotFound("User with this ID is not found");

                user.IsDeleted = true;
                await _dataContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("[action]/{id}")]
        public async Task<ActionResult> RestoreDeleted(string id)
        {
            try
            {
                ApplicationUser user = await _userManager.FindByIdAsync(id);

                if (user == null)
                    return NotFound("User with this ID is not found");

                user.IsDeleted = false;
                await _dataContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult> GetById(string id)
        {
            try
            {
                ApplicationUser user = await _userManager.FindByIdAsync(id);

                if (user == null)
                    return NotFound("User with this ID is not found");

                var countries = await _dataContext.Countries
                    .Include(c => c.Users)
                    .Where(c=>c.Users.FirstOrDefault(u=>u.Id==user.Id)!=null)
                    .ToListAsync();

                user.Countries = countries;

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
