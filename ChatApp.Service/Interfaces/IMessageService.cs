namespace ChatApp.Service {

    using Model;

    public interface IMessageService<K> : ICRUDService<MessageModel<K>, K> {
    }
}
