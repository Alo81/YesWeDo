﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SmashUltimateEditor.FighterDataTbl;

namespace SmashUltimateEditor
{
    public static class Extensions
    {
        public class EnumUtil<T> where T : struct, IConvertible
        {
            public static bool Contains(string value)
            {
                if (!typeof(T).IsEnum)
                    throw new ArgumentException("T must be an enumerated type");

                return Enum.IsDefined(typeof(T), value);
            }
            public static List<string> GetValuesSorted()
            {
                if (!typeof(T).IsEnum)
                    throw new ArgumentException("T must be an enumerated type");

                var sorted = (T[]) Enum.GetValues(typeof(T));
                return sorted.OrderBy(x => x.ToString()).Select(x => x.ToString()).ToList();

            }
        }
    }
}
