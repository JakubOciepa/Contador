using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Core.Common;
using Contador.Core.Models;
using Contador.Core.Utils.Extensions;
using Contador.DAL;
using Contador.Services.Interfaces;

namespace Contador.Services
{
	/// <summary>
	/// Provides mechanism to manage Contador issues.
	/// </summary>
	public class IssueManager : IIssueManager
	{
		private readonly IIssueRepository _repository;
		private readonly ILog _logger;
		/// <summary>
		/// Invoked when new(returned) issue has been added.
		/// </summary>
		public event EventHandler<Issue> IssueAdded;

		/// <summary>
		/// Creates an instance of the <see cref="IssueManager"/> class.
		/// </summary>
		/// <param name="repository">Issue storage repository.</param>
		/// <param name="logger">Logger</param>
		public IssueManager(IIssueRepository repository, ILog logger)
		{
			_repository = repository;
			_logger = logger;
		}

		/// <summary>
		/// Add the Contador issue.
		/// </summary>
		/// <param name="issue">Information about issue to add.</param>
		/// <returns>Added issue.</returns>
		public async Task<Result<Issue>> AddAsync(Issue issue)
		{
			Issue addedIssue;

			try
			{
				addedIssue = await _repository.AddAsync(issue).CAF();
			}
			catch (Exception ex)
			{
				_logger.Write(LogLevel.Error, $"{ex.Message}\n{ex.StackTrace}");

				return new Result<Issue>(ResponseCode.Error, null) { Message = ex.Message };
			}

			if (addedIssue is null)
			{
				var error = "Cannot add the category";
				_logger.Write(LogLevel.Warning, error);

				return new Result<Issue>(ResponseCode.Error, null) { Message = error };
			}

			IssueAdded?.Invoke(this, addedIssue);

			return new Result<Issue>(ResponseCode.Ok, addedIssue);
		}

		/// <summary>
		/// Gets all created issues.
		/// </summary>
		/// <returns>List of available issues and correct code.</returns>
		public async Task<Result<IList<Issue>>> GetAllAsync()
		{
			IList<Issue> issues;

			try
			{
				issues = await _repository.GetAllAsync().CAF() as List<Issue>;
			}
			catch (Exception ex)
			{
				_logger.Write(LogLevel.Error, $"{ex}");

				return new Result<IList<Issue>>(ResponseCode.Error, new List<Issue>()) { Message = $"{ex}" };
			}

			if (issues?.Count is 0)
			{
				_logger.Write(LogLevel.Warning, "Categories not found.");

				return new Result<IList<Issue>>(ResponseCode.NotFound, new List<Issue>());
			}

			return new Result<IList<Issue>>(ResponseCode.Ok, issues);
		}
	}
}
