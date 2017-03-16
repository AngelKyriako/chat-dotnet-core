namespace ChatApp.Repository {
    using System;
    using System.Collections.Generic;
    using Model;

    public class MongoRepository<M> : IRepository<M> where M : BaseModel {
        public void Commit() {
            throw new NotImplementedException();
        }

        public M Create(M model) {
            throw new NotImplementedException();
        }

        public M CreateAndCommit(M model) {
            throw new NotImplementedException();
        }

        public void Delete(M model) {
            throw new NotImplementedException();
        }

        public void DeleteAndCommit(M model) {
            throw new NotImplementedException();
        }

        public void Disable(M model) {
            throw new NotImplementedException();
        }

        public void DisableAndCommit(M model) {
            throw new NotImplementedException();
        }

        public void Enable(M model) {
            throw new NotImplementedException();
        }

        public void EnableAndCommit(M model) {
            throw new NotImplementedException();
        }

        public IEnumerable<M> Get(int page = 0, int limit = 100) {
            throw new NotImplementedException();
        }

        public IEnumerable<M> GetDisabled(int page = 0, int limit = 100) {
            throw new NotImplementedException();
        }

        public IEnumerable<M> GetEnabled(int page = 0, int limit = 100) {
            throw new NotImplementedException();
        }

        public M GetOne(string id) {
            throw new NotImplementedException();
        }

        public M GetOneDisabled(string id) {
            throw new NotImplementedException();
        }

        public M GetOneEnabled(string id) {
            throw new NotImplementedException();
        }
    }
}
