using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

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