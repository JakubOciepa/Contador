﻿using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Core.Common;
using Contador.Core.Models;
using Contador.Core.Utils.Extensions;
using Contador.DAL.Abstractions;
using Contador.Services;
using Contador.Services.Interfaces;

using FluentAssertions;

using Moq;

using Xunit;

namespace Server.Tests
{
	public class ExpenseServiceTests
	{
		private static readonly User _expectedUser = new User() { Email = "john@domain.com", Id = "0", UserName = "John" };
		private static readonly ExpenseCategory _expectedCategory = new ExpenseCategory("Słodycze") { Id = 0 };

		private static readonly IList<Expense> _returnedExpenses = new List<Expense>
		{
			new Expense("Słodycze",123,_expectedUser,_expectedCategory),
			new Expense("Słodycze",123,_expectedUser,_expectedCategory),
			new Expense("Słodycze",123,_expectedUser,_expectedCategory),
		};

		private static readonly List<Expense> _expectedExpenses = new List<Expense>
		{
			new Expense("Słodycze", 123 ,_expectedUser, _expectedCategory),
			new Expense("Słodycze", 123 ,_expectedUser, _expectedCategory),
			new Expense("Słodycze", 123 ,_expectedUser, _expectedCategory),
		};

		[Fact]
		public async void GetExpenses_WhenNoErrors_ReturnsCorrectListOfEpenses()
		{
			//arrange
			var expenseRepoMock = new Mock<IExpenseRepository>();
			var categoriesRepoMock = new Mock<IExpenseCategoryService>();
			var usersRepoMock = new Mock<IUserService>();
			var loggerMock = new Mock<ILog>();

			expenseRepoMock.Setup(r => r.GetAllAsync())
				.Returns(Task.FromResult(_returnedExpenses));

			categoriesRepoMock.Setup(c => c.GetByIdAsync(It.IsAny<int>()))
				.Returns(Task.FromResult(new Result<ExpenseCategory>(ResponseCode.Ok, _expectedCategory)));

			usersRepoMock.Setup(u => u.GetUserById(It.IsAny<string>())).Returns(_expectedUser);

			var expenseService = new ExpenseManager(expenseRepoMock.Object, loggerMock.Object);

			//act
			var result = await expenseService.GetAllAsync().CAF();

			//assert
			result.ResponseCode.Should().Equals(ResponseCode.Ok);
			result.ReturnedObject.Count.Should().Equals(_returnedExpenses.Count);
			result.ReturnedObject.Should().BeEquivalentTo(_expectedExpenses);
		}

		[Fact]
		public async void GetExpenses_WhenNoExpensesAvailable_ReturnsNotFound()
		{
			//arrange
			var expenseRepoMock = new Mock<IExpenseRepository>();
			var categoriesRepoMock = new Mock<IExpenseCategoryService>();
			var usersRepoMock = new Mock<IUserService>();
			var loggerMock = new Mock<ILog>();

			expenseRepoMock.Setup(r => r.GetAllAsync())
				.Returns(Task.FromResult(new List<Expense>() as IList<Expense>));

			categoriesRepoMock.Setup(c => c.GetByIdAsync(It.IsAny<int>()))
				.Returns(Task.FromResult(new Result<ExpenseCategory>(ResponseCode.Ok, _expectedCategory)));

			usersRepoMock.Setup(u => u.GetUserById(It.IsAny<string>())).Returns(_expectedUser);

			var expenseService = new ExpenseManager(expenseRepoMock.Object, loggerMock.Object);

			//act
			var result = await expenseService.GetAllAsync().CAF();

			//assert
			result.ResponseCode.Should().Equals(ResponseCode.NotFound);
			result.ReturnedObject.Count.Should().Be(0);
		}

		[Fact]
		public async void GetExpenseById_WhenExpenseExists_ReturnsOkAndExpense()
		{
			//arrange
			var expenseRepoMock = new Mock<IExpenseRepository>();
			var categoriesRepoMock = new Mock<IExpenseCategoryService>();
			var usersRepoMock = new Mock<IUserService>();
			var loggerMock = new Mock<ILog>();

			expenseRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
				.Returns(Task.FromResult(_returnedExpenses[1]));

			categoriesRepoMock.Setup(c => c.GetByIdAsync(It.IsAny<int>()))
				.Returns(Task.FromResult(new Result<ExpenseCategory>(ResponseCode.Ok, _expectedCategory)));

			usersRepoMock.Setup(u => u.GetUserById(It.IsAny<string>())).Returns(_expectedUser);

			var expenseService = new ExpenseManager(expenseRepoMock.Object, loggerMock.Object);
			//act
			var result = await expenseService.GetByIdAsync(0).CAF();

			//assert
			result.ResponseCode.Should().BeEquivalentTo(ResponseCode.Ok);
			result.ReturnedObject.Should().BeEquivalentTo(_expectedExpenses[1]);
		}

