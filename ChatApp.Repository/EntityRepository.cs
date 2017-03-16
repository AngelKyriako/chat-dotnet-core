using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;

namespace ChatApp.Repository {

    using Configuration;
    using Model;

    public class EntityRepository<M, K> : IRepository<M, K> where M : BaseModel<K> {

        private readonly AppEntityContext<K> _context;
        protected DbSet<M> _entities;

        protected string errorMessage = string.Empty;

        public EntityRepository(AppEntityContext<K> context) {
            _context = context;
            _entities = context.Set<M>();
        }

        // read
        public virtual IEnumerable<M> Get(int page = 0, int limit = 100) {
            return _entities
                .Skip(page * limit)
                .Take(limit)
                .ToList();
        }

        public virtual IEnumerable<M> GetEnabled(int page = 0, int limit = 100) {
            return _entities
                .Where(e => e.Enabled)
                .Skip(page * limit)
                .Take(limit)
                .ToList();
        }

        public virtual IEnumerable<M> GetDisabled(int page = 0, int limit = 100) {
            return _entities
                .Where(e => !e.Enabled)
                .Skip(page * limit)
                .Take(limit)
                .ToList();
        }

        public virtual M GetOne(K id) {
            return _entities
                .Where(e => e.Id.Equals(id))
                .FirstOrDefault();
        }

        public virtual M GetOneEnabled(K id) {
            return _entities
                .Where(e => e.Id.Equals(id))
                .Where(e => e.Enabled)
                .FirstOrDefault();
        }

        public virtual M GetOneDisabled(K id) {
            return _entities
                .Where(e => e.Id.Equals(id))
                .Where(e => !e.Enabled)
                .FirstOrDefault();
        }

        public virtual M GetOne(string id) {
            try {
                return GetOne((K)Convert.ChangeType(id, typeof(K)));
            } catch (Exception e) {
                Console.WriteLine("Could not convert string id to long: " + e);
                return null;
            }
        }

        public virtual M GetOneEnabled(string id) {
            try {
                return GetOneEnabled((K)Convert.ChangeType(id, typeof(K)));
            } catch (Exception e) {
                Console.WriteLine("Could not convert string id to long: " + e);
                return null;
            }
        }

        public virtual M GetOneDisabled(string id) {
            try {
                return GetOneDisabled((K)Convert.ChangeType(id, typeof(K)));
            } catch (Exception e) {
                Console.WriteLine("Could not convert string id to long: " + e);
                return null;
            }
        }

        // write
        public virtual void Create(M model) {
            if (model == null) {
                throw new ArgumentNullException("model");
            }
            _entities.Add(model);
        }

        public virtual void Disable(M model) {
            if (model == null) {
                throw new ArgumentNullException("model");
            }
            model.Enabled = false;
        }

        public virtual void Enable(M model) {
            if (model == null) {
                throw new ArgumentNullException("model");
            }
            model.Enabled = true;
        }

        public virtual void Delete(M model) {
            if (model == null) {
                throw new ArgumentNullException("model");
            }
            _entities.Remove(model);
        }

        // commit 
        public virtual void Commit() {
            _context.SaveChanges();
        }

        // write with auto commit
        public virtual void CreateAndCommit(M model) {
            Create(model);
            Commit();
        }

        public virtual void DisableAndCommit(M model) {
            Disable(model);
            Commit();
        }

        public virtual void EnableAndCommit(M model) {
            Enable(model);
            Commit();
        }

        public virtual void DeleteAndCommit(M model) {
            Delete(model);
            Commit();
        }
    }
}
