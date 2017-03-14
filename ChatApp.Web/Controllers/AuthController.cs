using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Logging;

namespace ChatApp.Web.Controllers {

    using Auth;
    using Microsoft.AspNetCore.Authorization;
    using Model;

    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : Controller {

        private readonly ILogger _logger;
        private readonly IAuthService _service;

        public AuthController(ILoggerFactory loggerFactory, IAuthService service) {
            _logger = loggerFactory.CreateLogger<AuthController>();
            _service = service;
        }

        [HttpGet]
        public byte[] Get() {
            return _service.SigningKey.Key;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<JwtTokenModel> Login() {
            return await _service.IssueToken("angel", "angelpass");
        }
    }
}
