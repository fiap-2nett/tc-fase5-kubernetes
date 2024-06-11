using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HelpDesk.AppService.Web.Extensions
{
    public static class EnumerableExtensions
    {
        #region Extension Methods

        public static IEnumerable<SelectListItem> ToSelectList<T>(this IEnumerable<T> enumerable,
            Func<T, string> textSelector,
            Func<T, string> valueSelector)
        {
            if (enumerable.IsNullOrEmpty())
                yield break;

            foreach (var item in enumerable)
            {
                yield return new SelectListItem
                {
                    Text = textSelector(item),
                    Value = valueSelector(item)
                };
            }
        }

        public static IEnumerable<SelectListItem> ToSelectList<T>(this IEnumerable<T> enumerable,
            Func<T, string> textSelector,
            Func<T, string> valueSelector,
            Func<T, bool> isSelectedSelector)
        {
            if (enumerable.IsNullOrEmpty())
                yield break;

            foreach (var item in enumerable)
            {
                yield return new SelectListItem
                {
                    Text = textSelector(item),
                    Value = valueSelector(item),
                    Selected = isSelectedSelector(item)
                };
            }
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
            => !enumerable?.Any() ?? true;

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T element in enumerable)
                action(element);
        }

        #endregion
    }
}
