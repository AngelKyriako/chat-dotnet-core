namespace ChatApp.Web.Controllers {
    using Service;

    public class AppUserController : UserController<long> {

        public AppUserController(IUserService<long> s) : base(s) {
        }

    }
}