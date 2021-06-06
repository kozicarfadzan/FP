using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FahrradladenPrinzenstrasse.Mobile.Helpers
{
    public static class ScreenCoords
    {
        public static (double X, double Y) GetScreenCoordinates(this VisualElement view)
        {
            // A view's default X- and Y-coordinates are LOCAL with respect to the boundaries of its parent,
            // and NOT with respect to the screen. This method calculates the SCREEN coordinates of a view.
            // The coordinates returned refer to the top left corner of the view.
            var screenCoordinateX = view.X;
            var screenCoordinateY = view.Y;

            var parent = (VisualElement)view.Parent;
            while (parent != null)
            {
                screenCoordinateX += parent.X;
                screenCoordinateY += parent.Y;
                if (parent.Parent is VisualElement newParent)
                    parent = newParent;
                else
                    parent = null;
            }
            return (screenCoordinateX, screenCoordinateY);
        }
    }
}
