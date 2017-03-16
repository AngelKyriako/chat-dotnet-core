using System.Collections.Generic;

namespace ChatApp.Repository {

    public interface IRepository<M> {
        // read
        IEnumerable<M> Get(int page = 0, int limit = 100);
        IEnumerable<M> GetEnabled(int page = 0, int limit = 100);
        IEnumerable<M> GetDisabled(int page = 0, int limit = 100);

        M GetOne(string id);
        M GetOneEnabled(string id);
        M GetOneDisabled(string id);

        // write without commit
        M Create(M model);
        void Delete(M model);
        void Disable(M model);
        void Enable(M model);

        // commit
        void Commit();

        // write with auto commit
        M CreateAndCommit(M model);
        void DisableAndCommit(M model);
        void EnableAndCommit(M model);
        void DeleteAndCommit(M model);
    }
}
