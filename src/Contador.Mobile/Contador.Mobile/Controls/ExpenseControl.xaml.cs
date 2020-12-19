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
        private bool _toggled = true;
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
            AvatarImage.IsVisible = false;
        }

        private async void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            CancelAnimations();

            if (_toggled)
            {
                await ToggleContent().ConfigureAwait(true);

                await SwitchAvatarImage().ConfigureAwait(true);
            }
            else
            {
                await SwitchAvatarImage().ConfigureAwait(true);

                await ToggleContent().ConfigureAwait(true);
            }

            Shadow.RotationX = 0;
            _toggled = !_toggled;
        }

        private void CancelAnimations()
        {
            Shadow.CancelAnimations();
            CategoryImage.CancelAnimations();
            AvatarImage.CancelAnimations();
        }

        private async Task ToggleContent()
        {
            if (_toggled)
                _pageHeight = Control.Height;

            await Shadow.RotateXTo(90, ANIMATION_LENGTH / 5, Easing.Linear).ConfigureAwait(true);

            Swipe.HeightRequest = _toggled ? _pageHeight * 2 : _pageHeight;

            await Shadow.RotateXTo(0, (uint)(ANIMATION_LENGTH * 0.80), Easing.Linear).ConfigureAwait(true);
        }

        private async Task SwitchAvatarImage()
        {
            if (_toggled)
            {
                AvatarImage.RotationY = -270;

                await CategoryImage.RotateYTo(-90, ANIMATION_LENGTH, Easing.SpringIn)
                    .ConfigureAwait(true);

                CategoryImage.IsVisible = false;
                AvatarImage.IsVisible = true;

                await AvatarImage.RotateYTo(-360, ANIMATION_LENGTH, Easing.SpringOut)
                    .ConfigureAwait(true);

                AvatarImage.RotationY = 0;
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
            }
        }
    }
}
