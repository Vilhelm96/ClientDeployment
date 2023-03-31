using Microsoft.AspNetCore.Mvc;

namespace PaymentAPI.Controllers
{
    public class DiagController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
