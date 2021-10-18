using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.Controllers.Responses
{
    public class PagedResponse<T>
    {
        public PagedResponse()
        {

        }

        public PagedResponse(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
