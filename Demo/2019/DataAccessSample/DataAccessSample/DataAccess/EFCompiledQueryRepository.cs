using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessSample.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessSample
{
    public struct EFCompiledQueryRepository : IDataSource
    {
        private static readonly Func<NorthwindContext, string, IEnumerable<Products>> _createQuery =
            EF.CompileQuery<NorthwindContext, string, IEnumerable<Products>>((db, categoryName)
                => db.Products
                .Where(b => b.Category.CategoryName == categoryName)
                .OrderBy(b => b.ProductName)
                .Select(x => x)
                );

        public Products[] GetAllProductsByCategory(NorthwindContext db, string categoryName)
        {
            return _createQuery(db, categoryName)
                .ToArray();
        }
    }
}
