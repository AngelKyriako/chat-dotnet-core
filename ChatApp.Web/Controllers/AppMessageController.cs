namespace ChatApp.Web.Controllers {
    using Service;

    public class AppMessageController : MessageController<long> {

        public AppMessageController(IMessageService<long> s) : base(s) {
        }

    }
}
