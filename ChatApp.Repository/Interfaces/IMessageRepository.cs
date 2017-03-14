namespace ChatApp.Repository {
    using Model;

    public interface IMessageRepository<K> : IRepository<MessageModel<K>, K> {
    }
}