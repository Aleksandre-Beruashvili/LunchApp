using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OfficeCafeApp.API.Controllers
{
    [ApiController]
    [Route("admin")]
    public class AdminPageRedirectController : Controller
    {
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult RedirectToDashboard()
        {
            return Redirect("/admin/dashboard.html");
        }
    }
}
