using System.Threading.Tasks;

using Contador.Core.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Contador.Mobile.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpenseControl : ContentView
    {
        private const int ANIMATION_LENGTH = 900;
        private bool _isCollapsed = true;
        private double _pageHeight;

        public static readonly BindableProperty ExpenseProperty
            = BindableProperty.Create(nameof(Expense), typeof(Expense), typeof(ExpenseControl));

        public Expense Expense
        {
            get => (Expense)GetValue(ExpenseProperty);
            set => SetValue(ExpenseProperty, value);
        }

        public ExpenseControl()
        {
            InitializeComponent();
            _pageHeight = Page.Height;
        }

        private async void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            await SwitchAvatarImage().ConfigureAwait(true);

            await Swipe.RotateXTo(360, ANIMATION_LENGTH, Easing.Linear);
            Swipe.RotationX = 0;
        }

        private async Task SwitchAvatarImage()
        {
            if (_isCollapsed)
            {
                AvatarImage.RotationY = -270;

                await CategoryImage.RotateYTo(-90, ANIMATION_LENGTH, Easing.SpringIn)
                    .ConfigureAwait(true);

                CategoryImage.IsVisible = false;
                AvatarImage.IsVisible = true;

                await AvatarImage.RotateYTo(-360, ANIMATION_LENGTH, Easing.SpringOut)
                    .ConfigureAwait(true);

                AvatarImage.RotationY = 0;
                _isCollapsed = false;
            }
            else
            {
                CategoryImage.RotationY = -270;

                await AvatarImage.RotateYTo(-90, ANIMATION_LENGTH, Easing.SpringIn)
                    .ConfigureAwait(true);

                AvatarImage.IsVisible = false;
                CategoryImage.IsVisible = true;

                await CategoryImage.RotateYTo(-360, ANIMATION_LENGTH, Easing.SpringOut)
                    .ConfigureAwait(true);

                CategoryImage.RotationY = 0;
                _isCollapsed = true;
            }
        }
    }
}
