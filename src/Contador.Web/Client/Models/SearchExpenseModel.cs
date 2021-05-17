using System;

namespace Contador.Web.Client.Models
{
	public class SearchExpenseModel
	{
		public string Name { get; set; }
		public string CategoryName { get; set; }
		public string UserName { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
	}
}
