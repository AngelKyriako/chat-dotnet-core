namespace ChatApp.Service {

    using Model;
    using Repository;

    public class MessageService<K> : CRUDService<MessageModel<K>, K>, IMessageService<K> {

        private IMessageRepository<K> _repo;
        public MessageService(IMessageRepository<K> repo): base(repo) {
            _repo = repo;
        }

        public override void Update(K id, MessageModel<K> model) {
            MessageModel<K> user = GetOneEnabled(id);
            if (user != null) {
                user.Copy(model);
            }
            _repo.Commit();
        }

    }
}
