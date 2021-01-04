
using Contador.Mobile.Styles;

using Xamarin.Forms;

namespace Contador.Mobile.Services
{
    public class CategoryAvatarService
    {
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
