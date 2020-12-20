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
        private bool _toggling = true;
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
            DescriptionText.IsVisible = false;
            AvatarImage.IsVisible = false;
            UntoggledValue.IsVisible = false;
            TopBar.IsVisible = false;
            CategoryName.TranslationY = -50;
        }

        private async void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            CancelAnimations();
            _toggling = !_toggling;

            if (_toggling)
            {
                await ToggleCategory().ConfigureAwait(true);

                await ToggleContent().ConfigureAwait(true);
            }
            else
            {
                await ToggleContent().ConfigureAwait(true);

                await ToggleCategory().ConfigureAwait(true);
            }

            Shadow.RotationX = 0;
        }

        private void CancelAnimations()
        {
            Shadow.CancelAnimations();
            CategoryImage.CancelAnimations();
            AvatarImage.CancelAnimations();
        }

        private async Task ToggleContent()
        {
            if (!_toggling)
                _pageHeight = Control.Height;

            await Shadow.RotateXTo(90, ANIMATION_LENGTH / 5, Easing.Linear).ConfigureAwait(true);

            Swipe.HeightRequest = _toggling ? _pageHeight : _pageHeight * 1.8;

            UntoggledValue.IsVisible = !_toggling;
            DescriptionText.IsVisible = !_toggling;
            TopBar.IsVisible = !_toggling;
            Description.Margin = !_toggling ? new Thickness(8, 0) : new Thickness(0);
            ToggledValue.IsVisible = _toggling;
            ToggledDate.IsVisible = _toggling;

            await Shadow.RotateXTo(0, (uint)(ANIMATION_LENGTH * 0.80), Easing.Linear).ConfigureAwait(true);
        }

        private async Task ToggleCategory()
        {
            if (_toggling)
            {
                CategoryImage.RotationY = -270;

                await AvatarImage.RotateYTo(-90, ANIMATION_LENGTH / 3, Easing.SpringIn)
                    .ConfigureAwait(true);

                AvatarImage.IsVisible = false;
                CategoryImage.IsVisible = true;

                await CategoryName.TranslateTo(0, -50, 300, Easing.SpringIn);

                await CategoryImage.RotateYTo(-360, ANIMATION_LENGTH / 3, Easing.SpringOut)
                    .ConfigureAwait(true);

                CategoryImage.RotationY = 0;
            }
            else
            {
                AvatarImage.RotationY = -270;

                await CategoryImage.RotateYTo(-90, ANIMATION_LENGTH / 3, Easing.SpringIn)
                    .ConfigureAwait(true);

                CategoryImage.IsVisible = false;
                AvatarImage.IsVisible = true;

                await CategoryName.TranslateTo(0, 0, 300, Easing.SpringIn);

                await AvatarImage.RotateYTo(-360, ANIMATION_LENGTH / 3, Easing.SpringOut)
                    .ConfigureAwait(true);

                AvatarImage.RotationY = 0;
            }
        }
    }
}
