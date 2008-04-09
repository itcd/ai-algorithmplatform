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
using MeshGeometry3DTransform;

namespace Mesh_Transform_Demo
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {

        public Window1()
        {
            InitializeComponent();

            MeshGeometry3D mesh1 = new MeshGeometry3D();
            MeshGeometry3D mesh2 = new MeshGeometry3D();
            MeshGeometry3D mesh3;

            mesh1.Positions.Add(new Point3D(10, 0, 0));
            mesh1.Positions.Add(new Point3D(0, 10, 0));
            mesh1.Positions.Add(new Point3D(0, 0, 10));

            mesh1.TriangleIndices.Add(0);
            mesh1.TriangleIndices.Add(1);
            mesh1.TriangleIndices.Add(2);

            mesh2.Positions.Add(new Point3D(20, 20, 0));
            mesh2.Positions.Add(new Point3D(0, 20, 20));
            mesh2.Positions.Add(new Point3D(20, 0, 20));

            mesh2.TriangleIndices.Add(0);
            mesh2.TriangleIndices.Add(1);
            mesh2.TriangleIndices.Add(2);

            mesh3 = MeshGeometry3DTransformer.combine(mesh1, mesh2);

            mesh1 = MeshGeometry3DTransformer.translate(mesh1, new Vector3D(20, 20, 20));
            mesh2 = MeshGeometry3DTransformer.scale(mesh2, new Vector3D(-3, -3, -3), new Point3D(0, 20, 20));

            GeometryModel3D mGeometry1 = new GeometryModel3D(mesh1, new DiffuseMaterial(Brushes.Red));
            GeometryModel3D mGeometry2 = new GeometryModel3D(mesh2, new DiffuseMaterial(Brushes.Green));
            GeometryModel3D mGeometry3 = new GeometryModel3D(mesh3, new DiffuseMaterial(Brushes.Blue));

            //model.Children.Add(mGeometry);
            Model3DGroup group = new Model3DGroup();
            group.Children.Add(mGeometry1);
            group.Children.Add(mGeometry2);
            group.Children.Add(mGeometry3);

            model.Content = group;
        }
    }
}
