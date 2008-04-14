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
using System.Reflection;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

namespace Perspective.Wpf.ResourceStrings
{
    /// <summary>
    /// A Decorator class to specify the resx file base name for localization.
    /// To use with StringResource markup extension.
    /// </summary>
    public class ResourceStringDecorator : Decorator
    {
        /// <summary>
        /// Gets or sets the resx file base name for localization.
        /// </summary>
        public string BaseName
        {
            get { return (string)GetValue(BaseNameProperty); }
            set { SetValue(BaseNameProperty, value); }
        }

        /// <summary>
        /// Identifies the BaseName dependency property.
        /// </summary>
        public static readonly DependencyProperty BaseNameProperty =
            DependencyProperty.Register(
                "BaseName",
                typeof(string),
                typeof(ResourceStringDecorator),
                new FrameworkPropertyMetadata(
                    "",
                    OnBaseNameChanged
                    ));


        /// <summary>
        /// Callback called when the BaseName property's value has changed.
        /// </summary>
        /// <param name="d">Sender object</param>
        /// <param name="e">Callback arguments</param>
        private static void OnBaseNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(d))
            {
                SetResourceManager(d, new ResourceManager(
                    (string)e.NewValue,
                    Assembly.GetEntryAssembly()));
            }
        }


        /// <summary>
        /// Gets the ResourceManager.
        /// This is an inheritable attached dependency property.
        /// </summary>
        /// <param name="obj">A descendant dependency object.</param>
        /// <returns></returns>
        public static ResourceManager GetResourceManager(DependencyObject obj)
        {
            return (ResourceManager)obj.GetValue(ResourceManagerProperty);
        }

        private static void SetResourceManager(DependencyObject obj, ResourceManager value)
        {
            obj.SetValue(ResourceManagerProperty, value);
        }

        
        /// <summary>
        /// Identifies the ResourceManager inheritable attached dependency property.
        /// </summary>
        public static readonly DependencyProperty ResourceManagerProperty =
            DependencyProperty.RegisterAttached(
                "ResourceManager", 
                typeof(ResourceManager), 
                typeof(ResourceStringDecorator), 
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.Inherits));
    }
}
