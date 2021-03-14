
using Contador.Core.Models;

using Microsoft.AspNetCore.Components;

namespace Contador.Web.Client.Components
{
	public partial class ExpenseComponent
	{
		[Parameter]
		public Expense Expense { get; set; }
	}
}
