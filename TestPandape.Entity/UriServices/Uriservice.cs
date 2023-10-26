using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPandape.Entity.Pagination;

namespace TestPandape.Entity.UriServices
{
    public class Uriservice: IUriservice
    {
        private readonly String _baseUri;
        public Uriservice(String baseUri)
        {
            _baseUri = baseUri;
        }
        public Uri GetPageUri(Paginator paginator, string route)
        {
            var _enpointUri = new Uri(string.Concat(_baseUri, route));
            var modifiedUri = QueryHelpers .AddQueryString(_enpointUri.ToString(), "pageNumber", paginator.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", paginator.PageSize.ToString());
            return  new Uri(modifiedUri);
        }
    }
}
