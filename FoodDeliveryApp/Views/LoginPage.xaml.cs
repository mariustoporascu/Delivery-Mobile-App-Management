﻿using FoodDeliveryApp.ViewModels;

using System;
using System.Diagnostics;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace FoodDeliveryApp.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            var vm = new LoginViewModel();
            if (Device.RuntimePlatform == Device.iOS)
                vm.OnSignIn += OnSignInApple;
            else
                vm.OnSignIn += OnSignIn;
            vm.OnSignInFailed += OnSignInFailed;
            BindingContext = vm;
            if (App.isLoggedIn)
            {
                if (Device.RuntimePlatform == Device.iOS)
                    OnSignInApple(this, new EventArgs());
                else
                    OnSignIn(this, new EventArgs());
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (App.isLoggedIn)
            {
                if (Device.RuntimePlatform == Device.iOS)
                    OnSignInApple(this, new EventArgs());
                else
                    OnSignIn(this, new EventArgs());
            }
        }

        private async void RedirSignUp(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new RegisterPage());
        }
        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            // Page appearance not animated
            await Navigation.PopModalAsync(false).ConfigureAwait(false);
        }
        private void OnSignInApple(object sender, EventArgs e)
        {
            try
            {
                this.DisplayToastAsync("Ai fost autentificat.", 2300);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            Navigation.PopModalAsync(false).ConfigureAwait(false);
        }
        private async void OnSignIn(object sender, EventArgs e)
        {
            try
            {
                await this.DisplayToastAsync("Ai fost autentificat.", 1300);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            await Navigation.PopModalAsync(false).ConfigureAwait(false);
        }

        private async void OnSignInFailed(object sender, EventArgs e)
        {

            await DisplayAlert("Eroare", "Autentificare esuata", "OK");
        }
    }
}