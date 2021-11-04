using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.Controllers.Responses
{
    public class Response<T>
    {
        public Response()
        {

        }

        public Response(T response)
        {
            Data = response;
        }

        public T Data { get; set; }
    }
}
