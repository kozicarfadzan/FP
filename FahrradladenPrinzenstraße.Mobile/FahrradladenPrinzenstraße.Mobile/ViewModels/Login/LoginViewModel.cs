using Xamarin.Forms.Internals;

namespace FahrradladenPrinzenstraße.Mobile.ViewModels.Login
{
    /// <summary>
    /// ViewModel for login page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class LoginViewModel : BaseViewModel
    {
        #region Fields

        private string username;

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the email ID from user in the login page.
        /// </summary>
        public string Username
        {
            get
            {
                return this.username;
            }

            set
            {
                if (this.username == value)
                {
                    return;
                }

                this.username = value;
                this.NotifyPropertyChanged();
            }
        }
        #endregion
    }
}
