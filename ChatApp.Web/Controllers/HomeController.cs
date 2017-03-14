using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Web.Controllers {
    public class HomeController : Controller {

        /// <summary>
        /// Index view
        /// </summary>
        /// <returns> API documentation </returns>
        public IActionResult Index() {
            return RedirectPermanent("/api/ui");
        }

        /// <summary>
        /// Error view
        /// </summary>
        /// <returns> error information </returns>
        public IActionResult Error() {
            return View();
        }
    }
}
