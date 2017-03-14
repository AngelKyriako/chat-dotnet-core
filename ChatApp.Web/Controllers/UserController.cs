using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Web.Controllers {

    using Model;
    using Service;

    [Route("api/v{version:apiVersion}/user")]
    public class UserController<K> : Controller {

        private IUserService<K> service;

        public UserController(IUserService<K> s) {
            service = s;
        }

        [HttpGet]
        public IEnumerable<UserModel<K>> Get() {
            return service.GetEnabled();
        }

        [HttpGet("{id}")]
        public UserModel<K> Get(K id) {
            return service.GetOneEnabled(id);
        }

        [HttpPost]
        public UserModel<K> Post([FromBody]UserModel<K> body) {
            service.Create(body);
            return body;
        }

        [HttpPut("{id}")]
        public UserModel<K> Put(K id, [FromBody]UserModel<K> body) {
            service.Update(id, body);
            return body;
        }

        [HttpDelete("{id}")]
        public void Delete(K id) {
            service.Disable(id);
        }
    }
}
