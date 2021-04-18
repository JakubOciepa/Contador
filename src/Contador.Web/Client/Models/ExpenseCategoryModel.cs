using System.ComponentModel.DataAnnotations;

namespace Contador.Web.Client.Models
{
	public class ExpenseCategoryModel
	{
		[Required]
		public string Name { get; set; }
	}
}
