using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ChatApp.Web.Controllers {
    using Model;
    using Service;

    [Route("api/v{version:apiVersion}/message")]
    public class MessageController : Controller {

        private IMessageService service;

        public MessageController(IMessageService s) {
            service = s;
        }

        [HttpGet]
        [Authorize(Policy = "RegularUser")]
        public IEnumerable<MessageModel> Get() {
            return service.GetEnabled();
        }

        [HttpGet("{id}")]
        public MessageModel Get(string id) {
            return service.GetOneEnabled(id);
        }

        [HttpPost]
        public MessageModel Post([FromBody]MessageModel body) {
            MessageModel createdModel = service.Create(body);
            return service.GetOneEnabled(createdModel.Id);
        }

        [HttpPut("{id}")]
        public MessageModel Put(string id, [FromBody]MessageModel body) {
            service.Update(id, body);
            return service.GetOneEnabled(id);
        }

        [HttpDelete("{id}")]
        public void Delete(string id) {
            service.Disable(id);
        }
    }
}