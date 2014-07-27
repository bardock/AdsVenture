using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace AdsVenture.Data.Helpers.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Include<T, TProperty>(
            this IQueryable<T> source, 
            Expression<Func<T, TProperty>> path,
            bool when)
        {
            if (!when)
                return source;
            return source.Include(path);
        }
    }
}
