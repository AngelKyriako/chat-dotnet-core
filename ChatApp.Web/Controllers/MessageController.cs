using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ChatApp.Web.Controllers {
    using Model;
    using Auth;
    using Service;

    [Route("api/v{version:apiVersion}/message")]
    public class MessageController : Controller {

        private IMessageService _messages;
        private IAuthService _auth;

        public MessageController(IMessageService messages, IAuthService auth) {
            _messages = messages;
            _auth = auth;
        }

        [HttpGet]
        [Authorize(Policy = "RegularUser")]
        public IEnumerable<MessageModel> Get() {
            return _messages.GetEnabled();
        }

        [HttpGet("{id}")]
        public MessageModel Get(string id) {
            return _messages.GetOneEnabled(id);
        }

        [HttpPost]
        public MessageModel Post([FromBody]MessageModel body) {
            //UserModel creator = _auth.GetTokenUser(bearerToken);
            //body.CreatorId = creator.Id;
            MessageModel createdModel = _messages.Create(body);
            //createdModel.Creator = creator;
            return _messages.GetOneEnabled(createdModel.Id);
        }

        [HttpPut("{id}")]
        public MessageModel Put(string id, [FromBody]MessageModel body) {
            _messages.Update(id, body);
            return _messages.GetOneEnabled(id);
        }

        [HttpDelete("{id}")]
        public void Delete(string id) {
            _messages.Disable(id);
        }
    }
}