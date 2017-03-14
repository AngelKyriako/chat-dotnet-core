using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ChatApp.Web.Controllers {
    using Model;
    using Service;

    [Route("api/v{version:apiVersion}/message")]
    public class MessageController<K> : Controller {

        private IMessageService<K> service;

        public MessageController(IMessageService<K> s) {
            service = s;
        }

        [HttpGet]
        [Authorize(Policy = "RegularUser")]
        public IEnumerable<MessageModel<K>> Get() {
            return service.GetEnabled();
        }

        [HttpGet("{id}")]
        public MessageModel<K> Get(K id) {
            return service.GetOneEnabled(id);
        }

        [HttpPost]
        public MessageModel<K> Post([FromBody]MessageModel<K> body) {
            service.Create(body);
            return service.GetOneEnabled(body.Id);
        }

        [HttpPut("{id}")]
        public MessageModel<K> Put(K id, [FromBody]MessageModel<K> body) {
            service.Update(id, body);
            return service.GetOneEnabled(body.Id);
        }

        [HttpDelete("{id}")]
        public void Delete(K id) {
            service.Disable(id);
        }
    }
}