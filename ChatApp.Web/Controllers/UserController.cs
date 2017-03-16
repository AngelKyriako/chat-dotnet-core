using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Web.Controllers {

    using Model;
    using Service;

    [Route("api/v{version:apiVersion}/user")]
    public class UserController : Controller {

        private IUserService service;

        public UserController(IUserService s) {
            service = s;
        }

        [HttpGet]
        public IEnumerable<UserModel> Get() {
            return service.GetEnabled();
        }

        [HttpGet("{id}")]
        public UserModel Get(string id) {
            return service.GetOneEnabled(id);
        }

        [HttpPost]
        public UserModel Post([FromBody]UserModel body) {
            return service.Create(body);
        }

        [HttpPut("{id}")]
        public UserModel Put(string id, [FromBody]UserModel body) {
            return service.Update(id, body);
        }

        [HttpDelete("{id}")]
        public void Delete(string id) {
            service.Disable(id);
        }
    }
}
