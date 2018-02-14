using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class HandlerPipelineHelpers
    {
        public static async Task<T> FirstOrNotFoundAsync<T>(this DbSet<T> table, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
            where T : class
        {

        }
    }
}
