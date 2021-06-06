using System.Collections.Generic;
using System.Runtime.Serialization;
using Xamarin.Forms.Internals;

namespace FahrradladenPrinzenstrasse.Mobile.Models
{
    /// <summary>
    /// Model for category.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DataContract]
    public class Category
    {
        /// <summary>
        /// Gets or sets the property that has been bound with a label in SfExpander header, which displays the category name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the property that has been bound with a label in SfExpander content, which displays the sub category.
        /// </summary>
        public Dictionary<int, string> SubCategories { get; set; }

        public string FilterType { get; set; }
        public string FieldName { get; set; }
    }
}