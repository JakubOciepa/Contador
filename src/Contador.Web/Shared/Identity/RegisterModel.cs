using System.ComponentModel.DataAnnotations;

namespace Contador.Web.Identity
{
	/// <summary>
	/// Contains data required to create account.
	/// </summary>
	public class RegisterModel
	{
		/// <summary>
		/// Tha name of the user to register.
		/// </summary>
		[Required]
		[Display(Name = "Name")]
		public string Name { get; set; }

		/// <summary>
		/// The email of the user to register.
		/// </summary>
		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		/// <summary>
		/// The password of the user to create.
		/// </summary>
		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		/// <summary>
		/// The confirmation of the provided password.
		/// </summary>
		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}
}
