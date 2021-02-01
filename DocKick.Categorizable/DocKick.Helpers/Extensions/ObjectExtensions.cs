using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DocKick.Helpers.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsEmpty<T>(this T value)
        {
            if (EqualityComparer<T>.Default.Equals(value, default))
            {
                return true;
            }

            switch (value)
            {
                case string str when string.IsNullOrWhiteSpace(str):
                    return true;
                case ICollection { Count: 0 }:
                case Array { Length: 0 }:
                case IEnumerable e when !e.Cast<object>()
                                          .Any():
                    return true;
            }

            return false;
        }
    }
}