using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ChatApp.Client {
    using Model;

    public class ApiResource<M> where M : BaseModel {

        protected Postman _request;
        protected string _route;

        public ApiResource(Uri serverUrl, string resource) {
            _request = new Postman(serverUrl);
            _route = Path.Combine("api", "v1", resource);
        }
        
        public virtual async Task<M> Post(M model) {
            if (model.Id != null) {
                throw new ArgumentException("id of " + typeof(M).Name + " model should not have a value to Post." 
                    + " Try using Put or Save to save changes on an already created model.");
            }
            return await _request.Post(_route, model);
        }

        public virtual async Task<M> Get(string id) {
            if (id == null) {
                throw new ArgumentNullException("id should be a valid string to get a " + typeof(M).Name + " model.");
            }
            return await _request.Get<M>(Path.Combine(_route, id));
        }

        public virtual async Task<List<M>> Get() {
            return await _request.GetMany<M>(_route);
        }

        public virtual async Task<M> Put(M model) {
            if (model.Id == null) {
                throw new ArgumentException("id of " + typeof(M).Name + " model should have a value."
                    + " Try using Post or Save to create a new model first.");
            }
            return await _request.Post(Path.Combine(_route, model.Id), model);
        }

        public virtual async Task<M> Save(M model) {
            if (model.Id == null) {
                return await Post(model);
            } else {
                return await Put(model);
            }
        }
       
        public virtual async Task<bool> Delete(string id) {
            if (id == null) {
                throw new ArgumentNullException("id should be a valid string to delete a " +  typeof(M).Name + " model");
            }
            return await _request.Delete(Path.Combine(_route, id));
        }

        public virtual async Task<bool> Delete(M model) {
            return await Delete(model.Id);
        }
    }
}
