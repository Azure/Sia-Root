using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sia.Shared.Protocol.Pagination
{
    public interface IPaginationRequest<TSource, TDestination>
    {
        Task<IPaginationResultMetadata<TDestination>> GetResultAsync(IQueryable<TSource> source);
    }
}
