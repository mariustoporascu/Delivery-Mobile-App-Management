﻿using FoodDeliveryApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodDeliveryApp.Views
{
    public partial class LoadingPage : ContentPage
    {
        public LoadingPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await DependencyService.Get<IDataStore>().Init();
            if (App.UserInfo.IsOwner)
                App.Current.MainPage = new AppShellOwner();
            else
                App.Current.MainPage = new AppShellDriver();
        }
    }
}