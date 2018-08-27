using AngularSPA.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AngularSPA.Controllers
{
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly IDbContext _dbContext;

        public AdminController(
            IDbContext dbContext
        )
        {
            _dbContext = dbContext;
        }

        [HttpGet("accounts/overview")]
        public async Task<IActionResult> GetAccountOverviewAsync()
        {
            var queryableUser = _dbContext.User
                .Select(usr => usr)
                .Include(usr => usr.Person)
                .Include(usr => usr.Role);

            var viewModel = await queryableUser.Select(usr =>
                    new {
                        usr.Person.GivenName,
                        usr.Person.Surname,
                        Role = usr.Role.Name,
                        usr.PhoneNumber,
                        usr.UserName
                    })
                .FirstOrDefaultAsync();
                //.ToListAsync();
            return Ok(viewModel);
        }
    }
}
