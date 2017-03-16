namespace ChatApp.Service {
    using Model;
    using Repository;

    public class MessageService : CRUDService<MessageModel>, IMessageService {

        private IMessageRepository _repo;
        public MessageService(IMessageRepository repo): base(repo) {
            _repo = repo;
        }

        public override MessageModel Update(string id, MessageModel model) {
            MessageModel message = GetOneEnabled(id);
            if (message != null) {
                message.Copy(model);
            }
            _repo.Commit();

            return message;
        }

    }
}
