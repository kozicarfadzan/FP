using FahrradladenPrinzenstrasse.Mobile.Helpers;
using FahrradladenPrinzenstrasse.Mobile.Models;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging.Abstractions;
using Syncfusion.XForms.Buttons;
using Syncfusion.XForms.PopupLayout;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace FahrradladenPrinzenstrasse.Mobile.Controls
{
    /// <summary>
    /// This class is extended from SfPopupLayout to show the popup.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class SfPopupView : SfPopupLayout
    {
        private List<Category> filterOptions;
        private List<SortOption> sortOptions;

        public Type requestType { get; private set; }
        public object request { get; private set; }

        private Func<Task> ucitajProizvodeCallback;

        /// <summary>
        /// To show the popup layout.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public void ShowPopUp(string title = null, string content = null, string buttonText = null, INavigation navigation = null, Type pageToNavigate = null)
        {
            DataTemplate templateView;
            Grid layout;

            templateView = new DataTemplate(() =>
            {
                layout = new Grid();
                layout.Margin = new Thickness(10, 0, 10, 0);
                layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
                layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                Label popupContent = new Label();
                popupContent.Text = content;
                popupContent.HorizontalTextAlignment = TextAlignment.Center;
                popupContent.VerticalTextAlignment = TextAlignment.Center;
                popupContent.VerticalOptions = LayoutOptions.Center;
                var fontFamily = Device.RuntimePlatform == Device.iOS ? "Montserrat-SemiBold" :
   Device.RuntimePlatform == Device.Android ? "Montserrat-SemiBold.ttf#Montserrat-SemiBold" : "Assets/Montserrat-SemiBold.ttf#Montserrat-SemiBold";
                popupContent.FontFamily = fontFamily;
                layout.Children.Add(popupContent, 0, 0);

                if (buttonText != null)
                {
                    SfButton button = new SfButton();
                    button.Text = buttonText;
                    button.Margin = new Thickness(20, 0, 20, 20);
                    button.VerticalOptions = LayoutOptions.End;
                    if (navigation != null && pageToNavigate != null)
                    {
                        button.CommandParameter = new List<object>() { navigation, pageToNavigate };
                        button.Clicked += Button_Clicked;
                    }
                    layout.Children.Add(button, 0, 1);
                }
                return layout;
            });

            this.PopupView.ShowHeader = false;
            this.PopupView.ShowFooter = false;
            this.PopupView.HeightRequest = 130;
            this.PopupView.ShowCloseButton = false;
            this.PopupView.AcceptButtonText = "OK";

            // Adding ContentTemplate of the SfPopupLayout
            this.PopupView.ContentTemplate = templateView;

            this.Show();
        }

        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            var btn = sender as SfButton;
            var parameters = btn.CommandParameter as List<object>;
            var navigation = parameters[0] as INavigation;
            var pageType = parameters[1] as Type;
            var page = Activator.CreateInstance(pageType);

            this.IsOpen = false;
            this.IsVisible = false;
            await navigation.PushAsync(page as Page);
        }

        public void ShowFilterPopUp<T>(string buttonText, View element, List<Models.Category> FilterOptions, T request, Func<Task> ucitajProizvodeCallback)
        {
            this.filterOptions = FilterOptions;
            this.requestType = typeof(T);
            this.request = request;
            this.ucitajProizvodeCallback = ucitajProizvodeCallback;
            DataTemplate templateView;
            Grid layout;

            templateView = new DataTemplate(() =>
            {
                layout = new Grid();
                layout.Margin = new Thickness(10, 0, 10, 0);

                var counter = 0;
                foreach (var item in filterOptions)
                {
                    layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                    StackLayout stack = new StackLayout
                    {
                        Orientation = StackOrientation.Vertical
                    };

                    Label label = new Label
                    {
                        Text = item.Name,
                        HorizontalTextAlignment = TextAlignment.Start,
                        VerticalTextAlignment = TextAlignment.Center,
                        VerticalOptions = LayoutOptions.Center,
                        FontAttributes = FontAttributes.Bold,
                        FontFamily = Device.RuntimePlatform == Device.iOS ? "Montserrat-SemiBold" :
       Device.RuntimePlatform == Device.Android ? "Montserrat-SemiBold.ttf#Montserrat-SemiBold" : "Assets/Montserrat-SemiBold.ttf#Montserrat-SemiBold"
                    };
                    stack.Children.Add(label);

                    if (item.FilterType == "radio")
                    {
                        SfRadioGroup radioGroup = new SfRadioGroup();
                        radioGroup.Spacing = 0;
                        foreach (var subCategory in item.SubCategories)
                        {
                            SfRadioButton option = new SfRadioButton
                            {
                                Text = subCategory.Value,
                                Padding = new Thickness { Left = 0, Bottom = 0, Right = 0, Top = 0 },
                            };

                            option.IsChecked = GetRadioButtonChecked(item.FieldName, subCategory.Value);

                            option.StateChanged += Option_StateChanged;
                            radioGroup.Children.Add(option);
                        }
                        stack.Children.Add(radioGroup);
                    }
                    else if (item.FilterType == "checkbox")
                    {
                        foreach (var subCategory in item.SubCategories)
                        {
                            SfCheckBox option = new SfCheckBox
                            {
                                Text = subCategory.Value
                            };
                            option.StateChanged += Option_StateChanged;
                            stack.Children.Add(option);
                        }
                    }



                    layout.Children.Add(stack, 0, counter++);
                }

                layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                SfButton button = new SfButton();
                button.Text = buttonText;
                button.Margin = new Thickness(20, 0, 20, 20);
                button.VerticalOptions = LayoutOptions.End;
                button.Clicked += Button_Clicked1;
                layout.Children.Add(button, 0, counter++);

                var scrollView = new ScrollView() { Content = layout, HeightRequest = 150 };

                return scrollView;
            });

            this.PopupView.ShowHeader = false;
            this.PopupView.ShowFooter = false;
            this.PopupView.HeightRequest = 600;
            this.PopupView.WidthRequest = 300;
            this.PopupView.ShowCloseButton = false;
            this.PopupView.AcceptButtonText = "OK";

            // Adding ContentTemplate of the SfPopupLayout
            this.PopupView.ContentTemplate = templateView;

            //this.ShowRelativeToView(element, RelativePosition.AlignBottom, 0, 0);

            // The default height and width of PopupView is 250 and 300 respectively.
            // You can set any desired values as the height and width of the popup view.

            (double btnX, double btnY) = ScreenCoords.GetScreenCoordinates(element);

            this.Show(btnX - 0, btnY + 75);
        }
        public void ShowSortingPopUp<T>(string buttonText, View element, List<Models.SortOption> SortOptions, T request, Func<Task> ucitajProizvodeCallback)
        {
            this.sortOptions = SortOptions;
            this.requestType = typeof(T);
            this.request = request;
            this.ucitajProizvodeCallback = ucitajProizvodeCallback;
            DataTemplate templateView;
            Grid layout;

            templateView = new DataTemplate(() =>
            {
                layout = new Grid();
                layout.Margin = new Thickness(10, 10, 10, 10);

                var counter = 0;

                layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                StackLayout stack = new StackLayout
                {
                    Orientation = StackOrientation.Vertical
                };

                Label label = new Label
                {
                    Text = "Poredak",
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalTextAlignment = TextAlignment.Center,
                    VerticalOptions = LayoutOptions.Center,
                    FontAttributes = FontAttributes.Bold,
                    FontFamily = Device.RuntimePlatform == Device.iOS ? "Montserrat-SemiBold" :
   Device.RuntimePlatform == Device.Android ? "Montserrat-SemiBold.ttf#Montserrat-SemiBold" : "Assets/Montserrat-SemiBold.ttf#Montserrat-SemiBold"
                };
                stack.Children.Add(label);

                SfRadioGroup radioGroup = new SfRadioGroup();
                radioGroup.Spacing = 0;
                foreach (var item in sortOptions)
                {

                    SfRadioButton option = new SfRadioButton
                    {
                        Text = item.Text,
                        Padding = new Thickness { Left = 0, Bottom = 0, Right = 0, Top = 0 },
                    };

                    option.IsChecked = GetSortRadioButtonChecked("Poredak", item.Value.ToString());

                    option.StateChanged += SortingOption_StateChanged;
                    radioGroup.Children.Add(option);
                }
                stack.Children.Add(radioGroup);


                layout.Children.Add(stack, 0, counter++);

                layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                SfButton button = new SfButton();
                button.Text = buttonText;
                button.Margin = new Thickness(20, 0, 20, 20);
                button.VerticalOptions = LayoutOptions.End;
                button.Clicked += Button_Clicked1;
                layout.Children.Add(button, 0, counter++);

                var scrollView = new ScrollView() { Content = layout, HeightRequest = 150 };

                return scrollView;
            });

            this.PopupView.ShowHeader = false;
            this.PopupView.ShowFooter = false;
            this.PopupView.HeightRequest = 300;
            this.PopupView.WidthRequest = 300;
            this.PopupView.ShowCloseButton = false;
            this.PopupView.AcceptButtonText = "OK";

            // Adding ContentTemplate of the SfPopupLayout
            this.PopupView.ContentTemplate = templateView;

            //this.ShowRelativeToView(element, RelativePosition.AlignBottom, 0, 0);

            // The default height and width of PopupView is 250 and 300 respectively.
            // You can set any desired values as the height and width of the popup view.

            (double btnX, double btnY) = ScreenCoords.GetScreenCoordinates(element);

            this.Show(btnX - 0, btnY + 75);
        }

        private bool? GetRadioButtonChecked(string fieldName, string value)
        {
            var properties = requestType.GetProperties();
            var prop = properties.FirstOrDefault(x => x.Name == fieldName);
            if (prop == null)
                return false;

            Type propType = prop.PropertyType;
            if (propType.IsEnum)
            {
                try
                {
                    var enumValue = Enum.Parse(propType, value);

                    return prop.GetValue(request) == enumValue;
                }
                catch (Exception)
                {
                    return false;
                }

            }
            else if (propType.IsNullableEnum())
            {
                propType = Nullable.GetUnderlyingType(propType);
                try
                {
                    var enumValue = Enum.Parse(propType, value);
                    return prop.GetValue(request) == enumValue;
                }
                catch (Exception)
                {
                    return false;
                }

            }
            else if (propType == typeof(string))
            {
                return prop.GetValue(request) == value;
            }
            else
            {
                bool IsNullable = false;
                if (propType.IsNullable())
                {
                    IsNullable = true;
                    propType = Nullable.GetUnderlyingType(propType);
                }

                if (propType == typeof(int))
                {



                    //foreach (KeyValuePair<int, string> filterChoice in currentFilterChoices)
                    //{
                    //    if (filterChoice.Value == value)
                    //    {
                    //        if (IsNullable && filterChoice.Key == -1)
                    //            prop.SetValue(request, null);
                    //        else
                    //            prop.SetValue(request, filterChoice.Key);

                    //        break;
                    //    }
                    //}
                }
                else
                {
                    Application.Current.MainPage.DisplayAlert("Error", "Nepoznati tip podatka: " + prop.PropertyType.Name, "OK");
                }

            }
            return false;
        }

        private bool? GetSortRadioButtonChecked(string fieldName, string value)
        {
            var properties = requestType.GetProperties();
            var prop = properties.FirstOrDefault(x => x.Name == fieldName);
            if (prop == null)
                return false;

            Type propType = prop.PropertyType;

            if (propType == typeof(int))
            {
                foreach (var sortChoice in sortOptions)
                {
                    if (sortChoice.Value.ToString() == value)
                    {
                        int requestValue = int.Parse(prop.GetValue(request).ToString());
                        return requestValue == sortChoice.Value;
                    }
                }
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Error", "Nepoznati tip podatka: " + prop.PropertyType.Name, "OK");
            }

            return false;
        }

        private void Option_StateChanged(object sender, StateChangedEventArgs e)
        {
            if (sender is SfRadioButton radioBtn && e.IsChecked == true)
            {
                var parentStack = radioBtn.Parent.Parent as StackLayout;
                var label = parentStack.Children[0] as Label;
                var optionName = label.Text;
                string fieldName = null;
                Dictionary<int, string> currentFilterChoices = null;

                foreach (var item in filterOptions)
                {
                    if (item.Name == optionName)
                    {
                        fieldName = item.FieldName;
                        currentFilterChoices = item.SubCategories;
                        break;
                    }
                }

                if (fieldName != null)
                {
                    foreach (PropertyInfo prop in requestType.GetProperties())
                    {
                        if (prop != null && fieldName == prop.Name)
                        {
                            Type propType = prop.PropertyType;
                            if (propType.IsEnum)
                            {
                                try
                                {
                                    var enumValue = Enum.Parse(propType, radioBtn.Text);
                                    prop.SetValue(request, enumValue);
                                }
                                catch (Exception)
                                {
                                    prop.SetValue(request, null);
                                }

                            }
                            else if (propType.IsNullableEnum())
                            {
                                propType = Nullable.GetUnderlyingType(propType);
                                try
                                {
                                    var enumValue = Enum.Parse(propType, radioBtn.Text);
                                    prop.SetValue(request, enumValue);

                                }
                                catch (Exception)
                                {
                                    prop.SetValue(request, null);
                                }

                            }
                            else if (propType == typeof(string))
                            {
                                prop.SetValue(request, radioBtn.Text);
                            }
                            else
                            {
                                bool IsNullable = false;
                                if (propType.IsNullable())
                                {
                                    IsNullable = true;
                                    propType = Nullable.GetUnderlyingType(propType);
                                }

                                if (propType == typeof(int))
                                {
                                    foreach (KeyValuePair<int, string> filterChoice in currentFilterChoices)
                                    {
                                        if (filterChoice.Value == radioBtn.Text)
                                        {
                                            if (IsNullable && filterChoice.Key == -1)
                                                prop.SetValue(request, null);
                                            else
                                                prop.SetValue(request, filterChoice.Key);

                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    Application.Current.MainPage.DisplayAlert("Error", "Nepoznati tip podatka: " + prop.PropertyType.Name, "OK");
                                }

                            }
                            break;
                        }
                    }
                }

            }

            else if (sender is SfCheckBox checkBox)
            {
                var parentStack = checkBox.Parent as StackLayout;
                var label = parentStack.Children[0] as Label;
                var optionName = label.Text;
                string fieldName = null;
                Dictionary<int, string> currentFilterChoices = null;

                foreach (var item in filterOptions)
                {
                    if (item.Name == optionName)
                    {
                        fieldName = item.FieldName;
                        currentFilterChoices = item.SubCategories;
                        break;
                    }
                }

                if (fieldName != null)
                {
                    foreach (PropertyInfo prop in requestType.GetProperties())
                    {
                        if (prop != null && fieldName == prop.Name)
                        {
                            Type propType = prop.PropertyType;
                            if (propType.IsGenericList(out Type listType))
                            {
                                object value = null;
                                if (listType.IsEnum)
                                {
                                    value = Enum.Parse(listType, checkBox.Text);

                                }
                                else if (listType == typeof(string))
                                {
                                    value = checkBox.Text;
                                }
                                else
                                {
                                    if (listType == typeof(int))
                                    {
                                        foreach (KeyValuePair<int, string> filterChoice in currentFilterChoices)
                                        {
                                            if (filterChoice.Value == checkBox.Text)
                                            {
                                                value = filterChoice.Key;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Application.Current.MainPage.DisplayAlert("Error", "Nepoznati tip podatka: " + prop.PropertyType.Name, "OK");
                                    }

                                }

                                if (value != null)
                                {
                                    object instance = prop.GetValue(request);
                                    if (instance == null)
                                        instance = Activator.CreateInstance(propType);

                                    IList list = (IList)instance;
                                    if (e.IsChecked == true)
                                        list.Add(value);
                                    else
                                        list.Remove(value);

                                    prop.SetValue(request, list, null);
                                }

                            }
                            else
                            {
                                Application.Current.MainPage.DisplayAlert("Error", "Parametar za checkbox nema list type: " + prop.PropertyType.Name, "OK");
                            }


                            break;
                        }
                    }
                }

            }

            ucitajProizvodeCallback();
        }


        private void SortingOption_StateChanged(object sender, StateChangedEventArgs e)
        {
            if (sender is SfRadioButton radioBtn && e.IsChecked == true)
            {
                var parentStack = radioBtn.Parent.Parent as StackLayout;
                var label = parentStack.Children[0] as Label;
                var optionName = label.Text;
                string fieldName = "Poredak";

                foreach (PropertyInfo prop in requestType.GetProperties())
                {
                    if (prop != null && fieldName == prop.Name)
                    {
                        Type propType = prop.PropertyType;

                        if (propType == typeof(int))
                        {
                            foreach (var sortChoice in sortOptions)
                            {
                                if (sortChoice.Text == radioBtn.Text)
                                {
                                    prop.SetValue(request, sortChoice.Value);

                                    break;
                                }
                            }
                        }
                        else
                        {
                            Application.Current.MainPage.DisplayAlert("Error", "Nepoznati tip podatka: " + prop.PropertyType.Name, "OK");
                        }

                        break;
                    }
                }

            }

            ucitajProizvodeCallback();
        }


        private void Button_Clicked1(object sender, EventArgs e)
        {
            this.IsOpen = false;
            // this.IsVisible = false;
        }
    }
}
