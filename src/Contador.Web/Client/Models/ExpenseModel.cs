using System.ComponentModel.DataAnnotations;

using Contador.Core.Models;

namespace Contador.Web.Client.Models
{
	public class ExpenseModel
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public decimal Value { get; set; }

		[Required]
		public ExpenseCategory Category { get; set; }

		public string Description { get; set; }
	}
}
