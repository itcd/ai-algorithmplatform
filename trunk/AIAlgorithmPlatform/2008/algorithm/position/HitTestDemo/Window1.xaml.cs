using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HitTestDemo
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private CompositeGeometryModelVisual3D box = new CompositeGeometryModelVisual3D();
        public Window1()
        {
            InitializeComponent();

            mainViewport.MouseDown += mainViewport_MouseDown;
            mainViewport.MouseUp += mainViewport_MouseUp;

            box.ShowBigModel();
            box.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0.6), 45));
            mainViewport.Children.Add(box);
        }

        ModelVisual3D GetHitTestResult(Point location)
        {
            HitTestResult result = VisualTreeHelper.HitTest(mainViewport, location);
            if (result != null && result.VisualHit is ModelVisual3D)
            {
                ModelVisual3D visual = (ModelVisual3D)result.VisualHit;
                return visual;
            }

            return null;
        }

        void mainViewport_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Point location = e.GetPosition(mainViewport);
            ModelVisual3D result = GetHitTestResult(location);
            if (result == null)
            {
                return;
            }

            if (result is CompositeGeometryModelVisual3D)
            {
                ((CompositeGeometryModelVisual3D)result).ShowBigModel();
                return;
            }
        }

        void mainViewport_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point location = e.GetPosition(mainViewport);
            ModelVisual3D result = GetHitTestResult(location);
            if (result == null)
            {
                return;
            }

            if (result is CompositeGeometryModelVisual3D)
            {
                ((CompositeGeometryModelVisual3D)result).ShowSmallModel();
                return;
            }
        }
    }
}
