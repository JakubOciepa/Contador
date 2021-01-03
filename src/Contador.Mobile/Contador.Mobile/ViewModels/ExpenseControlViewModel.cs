using Contador.Core.Models;
using Contador.Mobile.Styles;

using Xamarin.Forms;

namespace Contador.Mobile.ViewModels
{
    public class ExpenseControlViewModel : ViewModelBase
    {
        public Expense Expense { get; }

        //this should be taken from some service by the category name?
        public FontImageSource CategoryGlyph { get; } = new FontImageSource()
        {
            Glyph = FontAwesomeIcon.Solid.AppleAlt,
            Color = Color.White,
            FontFamily = "FontAwesome",
            Size = 20
        };

        //Same as above
        public FontImageSource UserGlyph { get; } = new FontImageSource()
        {
            Glyph = FontAwesomeIcon.Solid.Angry,
            Color = Color.White,
            FontFamily = "FontAwesome",
            Size = 27
        };

        public Color ExpenseColor { get; } = Color.Red;

        public ExpenseControlViewModel(Expense expense)
        {
            Expense = expense;
        }
    }
}
