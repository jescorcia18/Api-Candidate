using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPandape.Entity.UriServices;

namespace TestPandape.Entity.Pagination
{
    public class PaginationHelper
    {
        public static Paged<T> CreatePagedReponse<T>(List<T> data, Paginator validFilter, int totalRecords, IUriservice uriService, string route)
        {
            var respose = new Paged<T>(data, validFilter.PageNumber, validFilter.PageSize);
            var totalPages = ((double)totalRecords / (double)validFilter.PageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            respose.NextPage =
                (validFilter.PageNumber >= 1 && validFilter.PageNumber < roundedTotalPages)
                ?  uriService.GetPageUri(new Paginator(validFilter.PageNumber + 1, validFilter.PageSize), route)
                : null;
            respose.PreviousPage =
                validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= roundedTotalPages
                ? uriService.GetPageUri(new Paginator(validFilter.PageNumber - 1, validFilter.PageSize), route)
                : null;
            respose.FirstPage = uriService.GetPageUri(new Paginator(1, validFilter.PageSize), route);
            respose.LastPage = uriService.GetPageUri(new Paginator(roundedTotalPages, validFilter.PageSize), route);
            respose.TotalPages = roundedTotalPages;
            respose.TotalRecords = totalRecords;
            return respose;
        }
    }
}
