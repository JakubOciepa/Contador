using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Core.Common;
using Contador.Core.Models;
using Contador.DAL.Repositories;
using Contador.Web.Server.Services;

using FluentAssertions;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

namespace Server.Tests
{
    public class ExpenseServiceTests
    {
        private static IList<Contador.DAL.Models.Expense> _DALExpenses = new List<Contador.DAL.Models.Expense>()
        {
            new Contador.DAL.Models.Expense("Słodycze",123,0,1),
            new Contador.DAL.Models.Expense("Słodycze",123,0,1),
            new Contador.DAL.Models.Expense("Słodycze",123,0,1),
        };

        [Fact]
        public async void GetExpenses_WhenNoErrors_ReturnsCorrectListOfEpenses()
        {
            //arrange
            var expenseRepoMock = new Mock<IExpensesRepository>();
            var categoriesRepoMock = new Mock<IExpenseCategoryService>();
            var usersRepoMock = new Mock<IUserService>();
            var loggerMock = new Mock<ILogger<ExpenseService>>();

            expenseRepoMock.Setup(r => r.GetExpenses())
                .Returns(Task.FromResult(_DALExpenses));

            categoriesRepoMock.Setup(c => c.GetCategoryById(It.IsAny<int>()))
                .Returns(Task.FromResult(new Result<ExpenseCategory>(ResponseCode.Ok, new ExpenseCategory("Słodycze"))));

            usersRepoMock.Setup(u => u.GetUserById(It.IsAny<int>())).Returns(new User() { Name = "John" });

            var expenseService = new ExpenseService(expenseRepoMock.Object, categoriesRepoMock.Object,
                                        usersRepoMock.Object, loggerMock.Object);

            //act
            var expenses = await expenseService.GetExpenses();

            //assert
            expenses.ResponseCode.Should().Equals(ResponseCode.Ok);
            expenses.ReturnedObject.Count.Should().Equals(_DALExpenses.Count);
        }
    }
}

