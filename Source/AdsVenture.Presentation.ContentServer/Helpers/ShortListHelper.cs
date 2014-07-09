using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace AdsVenture.Presentation.ContentServer.Helpers
{
    public static class ShortListHelper
    {
        public static IEnumerable<string> FilterShortList<TModel>(
            this IEnumerable<TModel> list, 
            Func<TModel, string> valueFunc, 
            string query, 
            Int32 items)
        {
            return list
                .AsParallel()
                .Select(x => valueFunc(x))
                .Where(x => x != null && x.ToLower().Trim().Contains(query.ToLower().Trim()))
                .Take(items)
                .ToList();
        }

        public static IEnumerable<Models.Shared.ShortListItem> FilterShortList<TModel>(
            this IEnumerable<TModel> list, 
            Func<TModel, Models.Shared.ShortListItem> transformation, 
            Int32 items, 
            string query, 
            params Func<TModel, string>[] fieldSelectors)
        {
            return list
                .AsParallel()
                .Where(x => fieldSelectors.Any(f => f(x).ToLower().Trim().Contains(query.ToLower().Trim())))
                .Select(x => transformation(x))
                .Take(items)
                .ToList();
        }

    }
}