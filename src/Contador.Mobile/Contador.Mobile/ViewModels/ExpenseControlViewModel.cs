
using Contador.Core.Models;
using Contador.Mobile.Services;

using Xamarin.Forms;

namespace Contador.Mobile.ViewModels
{
    public class ExpenseControlViewModel : ViewModelBase
    {
        private readonly CategoryAvatarService _categoryAvatarService;
        private readonly UserAvatarService _userAvatarService;

        public Expense Expense { get; }

        //this should be taken from some service by the category name?
        public FontImageSource CategoryGlyph { get; private set; }

        //Same as above
        public FontImageSource UserGlyph { get; private set; }

        public Color ExpenseColor { get; } = Color.Red;

        public ExpenseControlViewModel(Expense expense)
        {
            Expense = expense;
            _categoryAvatarService = new CategoryAvatarService();
            _userAvatarService = new UserAvatarService();

            InitializeProperties();
        }

        private void InitializeProperties()
        {
            CategoryGlyph = _categoryAvatarService.GetByCategoryName(Expense.Category.Name);
            UserGlyph = _categoryAvatarService.GetByCategoryName(Expense.User.Name);
        }
    }
}
