//------------------------------------------------------------------
//
//  For licensing information and to get the latest version go to:
//  http://www.codeplex.com/perspective
//
//  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY
//  OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
//  LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR
//  FITNESS FOR A PARTICULAR PURPOSE.
//
//------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Windows.Markup;
using System.Windows;
using System.Windows.Media;
using System.Resources;

namespace Perspective.Wpf.ResourceStrings
{
    /// <summary>
    /// A markup extension to get a resource string from a resx.
    /// To use with StringResourceDecorator.
    /// </summary>
    [MarkupExtensionReturnType(typeof(String))]
    public class ResourceStringExtension : MarkupExtension
    {
        /// <summary>
        /// Initializes a new instance of StringResourceExtension.
        /// </summary>
        public ResourceStringExtension()
        {
        }

        /// <summary>
        /// Initializes a new instance of StringResourceExtension.
        /// </summary>
        /// <param name="name">Name of the resource string.</param>
        public ResourceStringExtension(string name)
        {
            _name = name;
        }

        private string _name;

        /// <summary>
        /// Gets or sets the name of the string resource.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Returns the value of the string resource.
        /// </summary>
        /// <param name="serviceProvider">Object that can provide services for the markup extension.</param>
        /// <returns>The string value.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            string value = "";

            IProvideValueTarget target = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            if (target != null)
            {
                DependencyObject d = (DependencyObject)target.TargetObject;
                if ( d != null)
                {

                    ResourceManager rm = ResourceStringDecorator.GetResourceManager(d);
                    if (rm != null)
                    {
                        value = rm.GetString(_name);
                    }
                }
            }
            return value;
        }
    }
}
