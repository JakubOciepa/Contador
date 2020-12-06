
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
        }

        private async void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            if (_isCollapsed)
            {
                AvatarImage.RotationY = -270;
                await CategoryImage.RotateYTo(-90, ANIMATION_LENGTH, Easing.SpringIn);
                CategoryImage.IsVisible = false;
                AvatarImage.IsVisible = true;
                await AvatarImage.RotateYTo(-360, ANIMATION_LENGTH, Easing.SpringOut);
                AvatarImage.RotationY = 0;
                _isCollapsed = false;
            }
            else
            {
                CategoryImage.RotationY = -270;
                await AvatarImage.RotateYTo(-90, ANIMATION_LENGTH, Easing.SpringIn);
                AvatarImage.IsVisible = false;
                CategoryImage.IsVisible = true;
                await CategoryImage.RotateYTo(-360, ANIMATION_LENGTH, Easing.SpringOut);
                CategoryImage.RotationY = 0;
                _isCollapsed = true;
            }

            //_ = CategoryImage.FadeTo(0, ANIMATION_LENGTH, Easing.Linear);
            //_ = AvatarImage.FadeTo(100, ANIMATION_LENGTH, Easing.Linear);
            ////_ = CategoryImage.RotateYTo(180, ANIMATION_LENGTH, Easing.Linear);
            //await AvatarImage.RotateYTo(180, ANIMATION_LENGTH, Easing.Linear);

            //CategoryImage.IsVisible = false;
            //AvatarImage.Opacity = 100;
            //AvatarImage.IsVisible = true;


            //var animation = new ParalelAnimation()
            //{
            //    {0, 1, new Animation((f) => AvatarImage.Opacity = f, 100, 0, Easing.Linear) },
            //    {0, 1, new Animation((f) => AvatarImage.RotationY = f, 180, 0, Easing.Linear) },
            //};

            //animation.Commit(AvatarImage, nameof(TapGestureRecognizer_Tapped), length: ANIMATION_LENGTH, finished: (_, t) => tcs.SetResult(true));
        }
    }
}
