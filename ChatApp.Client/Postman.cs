using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;

namespace ChatApp.Client {
    using Common;
    /// <summary>
    /// A basic HTTP client CRUD implementation on top of System.Net.Http.
    /// 
    /// M: request/response body model
    /// REQ_M: request body model
    /// RES_M: response body model
    /// </summary>
    public class Postman {

        private HttpClient client;

        public Postman(Uri serverUrl) {
            client = new HttpClient();
            client.BaseAddress = serverUrl;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        
        public void SetAuthorizationBearer(string token) {
            if (token != null) {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            } else {
                client.DefaultRequestHeaders.Authorization = null;
            }
        }
        public void CleanAuthorizationBearer() {
            SetAuthorizationBearer(null);
        }

        public async Task<RES_M> Post<REQ_M, RES_M>(string route, REQ_M model) where REQ_M: class where RES_M: class {
            string reqJson = JsonSerializer.Serialize(model);
            HttpResponseMessage response = await client.PostAsync(route, new StringContent(reqJson, Encoding.UTF8, "application/json"));

            RES_M responseModel = null;
            if (response.IsSuccessStatusCode) {
                string resJson = await response.Content.ReadAsStringAsync();
                responseModel = JsonSerializer.Deserialize<RES_M>(resJson);
            }

            return responseModel;
        }

        public async Task<M> Post<M>(string route, M model) where M : class {
            return await Post<M, M>(route, model);
        }

        public async Task<M> Get<M>(string route) where M : class {
            HttpResponseMessage response = await client.GetAsync(route);

            M responseModel = null;
            if (response.IsSuccessStatusCode) {
                string resJson = await response.Content.ReadAsStringAsync();
                responseModel = JsonSerializer.Deserialize<M>(resJson);
            }

            return responseModel;
        }

        public async Task<List<M>> GetMany<M>(string route) where M : class {
            HttpResponseMessage response = await client.GetAsync(route);

            List<M> responseModels = null;
            if (response.IsSuccessStatusCode) {
                string resJson = await response.Content.ReadAsStringAsync();
                responseModels = JsonSerializer.Deserialize<List<M>>(resJson);
            }

            return responseModels;
        }

        public async Task<RES_M> Put<REQ_M, RES_M>(string route, REQ_M model) where REQ_M : class where RES_M : class {
            string reqJson = JsonSerializer.Serialize(model);
            HttpResponseMessage response = await client.PutAsync(route, new StringContent(reqJson, Encoding.UTF8, "application/json"));

            RES_M responseModel = null;
            if (response.IsSuccessStatusCode) {
                string resJson = await response.Content.ReadAsStringAsync();
                responseModel = JsonSerializer.Deserialize<RES_M>(resJson);
            }

            return responseModel;
        }

        public async Task<M> Put<M>(string route, M model) where M : class {
            return await Put<M, M>(route, model);
        }

        public async Task<bool> Delete(string route) {
            HttpResponseMessage response = await client.DeleteAsync(route);

            return response.IsSuccessStatusCode;
        }

        public async Task<M> Delete<M>(string route) where M : class {
            HttpResponseMessage response = await client.DeleteAsync(route);

            M responseModel = null;

            if (response.IsSuccessStatusCode) {
                string resJson = await response.Content.ReadAsStringAsync();
                responseModel = JsonSerializer.Deserialize<M>(resJson);
            }

            return responseModel;
        }
    }
}
