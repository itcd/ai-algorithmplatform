using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Petzold.Media3D;
using System.ComponentModel;
using Configuration;

using M2M.Position.Interface;

using Real = System.Double;
using IPositionSet = System.Collections.Generic.ICollection<M2M.Position.Interface.IPosition>;
using IPosition_ConnectedSet = System.Collections.Generic.ICollection<M2M.Position.Interface.IPosition_Connected>;
using PositionSet = System.Collections.Generic.List<M2M.Position.Interface.IPosition>;
using Position_ConnectedSet = System.Collections.Generic.List<M2M.Position.Interface.IPosition_Connected>;

using IPosition3DSet = System.Collections.Generic.ICollection<M2M.Position.Interface.IPosition3D>;
using Position3DSet = System.Collections.Generic.List<M2M.Position.Interface.IPosition3D>;

namespace M2M.Util
{
    public interface Element
    {
        void AddToScene(Model3DGroup elementGroup);
    }

    public class Scene
    {
        SceneWindows sceneWindows = null;

        List<Element> ElementList = new List<Element>();

        Model3DGroup elementGroup = null;

        public Scene()
        {
            sceneWindows = new SceneWindows();
            elementGroup = sceneWindows.group;

            sceneWindows.MouseRightButtonDown += delegate { new ConfiguratedByForm(ElementList[0]); };
        }

        public void AddElement(Element e)
        {
            e.AddToScene(elementGroup);
            ElementList.Add(e);
        }

        public void ShowScene()
        {
            sceneWindows.Show();
        }
    }

    public abstract class ADrawer
    {
        private bool visible = true;
        public bool Visible
        {
            get { return visible; }
            set 
            {
                if (visible != value)
                {
                    visible = value;
                }
            }
        }

        protected Model3DGroup elementGroup = null;
        public Model3DGroup ElementGroup
        {
            set { elementGroup = value; }
        }
    }

    internal class Converter : ExpandableObjectConverter
    { }

    [TypeConverter(typeof(Converter))]
    [CategoryAttribute("Drawer")]
    public class PointDrawer : ADrawer
    {
        MeshGeneratorBase meshGeneratorBase = null;
        MeshGeometry3D sphere = null;
        Material material = null;

        Color materialColor = Colors.AliceBlue;
        public Color MaterialColor
        {
            get { return materialColor; }
            set {
                materialColor = value;
                material = new DiffuseMaterial(new SolidColorBrush(value));
            }
        }

        public PointDrawer()
        {
            meshGeneratorBase = new Petzold.Media3D.SphereMesh();
            sphere = meshGeneratorBase.Geometry;
            material = new DiffuseMaterial(new SolidColorBrush(materialColor));
        }

        public void Add(Real x, Real y, Real z)
        {
            GeometryModel3D geometryModel3D = new GeometryModel3D(sphere, material);
            Transform3DGroup transform3DGroup = new Transform3DGroup();
            transform3DGroup.Children.Add(new ScaleTransform3D(0.5,0.5,0.5));
            TranslateTransform3D translateTransform3D = new TranslateTransform3D(x, y, z);
            transform3DGroup.Children.Add(translateTransform3D);
            geometryModel3D.Transform = transform3DGroup;
            elementGroup.Children.Add(geometryModel3D);
        }
    }

    public class PositionSetElement : Element
    {
        IPosition3DSet position3DSet = null;

        PointDrawer pointDrawer = new PointDrawer();

        [CategoryAttribute("Drawer")]
        public PointDrawer PointDrawer
        {
            get { return pointDrawer; }
            set { pointDrawer = value; }
        }

        public PositionSetElement(IPosition3DSet positionSet)
        {
            this.position3DSet = positionSet;
        }

        public void AddToScene(Model3DGroup elementGroup)
        {
            pointDrawer.ElementGroup = elementGroup;
           
            foreach (IPosition3D position in position3DSet)
            {
                pointDrawer.Add(position.GetX(),
                    position.GetY(),
                    position.GetZ());
            }
        }
    }
}