		[Fact]
		public async void GetExpenseById_WhenExpenseDoesNotExist_ReturnsNotFoundAndDefault()
		{
			//arrange
			var expenseRepoMock = new Mock<IExpenseRepository>();
			var categoriesRepoMock = new Mock<IExpenseCategoryService>();
			var usersRepoMock = new Mock<IUserService>();
			var loggerMock = new Mock<ILog>();

			expenseRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
				.Returns(Task.FromResult(default(Expense)));

			categoriesRepoMock.Setup(c => c.GetByIdAsync(It.IsAny<int>()))
				.Returns(Task.FromResult(new Result<ExpenseCategory>(ResponseCode.Ok, _expectedCategory)));

			usersRepoMock.Setup(u => u.GetUserById(It.IsAny<string>())).Returns(_expectedUser);

			var expenseService = new ExpenseManager(expenseRepoMock.Object, loggerMock.Object);
			//act
			var result = await expenseService.GetByIdAsync(0).CAF();

			//assert
			result.ResponseCode.Should().BeEquivalentTo(ResponseCode.NotFound);
			result.ReturnedObject.Should().BeEquivalentTo(default(Expense));
		}

		[Fact]
		public async void Update_WhenCannotUpdate_ReturnsErrorAndDefault()
		{
			//arrange
			var expenseRepoMock = new Mock<IExpenseRepository>();
			var categoriesRepoMock = new Mock<IExpenseCategoryService>();
			var usersRepoMock = new Mock<IUserService>();
			var loggerMock = new Mock<ILog>();

			expenseRepoMock.Setup(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<Expense>()))
				.Returns(Task.FromResult(default(Expense)));

			categoriesRepoMock.Setup(c => c.GetByIdAsync(It.IsAny<int>()))
				.Returns(Task.FromResult(new Result<ExpenseCategory>(ResponseCode.Ok, _expectedCategory)));

			usersRepoMock.Setup(u => u.GetUserById(It.IsAny<string>())).Returns(_expectedUser);

			var expenseService = new ExpenseManager(expenseRepoMock.Object, loggerMock.Object);
			//act
			var result = await expenseService.UpdateAsync(0, _expectedExpenses[0]).CAF();

			//assert
			result.ResponseCode.Should().BeEquivalentTo(ResponseCode.Error);
			result.ReturnedObject.Should().BeEquivalentTo(default(Expense));
		}

		[Fact]
		public async void Update_WhenExpenseHasBeenUpdated_ReturnsOkAndDefault()
		{
			//arrange
			var expenseRepoMock = new Mock<IExpenseRepository>();
			var categoriesRepoMock = new Mock<IExpenseCategoryService>();
			var usersRepoMock = new Mock<IUserService>();
			var loggerMock = new Mock<ILog>();

			var expectedDescription = "Here is a new description!!!";

			var updatedExpense = _returnedExpenses[0];
			updatedExpense.Description = expectedDescription;

			var expectedExpenes = _expectedExpenses[0];
			expectedExpenes.Description = expectedDescription;

			expenseRepoMock.Setup(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<Expense>()))
				.Returns(Task.FromResult(updatedExpense));

			categoriesRepoMock.Setup(c => c.GetByIdAsync(It.IsAny<int>()))
				.Returns(Task.FromResult(new Result<ExpenseCategory>(ResponseCode.Ok, _expectedCategory)));

			usersRepoMock.Setup(u => u.GetUserById(It.IsAny<string>())).Returns(_expectedUser);

			var expenseService = new ExpenseManager(expenseRepoMock.Object, loggerMock.Object);
			//act
			var result = await expenseService.UpdateAsync(0, _expectedExpenses[0]).CAF();

			//assert
			result.ResponseCode.Should().BeEquivalentTo(ResponseCode.Ok);
			result.ReturnedObject.Should().BeEquivalentTo(expectedExpenes);
		}
	}
}
