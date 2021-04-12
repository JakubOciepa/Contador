using System.ComponentModel.DataAnnotations;

namespace Contador.Web.Identity
{
	public class LoginModel
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
