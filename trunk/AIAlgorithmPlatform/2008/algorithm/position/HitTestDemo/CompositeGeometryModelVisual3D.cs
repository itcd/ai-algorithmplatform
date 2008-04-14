using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace HitTestDemo
{
    public class CompositeGeometryModelVisual3D : ModelVisual3D
    {
        GeometryModel3D bigCubeModel;
        GeometryModel3D smallCubeModel;

        public CompositeGeometryModelVisual3D()
        {
            bigCubeModel = GeometryGenerator.CreateCubeModel();
            smallCubeModel = GeometryGenerator.CreateCubeModel();
            smallCubeModel.Transform = new ScaleTransform3D(.3, .3, .3);

            Model3DGroup group = new Model3DGroup();
            group.Children.Add(bigCubeModel);
            group.Children.Add(smallCubeModel);

            Content = group;
        }

        public void ShowBigModel()
        {
            DiffuseMaterial bigMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Red));
            bigCubeModel.Material = bigMaterial;
            smallCubeModel.Material = null;
        }

        public void ShowSmallModel()
        {
            DiffuseMaterial smallMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Blue));
            smallCubeModel.Material = smallMaterial;
            bigCubeModel.Material = null;
        }
    }
}
