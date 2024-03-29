﻿using FahrradladenPrinzenstrasse.Mobile.ViewModels.Signup;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace FahrradladenPrinzenstrasse.Mobile.Views.Signup
{
    /// <summary>
    /// Page to sign in with user details.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SimpleSignUpPage
    {
        private SignUpPageViewModel VM;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleSignUpPage" /> class.
        /// </summary>
        public SimpleSignUpPage()
        {
            InitializeComponent();
            BindingContext = VM = new SignUpPageViewModel(Navigation);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await VM.Init();
        }
    }
}