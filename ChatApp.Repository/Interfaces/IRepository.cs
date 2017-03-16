using System.Collections.Generic;

namespace ChatApp.Repository {

    public interface IRepository<M, K> {
        // read
        IEnumerable<M> Get(int page = 0, int limit = 100);
        IEnumerable<M> GetEnabled(int page = 0, int limit = 100);
        IEnumerable<M> GetDisabled(int page = 0, int limit = 100);

        M GetOne(K id);
        M GetOne(string id);
        M GetOneEnabled(K id);
        M GetOneEnabled(string id);
        M GetOneDisabled(K id);
        M GetOneDisabled(string id);

        // write without commit
        void Create(M entity);
        void Delete(M entity);
        void Disable(M entity);
        void Enable(M entity);

        // commit
        void Commit();

        // write with auto commit
        void CreateAndCommit(M entity);
        void DisableAndCommit(M entity);
        void EnableAndCommit(M entity);
        void DeleteAndCommit(M entity);
    }
}
