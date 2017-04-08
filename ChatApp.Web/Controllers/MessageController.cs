using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Logging;

namespace ChatApp.Web.Controllers {
    using Model;
    using Auth;
    using Service;
    using WS;

    [Route("api/v{version:apiVersion}/message")]
    public class MessageController : Controller {

        private ILogger _logger;
        private IMessageService _messages;
        private IAuthService _auth;
        private SocketController _wsController;

        public MessageController(ILoggerFactory loggerFactory, IMessageService messages, IAuthService auth,
                                SocketController wsController) {
            _logger = loggerFactory.CreateLogger<MessageController>();
            _messages = messages;
            _auth = auth;
            _wsController = wsController;
        }

        [HttpGet]
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

            createdModel = _messages.GetOneEnabled(createdModel.Id);

            // Because this call is not awaited, execution of the current method continues before the call is completed
            // That is cool, its what we want here.
            _wsController.SendMessage(new WSMessage<MessageModel>() {
                Action = "Sent",
                AccessToken = null,
                Payload = createdModel
            });

            return createdModel;
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