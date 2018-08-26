using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularSPA.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularSPA.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly IDbContext _dbContext;

        public TestController(
            IDbContext dbContext    
        )
        {
            _dbContext = dbContext;
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            //var roles = await _dbContext.Role.ToListAsync();

            var roleQueryable = _dbContext.Role
                .AsQueryable()
                .Select(role => new
                {
                    role.Name,
                    role.Description
                });
            var roleView = await roleQueryable.ToListAsync();

            return Ok(roleView);
        }

    }
}