using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace MercaMetaBlazor.Controllers
{
    public class SiteController : Controller
    {
        

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");

            return LocalRedirect("/");
        }
    }
}
