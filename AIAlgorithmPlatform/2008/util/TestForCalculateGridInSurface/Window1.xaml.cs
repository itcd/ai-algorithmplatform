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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Media.Media3D;
using M2M.Util;
using M2M.Position.Interface;
using M2M.Position.Implement;

namespace TestForCalculateGridInSurface
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions.Add(new Point3D(56, 0, 0));
            mesh.Positions.Add(new Point3D(7, 67, 100));
            mesh.Positions.Add(new Point3D(0, 34, 78));
                                
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(0);
            
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(2);

            IList<Point3D> vertexs = new List<Point3D>();  
            vertexs.Add(new Point3D(56, 0, 0));
            vertexs.Add(new Point3D(7, 67, 100));
            vertexs.Add(new Point3D(0, 34, 78));

            IList<int> triangleIndices = new List<int>();
            triangleIndices.Add(2);
            triangleIndices.Add(1);
            triangleIndices.Add(0);

            var mGeometry = new GeometryModel3D(mesh, new DiffuseMaterial(Brushes.YellowGreen));

            Model3DGroup model3DGroup = new Model3DGroup();
            model3DGroup.Children.Add(mGeometry);

            var modelVisual3D = new ModelVisual3D();
            modelVisual3D.Content = model3DGroup;

            var calculateGridInSurfaceEngine = new CalculateGridInSurfaceEngine();
            
            Scene scene = new Scene();


            List<IPosition3D> point3DList = new List<IPosition3D>();
            calculateGridInSurfaceEngine.CalculateGridInSurface(vertexs, 5, point3DList);
            //point3DList.Add(new Position3D(0, 0, 0));

            scene.AddElement(new GridSetElement(point3DList));
            scene.ElementGroup.Add(modelVisual3D);
            scene.ShowScene();
        }
    }
}
