using AngularSPA.Auth;
using AngularSPA.Auth.Helpers;
using AngularSPA.Data;
using AngularSPA.Helpers;
using AngularSPA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace AngularSPA.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtIssuerOptions;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ISignInManager _signInManager;
        private readonly IDbContext _dbContext;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IJwtFactory jwtFactory,
            IOptions<JwtIssuerOptions> jwtIssuerOptions,
            IPasswordHasher passwordHasher,
            ISignInManager signInManager,
            IDbContext dbContext,
            ILogger<AuthController> logger
        )
        {
            _jwtFactory = jwtFactory;
            _jwtIssuerOptions = jwtIssuerOptions.Value;
            _passwordHasher = passwordHasher;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Credentials credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userName = credentials.UserName;
            var password = credentials.Password;

            // Verify credentials
            var res = await VerifyCredentials(userName, password);
            switch (res)
            {
                case (int)Constants.LoginStates.OK:
                    _logger.LogDebug("Valid credentials");
                    break;
                case (int)Constants.LoginStates.InvalidCredentials:
                    _logger.LogDebug("Invalid credentials");
                    ModelState.AddModelError("login_failure", "Invalid credentials.");
                    return BadRequest(ModelState);
                case (int)Constants.LoginStates.LockedOut:
                    _logger.LogWarning("User account is locked out.");
                    return StatusCode(StatusCodes.Status403Forbidden, ModelState);
                default:
                    return Forbid();
            }

            var user = await _dbContext.User
                .Where(usr => usr.UserName.Equals(userName))
                .Include(usr => usr.Password)
                .Include(usr => usr.Role)
                .FirstOrDefaultAsync();

            var identity = await GetClaimsIdentity(user);
            if (identity == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, credentials.UserName, _jwtIssuerOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
            _logger.LogInformation(jwt);
            return new ContentResult
            {
                Content = jwt,
                ContentType = "application/json; charset=utf-8",
                StatusCode = 200
            };
        }

        private async Task<int> VerifyCredentials(string userName, string password)
        {
            // Check for empty username
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return (int)Constants.LoginStates.InvalidCredentials;

            // Verify that user exists
            var userQueryAble = _dbContext.User
                .Where(usr => usr.UserName.Equals(userName));
            if (!userQueryAble.Any())
            {
                _logger.LogDebug("User is null");
                return (int)Constants.LoginStates.InvalidCredentials;
            }

            // Get user
            var user = await userQueryAble.Include(usr => usr.Password).FirstOrDefaultAsync();

            // Verify username and password match
            var result = _signInManager.TrySignInWithPassword(user, password);
            if (!result.Succeeded)
                return (int) Constants.LoginStates.InvalidCredentials;

            _logger.LogInformation("User logged in successfully");
            return (int)Constants.LoginStates.OK;

        }

        private async Task<System.Security.Claims.ClaimsIdentity> GetClaimsIdentity(User user)
        {
            var roleClaims = await _dbContext.RoleClaim
                .Where(rc => rc.RoleId == user.RoleId)
                .Include(rc => rc.Claim)
                .ThenInclude(c => c.ClaimType)
                .ToListAsync();

            var claims = roleClaims.Join(_dbContext.Claim,
                rc => rc.ClaimId,
                c => c.ClaimId,
                (rc, c) => new System.Security.Claims.Claim
                (
                    type: c.ClaimType.Name,
                    value: c.ClaimValue
                )).ToList();

            _logger.LogDebug($"Generating JWT for user {user.Id}");

            return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(user.UserName, user.UserId, user.Role.Name, claims));
        }


        [Authorize]
        [HttpGet("hasValidToken")]
        public IActionResult HasValidToken()
        {
            return Ok();
        }


    }
}
