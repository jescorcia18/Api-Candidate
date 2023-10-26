using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPandape.Entity.Pagination;

namespace TestPandape.Entity.UriServices
{
    public interface IUriservice
    {
        Uri GetPageUri(Paginator paginator, string route);
    }
}
