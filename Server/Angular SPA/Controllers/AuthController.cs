using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularSPA.Data;
using AngularSPA.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularSPA.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IDbContext _dbContext;

        public AuthController(
            IPasswordHasher passwordHasher,
            IDbContext dbContext
        )
        {
            _passwordHasher = passwordHasher;
            _dbContext = dbContext;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Credentials credentials)
        {
            var userQueryAble = _dbContext.User
                .Where(usr => usr.UserName.Equals(credentials.UserName));
            if (!userQueryAble.Any())
                return BadRequest();

            var hash = userQueryAble
                .Include(usr => usr.Password)
                .Select(usr => usr.Password.Hash)
                .FirstOrDefault();
            if (_passwordHasher.VerifyPassword(credentials.Password, hash))
                return Ok();
            return BadRequest();
        }
    }
}
