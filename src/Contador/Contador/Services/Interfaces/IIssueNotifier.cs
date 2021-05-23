using System;

using Contador.Core.Models;

namespace Contador.Services.Interfaces
{
	/// <summary>
	/// Notify on Contador issues changes.
	/// </summary>
	public interface IIssueNotifier
	{
		/// <summary>
		/// Invoked when new(returned) issue has been added.
		/// </summary>
		event EventHandler<Issue> IssueAdded;

	}
}
