using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;

namespace ChatApp.Repository {

    using Configuration;
    using Model;

    public class EntityRepository<E, K> : IRepository<E, K> where E : BaseModel<K> {

        private readonly AppEntityContext<K> _context;
        protected DbSet<E> _entities;

        protected string errorMessage = string.Empty;

        public EntityRepository(AppEntityContext<K> context) {
            _context = context;
            _entities = context.Set<E>();
        }

        // read
        public virtual IEnumerable<E> Get(int page = 0, int limit = 100) {
            return _entities
                .Skip(page * limit)
                .Take(limit)
                .ToList();
        }

        public virtual IEnumerable<E> GetEnabled(int page = 0, int limit = 100) {
            return _entities
                .Where(e => e.Enabled)
                .Skip(page * limit)
                .Take(limit)
                .ToList();
        }

        public virtual IEnumerable<E> GetDisabled(int page = 0, int limit = 100) {
            return _entities
                .Where(e => !e.Enabled)
                .Skip(page * limit)
                .Take(limit)
                .ToList();
        }

        public virtual E GetOne(K id) {
            return _entities
                .Where(e => e.Id.Equals(id))
                .FirstOrDefault();
        }

        public virtual E GetOneEnabled(K id) {
            return _entities
                .Where(e => e.Id.Equals(id))
                .Where(e => e.Enabled)
                .FirstOrDefault();
        }

        public virtual E GetOneDisabled(K id) {
            return _entities
                .Where(e => e.Id.Equals(id))
                .Where(e => !e.Enabled)
                .FirstOrDefault();
        }

        public virtual E GetOne(string id) {
            try {
                return GetOne((K)Convert.ChangeType(id, typeof(K)));
            } catch (Exception e) {
                Console.WriteLine("Could not convert string id to long: " + e);
                return null;
            }
        }

        public virtual E GetOneEnabled(string id) {
            try {
                return GetOneEnabled((K)Convert.ChangeType(id, typeof(K)));
            } catch (Exception e) {
                Console.WriteLine("Could not convert string id to long: " + e);
                return null;
            }
        }

        public virtual E GetOneDisabled(string id) {
            try {
                return GetOneDisabled((K)Convert.ChangeType(id, typeof(K)));
            } catch (Exception e) {
                Console.WriteLine("Could not convert string id to long: " + e);
                return null;
            }
        }

        // write
        public virtual void Create(E entity) {
            if (entity == null) {
                throw new ArgumentNullException("entity");
            }
            _entities.Add(entity);
        }

        public virtual void Disable(E entity) {
            if (entity == null) {
                throw new ArgumentNullException("entity");
            }
            entity.Enabled = false;
        }

        public virtual void Enable(E entity) {
            if (entity == null) {
                throw new ArgumentNullException("entity");
            }
            entity.Enabled = true;
        }

        public virtual void Delete(E entity) {
            if (entity == null) {
                throw new ArgumentNullException("entity");
            }
            _entities.Remove(entity);
        }

        // commit 
        public virtual void Commit() {
            _context.SaveChanges();
        }

        // write with auto commit
        public virtual void CreateAndCommit(E entity) {
            Create(entity);
            Commit();
        }

        public virtual void DisableAndCommit(E entity) {
            Disable(entity);
            Commit();
        }

        public virtual void EnableAndCommit(E entity) {
            Enable(entity);
            Commit();
        }

        public virtual void DeleteAndCommit(E entity) {
            Delete(entity);
            Commit();
        }
    }
}
