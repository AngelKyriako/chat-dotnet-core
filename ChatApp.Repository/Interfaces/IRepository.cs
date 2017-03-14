using System.Collections.Generic;

namespace ChatApp.Repository {

    public interface IRepository<E, K> {
        // read
        IEnumerable<E> Get(int page = 0, int limit = 100);
        IEnumerable<E> GetEnabled(int page = 0, int limit = 100);
        IEnumerable<E> GetDisabled(int page = 0, int limit = 100);

        E GetOne(K id);
        E GetOne(string id);
        E GetOneEnabled(K id);
        E GetOneEnabled(string id);
        E GetOneDisabled(K id);
        E GetOneDisabled(string id);

        // write without commit
        void Create(E entity);
        void Delete(E entity);
        void Disable(E entity);
        void Enable(E entity);

        // commit
        void Commit();

        // write with auto commit
        void CreateAndCommit(E entity);
        void DisableAndCommit(E entity);
        void EnableAndCommit(E entity);
        void DeleteAndCommit(E entity);
    }
}
