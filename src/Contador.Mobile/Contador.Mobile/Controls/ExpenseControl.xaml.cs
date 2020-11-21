using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Contador.Mobile.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpenseControl : ContentView
    {
        private readonly WeakEventManager _tappedEventManager = new WeakEventManager();

        public event EventHandler Tapped
        {
            add => _tappedEventManager.AddEventHandler(value);
            remove => _tappedEventManager.AddEventHandler(value);
        }

        public static readonly BindableProperty TitleProperty
            = BindableProperty.Create(nameof(Title), typeof(string), typeof(ExpenseControl));

        public static readonly BindableProperty DescriptionProperty
            = BindableProperty.Create(nameof(Description), typeof(string), typeof(ExpenseControl));

        public static readonly BindableProperty DateProperty
            = BindableProperty.Create(nameof(Date), typeof(DateTime), typeof(ExpenseControl));

        public static readonly BindableProperty ValueProperty
            = BindableProperty.Create(nameof(Value), typeof(decimal), typeof(ExpenseControl));

        public static readonly BindableProperty CategoryViewProperty
            = BindableProperty.Create(nameof(CategoryView), typeof(View), typeof(ExpenseControl));

        public static readonly BindableProperty UserViewProperty
            = BindableProperty.Create(nameof(UserView), typeof(View), typeof(ExpenseControl));

        public static readonly BindableProperty ReceiptImageProperty
            = BindableProperty.Create(nameof(ReceiptImage), typeof(Image), typeof(ExpenseControl));

        public static readonly BindableProperty UserNameProperty
            = BindableProperty.Create(nameof(UserName), typeof(string), typeof(ExpenseControl), "Dupa");

        public static readonly BindableProperty CornerRadiusProperty
            = BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(ExpenseControl));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public DateTime Date
        {
            get => (DateTime)GetValue(DateProperty);
            set => SetValue(DateProperty, value);
        }

        public decimal Value
        {
            get => (decimal)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public View CategoryView
        {
            get => (View)GetValue(CategoryViewProperty);
            set => SetValue(CategoryViewProperty, value);
        }

        public View UserView
        {
            get => (View)GetValue(UserViewProperty);
            set => SetValue(UserViewProperty, value);
        }

        public Image ReceiptImage
        {
            get => (Image)GetValue(ReceiptImageProperty);
            set => SetValue(ReceiptImageProperty, value);
        }

        public string UserName
        {
            get => (string)GetValue(UserNameProperty);
            set => SetValue(UserNameProperty, value);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public ExpenseControl()
        {
            InitializeComponent();
        }
    }
}
