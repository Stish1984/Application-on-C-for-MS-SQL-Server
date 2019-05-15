using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;


namespace Sales_registration
{
    public static class SaleQuery                      //Обработка динамического запроса полей таблицы
    {
        public static IQueryable  GroupByColumns(this IQueryable source,
        bool includeDate = false,
        bool includeOrg = false,
        bool includeCity = false,
        bool includeCountry=false,
        bool includeMan=false)
        {
            var columns = new List<string>();
            if (includeDate) columns.Add("Date");
            if (includeOrg) columns.Add("Organization");
            if (includeCity) columns.Add("City");
            if (includeCountry) columns.Add("Country");
            if (includeMan) columns.Add("Manager");
            try
            {
                return source
            .GroupBy($"new( {String.Join(",", columns)})", "it")
            .Select($"new(Key.{String.Join(",Key.", columns)},Sum(Quantity) as Quantity, Sum(Sum) as Sum)");
            }
            catch
            {
                return source;
            }
        }
    }
}
