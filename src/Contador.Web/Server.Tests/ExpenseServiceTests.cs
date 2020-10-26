using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Core.Common;
using Contador.Core.Models;
using Contador.Core.Utils.Extensions;
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
        private static readonly User _expectedUser = new User() { Email = "john@domain.com", Id = 0, Name = "John" };
        private static readonly ExpenseCategory _expectedCategory = new ExpenseCategory("Słodycze") { Id = 0 };

        private static readonly IList<Contador.DAL.Models.Expense> _returnedExpenses = new List<Contador.DAL.Models.Expense>
        {
            new Contador.DAL.Models.Expense("Słodycze",123,0,1),
            new Contador.DAL.Models.Expense("Słodycze",123,0,1),
            new Contador.DAL.Models.Expense("Słodycze",123,0,1),
        };

        private static readonly List<Contador.Core.Models.Expense> _expextedExpenses = new List<Contador.Core.Models.Expense>
        {
            new Expense("Słodycze", 123 ,_expectedUser, _expectedCategory),
            new Expense("Słodycze", 123 ,_expectedUser, _expectedCategory),
            new Expense("Słodycze", 123 ,_expectedUser, _expectedCategory),
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
                .Returns(Task.FromResult(_returnedExpenses));

            categoriesRepoMock.Setup(c => c.GetCategoryById(It.IsAny<int>()))
                .Returns(Task.FromResult(new Result<ExpenseCategory>(ResponseCode.Ok, _expectedCategory)));

            usersRepoMock.Setup(u => u.GetUserById(It.IsAny<int>())).Returns(_expectedUser);

            var expenseService = new ExpenseService(expenseRepoMock.Object, categoriesRepoMock.Object,
                                        usersRepoMock.Object, loggerMock.Object);

            //act
            var expenses = await expenseService.GetExpenses().CAF();

            //assert
            expenses.ResponseCode.Should().Equals(ResponseCode.Ok);
            expenses.ReturnedObject.Count.Should().Equals(_returnedExpenses.Count);
            expenses.ReturnedObject.Should().BeEquivalentTo(_expextedExpenses);
        }

        [Fact]
        public async void GetExpenses_WhenNoExpensesAvailable_ReturnsNotFound()
        {
            //arrange
            var expenseRepoMock = new Mock<IExpensesRepository>();
            var categoriesRepoMock = new Mock<IExpenseCategoryService>();
            var usersRepoMock = new Mock<IUserService>();
            var loggerMock = new Mock<ILogger<ExpenseService>>();

            expenseRepoMock.Setup(r => r.GetExpenses())
                .Returns(Task.FromResult(new List<Contador.DAL.Models.Expense>() as IList<Contador.DAL.Models.Expense>));

            categoriesRepoMock.Setup(c => c.GetCategoryById(It.IsAny<int>()))
                .Returns(Task.FromResult(new Result<ExpenseCategory>(ResponseCode.Ok, _expectedCategory)));

            usersRepoMock.Setup(u => u.GetUserById(It.IsAny<int>())).Returns(_expectedUser);

            var expenseService = new ExpenseService(expenseRepoMock.Object, categoriesRepoMock.Object,
                                        usersRepoMock.Object, loggerMock.Object);

            //act
            var expenses = await expenseService.GetExpenses().CAF();

            //assert
            expenses.ResponseCode.Should().Equals(ResponseCode.NotFound);
        }
    }
}

