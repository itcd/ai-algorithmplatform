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

namespace M2M.Util.Scene
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
        static MeshGeneratorBase m = new Petzold.Media3D.SphereMesh();
        static MeshGeometry3D sphere = m.Geometry;
        static Material material = new DiffuseMaterial(Brushes.AliceBlue);

        public void Add(Real x, Real y, Real z)
        {
            GeometryModel3D mGeometry;
            Transform3DGroup tg;
            TranslateTransform3D tt;
            mGeometry = new GeometryModel3D(sphere, material);
            tg = new Transform3DGroup();
            tt = new TranslateTransform3D(x, y, z);
            tg.Children.Add(tt);
            mGeometry.Transform = tg;
            elementGroup.Children.Add(mGeometry);
        }
    }

    public class PositionSetElement : Element
    {
        IPositionSet positionSet = null;

        PointDrawer pointDrawer = new PointDrawer();
        [CategoryAttribute("Drawer")]
        public PointDrawer PointDrawer
        {
            get { return pointDrawer; }
            set { pointDrawer = value; }
        }

        public PositionSetElement(IPositionSet positionSet)
        {
            this.positionSet = positionSet;
        }

        public void AddToScene(Model3DGroup elementGroup)
        {
            pointDrawer.ElementGroup = elementGroup;

            foreach (IPosition position in positionSet)
            {
                pointDrawer.Add(position.GetValue(0),
                    position.GetValue(1),
                    position.GetValue(2));
            }
        }
    }
}
