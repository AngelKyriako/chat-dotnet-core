using System.Collections.Generic;

namespace ChatApp.Service {
    
    public interface ICRUDService<E, K> {

        IEnumerable<E> Get(int page = 0, int limit = 100);
        IEnumerable<E> GetEnabled(int page = 0, int limit = 100);
        IEnumerable<E> GetDisabled(int page = 0, int limit = 100);

        E GetOne(K id);
        E GetOne(string id);
        E GetOneEnabled(K id);
        E GetOneEnabled(string id);
        E GetOneDisabled(K id);
        E GetOneDisabled(string id);

        void Create(E model);

        void Update(K id, E model);

        void Delete(K id);
        void Delete(E model);

        void Disable(K id);
        void Disable(E model);

        void Enable(K id);
        void Enable(E model);
    }
}
