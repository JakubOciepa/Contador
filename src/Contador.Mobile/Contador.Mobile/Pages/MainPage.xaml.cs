﻿using System;
using System.ComponentModel;
using System.Threading.Tasks;

using Contador.Core.Models;

using Xamarin.Forms;

namespace Contador.Mobile.Pages
{
    public partial class MainPage : ContentPage
    {
        public Expense Expense => new Expense("Cuksy", 12.11m, null, null) 
        {
            CreateDate = DateTime.Today,
        };

        public MainPage()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var animation = new Animation();
            animation.WithConcurrent((g) => MyGrid.HeightRequest = g, 70, 150, Easing.SpringOut);
            animation.Commit(Page, "ResizeAnimation", length: 3000);
        }
    }
}