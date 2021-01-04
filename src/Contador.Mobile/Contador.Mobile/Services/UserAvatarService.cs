
using Contador.Mobile.Styles;

using Xamarin.Forms;

namespace Contador.Mobile.Services
{
    public class UserAvatarService
    {
        public FontImageSource GetByUserName(string UserName)
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
