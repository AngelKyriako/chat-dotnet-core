using System.Collections.Generic;

namespace ChatApp.Service {

    using Model;
    using Repository;

    public abstract class CRUDService<E, K> : ICRUDService<E, K> where E : BaseModel<K> {

        private IRepository<E, K> _repo;

        public CRUDService(IRepository<E, K> repo) {
            _repo = repo;
        }

        public void Create(E model) {
            _repo.CreateAndCommit(model);
        }

        public void Delete(E model) {
            _repo.Delete(model);
        }

        public void Delete(K id) {
            E model = _repo.GetOne(id);
            if (model != null) {
                _repo.Delete(model);
            }
        }

        public void Disable(E model) {
            _repo.Disable(model);
        }

        public void Disable(K id) {
            E model = _repo.GetOneEnabled(id);
            if (model != null) {
                _repo.Disable(model);
            }
        }

        public void Enable(E model) {
            _repo.Enable(model);
        }

        public void Enable(K id) {
            E model = _repo.GetOneDisabled(id);
            if (model != null) {
                _repo.Enable(model);
            }
        }

        public IEnumerable<E> Get(int page = 0, int limit = 100) {
            return _repo.Get(page, limit);
        }

        public IEnumerable<E> GetDisabled(int page = 0, int limit = 100) {
            return _repo.GetDisabled(page, limit);
        }

        public IEnumerable<E> GetEnabled(int page = 0, int limit = 100) {
            return _repo.GetEnabled(page, limit);
        }

        public E GetOne(string id) {
            return _repo.GetOne(id);
        }

        public E GetOne(K id) {
            return _repo.GetOne(id);
        }

        public E GetOneDisabled(string id) {
            return _repo.GetOneDisabled(id);
        }

        public E GetOneDisabled(K id) {
            return _repo.GetOneDisabled(id);
        }

        public E GetOneEnabled(string id) {
            return _repo.GetOneEnabled(id);
        }

        public E GetOneEnabled(K id) {
            return _repo.GetOneEnabled(id);
        }

        public abstract void Update(K id, E model);
    }
}