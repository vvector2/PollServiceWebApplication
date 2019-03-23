using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApplicatationPollService.Models.ViewModels;

namespace WebApplicatationPollService.Models {
    public class PaginationHandler<T> where T : class {
        Expression<Func<T, object>> SortFunction;
        Expression<Func<T, bool>> FilterFunction;
        public PaginationHandler(Expression<Func<T, object>> _SortFunction, Expression<Func<T, bool>> _FilterFunction) {
            SortFunction = _SortFunction;
            FilterFunction = _FilterFunction;
        }
        public IEnumerable<T> GetEntityFromFilterOption(FilterOptionModelView filterModelOption, ApplicationDbContext db) {
            IQueryable<T> query;
            if (filterModelOption.phrase != null)//filter using phrase
                query = db.Set<T>().Where(FilterFunction);
            else query = db.Set<T>();

            if (filterModelOption.orderMode)//sort element
                query = query.OrderBy(SortFunction);
            else query = query.OrderByDescending(SortFunction);

            return query.Skip((filterModelOption.page - 1) * filterModelOption.elements).Take(filterModelOption.elements);
        }
    }
}