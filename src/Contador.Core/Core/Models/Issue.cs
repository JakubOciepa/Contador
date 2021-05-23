using System;

namespace Contador.Core.Models
{
	/// <summary>
	/// Contains information about Contador issue.
	/// </summary>
	public class Issue
	{

		/// <summary>
		/// Gets or sets the id of the Contador issue.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the Contador issue.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the create date of the Contador issue.
		/// </summary>
		public DateTime CreatedDate { get; set; }

		/// <summary>
		/// Gets or sets the value that indicates if the Contador issue is open.
		/// </summary>
		public bool IsOpen { get; set; }
	}
}
