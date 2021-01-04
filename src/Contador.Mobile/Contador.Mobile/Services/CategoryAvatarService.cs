
using Contador.Mobile.Styles;

using Xamarin.Forms;

namespace Contador.Mobile.Services
{
    /// <summary>
    /// Provides methods to get category avatars.
    /// </summary>
    public class CategoryAvatarService
    {
        /// <summary>
        /// Gets category avatar image by the category name.
        /// </summary>
        /// <param name="categoryName">Category name for searching avatar.</param>
        /// <returns>Avatar source.</returns>
        public FontImageSource GetByCategoryName(string categoryName)
        {
            return new FontImageSource()
            {
                Glyph = FontAwesomeIcon.Solid.AppleAlt,
                Color = Color.White,
                FontFamily = "FontAwesome",
                Size = 20
            };
        }
    }
}
