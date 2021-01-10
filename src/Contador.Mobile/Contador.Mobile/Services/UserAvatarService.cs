using Contador.Mobile.Styles;

using Xamarin.Forms;

namespace Contador.Mobile.Services
{
	/// <summary>
	/// Provides methods to get image for user avatar.
	/// </summary>
	public class UserAvatarService
	{
		/// <summary>
		/// Gets user avatar image by the user name.
		/// </summary>
		/// <param name="userName">User name for searching avatar image.</param>
		/// <returns>Avatar image.</returns>
		public ImageSource GetByUserName(string userName)
		{
			return new FontImageSource()
			{
				Glyph = FontAwesomeIcon.Solid.Angry,
				Color = Color.White,
				FontFamily = "FontAwesome",
				Size = 27
			};
		}
	}
}
