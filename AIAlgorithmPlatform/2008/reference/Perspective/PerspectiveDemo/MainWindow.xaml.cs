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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Perspective.Core;
using Perspective.Wpf;

namespace PerspectiveDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string skin = (string)App.Current.Properties["Skin"];
            if (!String.IsNullOrEmpty(skin))
            {
                LoadSkin(skin);
                switch (skin)
                {
                    case "LaMoisson":
                        rbLaMoisson.IsChecked = true;
                        break;
                    case "PsycheRock":
                        rbPsycheRock.IsChecked = true;
                        break;
                    case "BlackAndWhite":
                        rbBlackAndWhite.IsChecked = true;
                        break;
                }
            }
            else
            {
                SkinManager.Current.LoadDefaultSkin();
                Perspective.Wpf.SkinManager.Current.LoadDefaultSkin();
#if DN35
                Perspective.Wpf3D.SkinManager.Current.LoadDefaultSkin();
#endif
            }

            //((App)App.Current).ResetScreenIndependentScaleTransform();
            //_dpiScaleTransform.ScaleX = RenderingHelper.ScreenIndependentScaleX;
            //_dpiScaleTransform.ScaleY = RenderingHelper.ScreenIndependentScaleY;
            tabControl1.LayoutTransform = DipHelper.ScreenIndependentScaleTransform;
        }

        private void OpenPageCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter != null)
            {
                string page = e.Parameter.ToString();
                if (!String.IsNullOrEmpty(page))
                {

                    frame1.Source = new Uri(page, UriKind.RelativeOrAbsolute);
                    if (page != "Pages/DpiScaling.xaml")
                    {
                        frame1.LayoutTransform = DipHelper.ScreenIndependentScaleTransform;
                    }
                    else
                    {
                        frame1.LayoutTransform = null;
                    }
                }
            }
        }

        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void LoadSkinCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter != null)
            {
                string skin = e.Parameter.ToString();
                if (!String.IsNullOrEmpty(skin))
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    try
                    {
                        LoadSkin(skin);
                        App.Current.Properties["Skin"] = skin;
                    }
                    finally
                    {
                        Mouse.OverrideCursor = null;
                    }
                }
            }
        }

        private void LoadSkin(string skin)
        {
            SkinManager.Current.LoadSkin(skin);
            Perspective.Wpf.SkinManager.Current.LoadSkin(skin);
#if DN35
            Perspective.Wpf3D.SkinManager.Current.LoadSkin(skin);
#endif
        }
    }
}
