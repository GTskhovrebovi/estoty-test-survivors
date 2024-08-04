using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class StringExtensions
    {
        public static string Colorize(this string str, Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{str}</color>";
        }
    }

    public static class MiscExtensions
    {
        public static bool TryGetFirst<T>(this IEnumerable<T> list, Func<T, bool> predicate, out T result)
        {
            result = default(T); // Initialize the out parameter

            foreach (T item in list)
            {
                if (predicate(item))
                {
                    result = item;
                    return true;
                }
            }

            return false;
        }
    }
}