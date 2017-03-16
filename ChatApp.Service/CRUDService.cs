using System.Collections.Generic;

namespace ChatApp.Service {
    using Model;
    using Repository;

    public abstract class CRUDService<M> : ICRUDService<M> where M : BaseModel {

        private IRepository<M> _repo;

        public CRUDService(IRepository<M> repo) {
            _repo = repo;
        }

        public virtual M Create(M model) {
            return _repo.CreateAndCommit(model);
        }

        public abstract M Update(string id, M model);

        public virtual void Delete(M model) {
            _repo.Delete(model);
        }

        public virtual void Delete(string id) {
            M model = _repo.GetOne(id);
            if (model != null) {
                _repo.Delete(model);
            }
        }

        public virtual void Disable(M model) {
            _repo.Disable(model);
        }

        public virtual void Disable(string id) {
            M model = _repo.GetOneEnabled(id);
            if (model != null) {
                _repo.Disable(model);
            }
        }

        public virtual void Enable(M model) {
            _repo.Enable(model);
        }

        public virtual void Enable(string id) {
            M model = _repo.GetOneDisabled(id);
            if (model != null) {
                _repo.Enable(model);
            }
        }

        public virtual IEnumerable<M> Get(int page = 0, int limit = 100) {
            return _repo.Get(page, limit);
        }

        public virtual IEnumerable<M> GetDisabled(int page = 0, int limit = 100) {
            return _repo.GetDisabled(page, limit);
        }

        public virtual IEnumerable<M> GetEnabled(int page = 0, int limit = 100) {
            return _repo.GetEnabled(page, limit);
        }

        public virtual M GetOne(string id) {
            return _repo.GetOne(id);
        }
        
        public virtual M GetOneDisabled(string id) {
            return _repo.GetOneDisabled(id);
        }
        
        public virtual M GetOneEnabled(string id) {
            return _repo.GetOneEnabled(id);
        }
    }
}