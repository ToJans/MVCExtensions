using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MvcExtensions.FNHModules.Searchable
{
    public static class IEnumerableExtensions 
    {
        static Regex MyRegex = new Regex(@"?(\-)[a-z0-9]+");

        public static IEnumerable<T> Search<T>(this IEnumerable<T> _this, params string[] Searchtext) where T : ISearchable
        {
            foreach(var m in MyRegex.Matches(string.Join(" ",Searchtext)).Cast<Match>().Select(x=>x.Value).Distinct())
            {
                if (m.StartsWith("-"))
                {
                    _this = from t in _this 
                        where ! t.SearchText.Contains(" " + m.Substring(1) + " ")
                        select t;
                }
                else
                {
                _this = from t in _this 
                        where t.SearchText.Contains(" " + m + " ")
                        select t;
                }
           }
            return _this;
        }
    }
}
