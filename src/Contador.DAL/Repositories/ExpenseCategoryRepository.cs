using Contador.Core.Models;
using Contador.Core.Utils;
using Contador.DAL.DbContext;

namespace Contador.DAL.Repositories
{
    public class ExpenseCategoryRepository
    {
        private readonly ContadorContext _db;

        public ExpenseCategoryRepository(ContadorContext context)
        {
            _db = context;
        }

        public string GetCategoryNameById(int id)
        {
            var category = _db.Set<ExpenseCategory>().Find(id);

            if (category is ExpenseCategory)
                return category.Name;

            return Messages.NotFound;
        }
    }
}
