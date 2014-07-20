using Bardock.Utils.Collections;
using Bardock.Utils.Extensions;
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
        public static IOrderedQueryable<T> Order<T, TOrder>(
            this IQueryable<T> query,
            ISortParams sortParams,
            Expression<Func<T, TOrder>> orderBy,
            SortDirs sortDir = SortDirs.Asc)
        {
            return query.Order(sortParams, Coll.Array(orderBy), Coll.Array(sortDir));
        }

        public static IOrderedQueryable<T> Order<T, TOrder>(
            this IQueryable<T> query,
            ISortParams sortParams,
            IEnumerable<Expression<Func<T, TOrder>>> ordersBy,
            SortDirs[] sortDirs = null,
            SortDirs defaultSortDir = SortDirs.Asc)
        {
            //Choose between defaults and selected orders
            string[] _ordersBy = null;
            SortDirs[] _sortDirs = null;
            if (sortParams != null && sortParams.SortBy != null && sortParams.SortBy.Any())
            {
                _ordersBy = sortParams.SortBy;
                _sortDirs = sortParams.SortDir;
            }
            else
            {
                _ordersBy = ordersBy.Select(o => ExpressionHelper.GetExpressionText(ExpressionHelper.RemoveConvert(o))).ToArray();
                _sortDirs = ordersBy.Select((o, i) => sortDirs != null && sortDirs.Where((x, ix) => i == ix).Any() ? sortDirs.Where((x, ix) => i == ix).FirstOrDefault() : defaultSortDir).ToArray();
            }

            //Apply Orders
            _ordersBy.ForEach((o, i) =>
            {
                if (i == 0)
                {
                    query = query.OrderByProperty(o, _sortDirs.ElementAt(i) == SortDirs.Asc);
                }
                else
                {
                    query = ((IOrderedQueryable<T>)query).ThenByProperty(o, _sortDirs.ElementAt(i) == SortDirs.Asc);
                }
            });

            return (IOrderedQueryable<T>)query;
        }

        public static PageData<T> Page<T>(this IQueryable<T> query, IPageParams pageParams)
        {

            //TODO: Check that query parameter is IOrderedQueryable
            //If not Thrown New ArgumentException()
            query = (IOrderedQueryable<T>)query;

            if (pageParams == null || pageParams.Length <= 0)
            {
                return new PageData<T>(new List<T>(), 0, 0);
            }

            pageParams.StartIndex = pageParams.StartIndex < 0 ? 0 : pageParams.StartIndex;
            var skip = pageParams.StartIndex >= 0 ? pageParams.StartIndex : 0;
            var take = pageParams.Length > 0 ? pageParams.Length : 25;
            return new PageData<T>(
                query
                    .Skip(skip)
                    .Take(take)
                    .ToList(),
                query.Count(),
                pageParams.StartIndex / pageParams.Length);
        }
    }
}
