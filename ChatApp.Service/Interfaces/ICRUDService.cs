using System.Collections.Generic;

namespace ChatApp.Service {
    
    public interface ICRUDService<M> {

        IEnumerable<M> Get(int page = 0, int limit = 100);
        IEnumerable<M> GetEnabled(int page = 0, int limit = 100);
        IEnumerable<M> GetDisabled(int page = 0, int limit = 100);
        
        M GetOne(string id);
        M GetOneEnabled(string id);
        M GetOneDisabled(string id);

        M Create(M model);

        M Update(string id, M model);

        void Delete(string id);
        void Delete(M model);

        void Disable(string id);
        void Disable(M model);

        void Enable(string id);
        void Enable(M model);
    }
}
