using System.ComponentModel.DataAnnotations;

namespace Contador.Web.Client.Models
{
	public class ExpenseModel
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public decimal Value { get; set; }

		public int CategoryId { get; set; }

		public string Description { get; set; }
	}
}
