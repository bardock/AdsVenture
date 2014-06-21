using Bardock.Utils.Collections;
using Bardock.Utils.Linq.Expressions;
using AdsVenture.Commons.Pagination.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdsVenture.Commons.Pagination.Extensions
{
    public static class IQueryableExtensions
    {
        private static readonly MethodInfo _OrderByMethod = typeof(Queryable).GetMethods().Where(method => method.Name == "OrderBy").Where(method => method.GetParameters().Length == 2).Single();
        private static readonly MethodInfo _OrderByDescendingMethod = typeof(Queryable).GetMethods().Where(method => method.Name == "OrderByDescending").Where(method => method.GetParameters().Length == 2).Single();
        private static readonly MethodInfo _ThenByMethod = typeof(Queryable).GetMethods().Where(method => method.Name == "ThenBy").Where(method => method.GetParameters().Length == 2).Single();
        private static readonly MethodInfo _ThenByDescendingMethod = typeof(Queryable).GetMethods().Where(method => method.Name == "ThenByDescending").Where(method => method.GetParameters().Length == 2).Single();

        private static IOrderedQueryable<TSource> OrderByProperty<TSource>(this IQueryable<TSource> source, string propertyExpression, bool @ascending = true)
        {
	        var lambda = ExpressionHelper.ParseProperties<TSource>(propertyExpression);
	        var orderByMethod = @ascending ? _OrderByMethod : _OrderByDescendingMethod;
	        var genericMethod = orderByMethod.MakeGenericMethod(typeof(TSource),lambda.Body.Type);
            var ret = genericMethod.Invoke(null, new object[] { source, lambda });
	        return (IOrderedQueryable<TSource>)ret;
        }

        private static IOrderedQueryable<TSource> ThenByProperty<TSource>(this IOrderedQueryable<TSource> source, string propertyExpression, bool @ascending = true)
        {
            var lambda = ExpressionHelper.ParseProperties<TSource>(propertyExpression);
            var orderByMethod = @ascending ? _ThenByMethod : _ThenByDescendingMethod;
            var genericMethod = orderByMethod.MakeGenericMethod(typeof(TSource), lambda.Body.Type);
            var ret = genericMethod.Invoke(null, new object[] { source, lambda });
	        return (IOrderedQueryable<TSource>)ret;
        }

        public static IOrderedQueryable<T> Order<T, TOrder>(this IQueryable<T> query, ISortParams sortParams, Expression<Func<T, TOrder>> orderBy, SortDirection sortDir = SortDirection.Asc)
        {
            return query.Order(
                sortParams, 
                Coll.Array(orderBy), 
                Coll.Array(sortDir)
            );
        }

        public static IOrderedQueryable<TSource> Order<TSource, TOrder>(this IQueryable<TSource> query, ISortParams sortParams, IEnumerable<Expression<Func<TSource, TOrder>>> ordersBy, SortDirection[] sortDirs = null, SortDirection defaultSortDir = SortDirection.Asc)
        {
	        //Choose between defaults and selected orders
	        string[] _ordersBy = null;
	        SortDirection[] _sortDirs = null;
	        if ((sortParams != null && sortParams.SortBy != null && sortParams.SortBy.Any())) {
		        _ordersBy = sortParams.SortBy;
		        _sortDirs = sortParams.SortDir;
	        } else {
		        _ordersBy = ordersBy.Select(o => ExpressionHelper.GetExpressionText(ExpressionHelper.RemoveConvert(o))).ToArray();
		        _sortDirs = ordersBy.Select((o, i) => sortDirs != null && sortDirs.Where((x, ix) => i == ix).Any() 
                                        ? sortDirs.Where((x, ix) => i == ix).FirstOrDefault() 
                                        : defaultSortDir).ToArray();
	        }

	        //Apply Orders
            int pos = 0;
            foreach (var o in _ordersBy)
            {
                if (pos == 0)
                    query = query.OrderByProperty(o, _sortDirs.ElementAt(pos) == SortDirection.Asc);
                else
                    query = ((IOrderedQueryable<TSource>)query).ThenByProperty(o, _sortDirs.ElementAt(pos) == SortDirection.Asc);
            }

            return (IOrderedQueryable<TSource>)query;
        }

        public static IOrderedQueryable<TSource> AsOrdered<TSource>(this IQueryable<TSource> query)
        {
            return (IOrderedQueryable<TSource>)query;
        }

        public static PageData<TSource> Page<TSource>(this IOrderedQueryable<TSource> query, IPageParams pageParams)
        {
            if (pageParams == null)
                throw new ArgumentException("pageParams");

            pageParams.Offset = pageParams.Offset < 0 ? 0 : pageParams.Offset;
            pageParams.Limit = pageParams.Limit > 0 ? pageParams.Limit : 25;

            return new PageData<TSource>(
                query.Skip(pageParams.Offset).Take(pageParams.Limit).ToList(), 
                query.Count(),
                pageParams.Offset, 
                pageParams.Limit
            );
        }
    }
}
