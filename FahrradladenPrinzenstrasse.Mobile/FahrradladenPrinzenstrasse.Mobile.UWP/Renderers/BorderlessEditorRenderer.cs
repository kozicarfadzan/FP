using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(FahrradladenPrinzenstrasse.Mobile.Controls.BorderlessEditor), typeof(FahrradladenPrinzenstrasse.Mobile.UWP.BorderlessEditorRenderer))]

namespace FahrradladenPrinzenstrasse.Mobile.UWP
{
    /// <summary>
    /// Implementation of Borderless editor control.
    /// </summary>
    public class BorderlessEditorRenderer : EditorRenderer
    {
        #region Methods

        /// <summary>
        /// Used to set the zero border thickness for editor control .
        /// </summary>
        /// <param name="e">The editor</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (this.Control != null)
            {
                this.Control.BorderThickness = new Windows.UI.Xaml.Thickness(0);
                this.Control.Margin = new Windows.UI.Xaml.Thickness(0, 4, 0, 0);
            }
        }

        #endregion
    }
}
