using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Client {
    using Model;

    public class ApiMessageResource : ApiResource<MessageModel> {

        private const string RESOURCE_NAME = "message";

        public ApiMessageResource(IApiClient parent, string version)
            : base(parent, version, RESOURCE_NAME) {
        }

        public ApiMessageResource(IApiClient parent)
            : base(parent, RESOURCE_NAME) {
        }
    }
}
