using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Web.Controllers {
    using Model;
    using Auth;
    using Service;

    [Route("api/v{version:apiVersion}/user")]
    public class UserController : Controller {

        private IUserService _users;
        private IAuthService _auth;

        public UserController(IUserService users, IAuthService auth) {
            _users = users;
            _auth = auth;
        }

        [HttpGet]
        public IEnumerable<UserModel> Get() {
            return _users.GetEnabled();
        }

        [HttpGet("{id}")]
        public UserModel Get(string id) {
            return _users.GetOneEnabled(id);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<UserAndToken> Post([FromBody]UserModel body) {
            return await _users.CreateAndAuthenticate(body);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<UserAndToken> Login([FromBody]LocalCredentials credentials) {
            return await _auth.IssueToken(credentials.Username, credentials.Password);
        }

        [HttpPut("{id}")]
        public UserModel Put(string id, [FromBody]UserModel body) {
            return _users.Update(id, body);
        }

        [HttpDelete("{id}")]
        public void Delete(string id) {
            _users.Disable(id);
        }
    }
}
