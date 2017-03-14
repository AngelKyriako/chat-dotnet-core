namespace ChatApp.Repository {
    using System;
    using System.Collections.Generic;
    using Model;

    public class MongoRepository<E> : IRepository<E, string> where E : BaseModel<string> {
        public void Commit() {
            throw new NotImplementedException();
        }

        public void Create(E entity) {
            throw new NotImplementedException();
        }

        public void CreateAndCommit(E entity) {
            throw new NotImplementedException();
        }

        public void Delete(E entity) {
            throw new NotImplementedException();
        }

        public void DeleteAndCommit(E entity) {
            throw new NotImplementedException();
        }

        public void Disable(E entity) {
            throw new NotImplementedException();
        }

        public void DisableAndCommit(E entity) {
            throw new NotImplementedException();
        }

        public void Enable(E entity) {
            throw new NotImplementedException();
        }

        public void EnableAndCommit(E entity) {
            throw new NotImplementedException();
        }

        public IEnumerable<E> Get(int page = 0, int limit = 100) {
            throw new NotImplementedException();
        }

        public IEnumerable<E> GetDisabled(int page = 0, int limit = 100) {
            throw new NotImplementedException();
        }

        public IEnumerable<E> GetEnabled(int page = 0, int limit = 100) {
            throw new NotImplementedException();
        }

        public E GetOne(string id) {
            throw new NotImplementedException();
        }

        public E GetOneDisabled(string id) {
            throw new NotImplementedException();
        }

        public E GetOneEnabled(string id) {
            throw new NotImplementedException();
        }
    }
}
