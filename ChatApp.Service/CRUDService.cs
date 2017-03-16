using System.Collections.Generic;

namespace ChatApp.Service {

    using Model;
    using Repository;

    public abstract class CRUDService<M> : ICRUDService<M> where M : BaseModel {

        private IRepository<M> _repo;

        public CRUDService(IRepository<M> repo) {
            _repo = repo;
        }

        public M Create(M model) {
            return _repo.CreateAndCommit(model);
        }

        public abstract M Update(string id, M model);

        public void Delete(M model) {
            _repo.Delete(model);
        }

        public void Delete(string id) {
            M model = _repo.GetOne(id);
            if (model != null) {
                _repo.Delete(model);
            }
        }

        public void Disable(M model) {
            _repo.Disable(model);
        }

        public void Disable(string id) {
            M model = _repo.GetOneEnabled(id);
            if (model != null) {
                _repo.Disable(model);
            }
        }

        public void Enable(M model) {
            _repo.Enable(model);
        }

        public void Enable(string id) {
            M model = _repo.GetOneDisabled(id);
            if (model != null) {
                _repo.Enable(model);
            }
        }

        public IEnumerable<M> Get(int page = 0, int limit = 100) {
            return _repo.Get(page, limit);
        }

        public IEnumerable<M> GetDisabled(int page = 0, int limit = 100) {
            return _repo.GetDisabled(page, limit);
        }

        public IEnumerable<M> GetEnabled(int page = 0, int limit = 100) {
            return _repo.GetEnabled(page, limit);
        }

        public M GetOne(string id) {
            return _repo.GetOne(id);
        }
        
        public M GetOneDisabled(string id) {
            return _repo.GetOneDisabled(id);
        }
        
        public M GetOneEnabled(string id) {
            return _repo.GetOneEnabled(id);
        }
    }
}