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
        void AddToScene(Visual3DCollection elementGroup);
    }

    public class Scene
    {
        SceneWindows sceneWindows = null;

        List<Element> ElementList = new List<Element>();

        Visual3DCollection elementGroup = null;

        public Visual3DCollection ElementGroup
        {
            get { return elementGroup; }
            set { elementGroup = value; }
        }

        public Scene()
        {
            sceneWindows = new SceneWindows();
            elementGroup = sceneWindows.model.Children;

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

        MeshGeneratorBase meshGeneratorBase = null;
        MeshGeometry3D sphere = null;
        Material material = null;
        Model3DGroup modelGroup = new Model3DGroup();

        //PointDrawer pointDrawer = new PointDrawer();
        //[CategoryAttribute("Drawer")]
        //public PointDrawer PointDrawer
        //{
        //    get { return pointDrawer; }
        //    set { pointDrawer = value; }
        //}

        Color materialColor = Colors.AliceBlue;
        [CategoryAttribute("Appearance")]
        public Color MaterialColor
        {
            get { return materialColor; }
            set
            {
                materialColor = value;
                material = new DiffuseMaterial(new SolidColorBrush(value));
                foreach (GeometryModel3D model in modelGroup.Children)
                {
                    model.Material = material;
                }
            }
        }

        public enum EMaterialStyle{EmissiveMaterial, DiffuseMaterial, SpecularMaterial};

        EMaterialStyle materialStyle = EMaterialStyle.DiffuseMaterial;
        [CategoryAttribute("Appearance")]
        public EMaterialStyle MaterialStyle
        {
            get { return materialStyle; }
            set {
                materialStyle = value;

                if (value == EMaterialStyle.DiffuseMaterial)
                {
                    material = new DiffuseMaterial(new SolidColorBrush(materialColor));
                }
                else if (value == EMaterialStyle.EmissiveMaterial)
                {
                    material = new EmissiveMaterial(new SolidColorBrush(materialColor));
                }
                else if (value == EMaterialStyle.SpecularMaterial)
                {
                    MaterialGroup materialGroup = new MaterialGroup();
                    materialGroup.Children.Add(new DiffuseMaterial(new SolidColorBrush(materialColor)));
                    materialGroup.Children.Add(new SpecularMaterial(new SolidColorBrush(Colors.White), 64));                    
                    material = materialGroup;
                }

                foreach (GeometryModel3D model in modelGroup.Children)
                {
                    model.Material = material;
                }
            }
        }

        public enum EShape { Cube, Sphere, Dodecahedron, Icosahedron, Octahedron, Tetrahedron};
        EShape shape = EShape.Tetrahedron;
        [CategoryAttribute("Appearance")]
        public EShape Shape
        {
            get { return shape; }
            set {
                shape = value;

                Geometry3D geometry3D = null;

                if (value == EShape.Cube)
                {
                    geometry3D = new CubeMesh().Geometry;
                }
                else if (value == EShape.Dodecahedron)
                {
                    geometry3D = new DodecahedronMesh().Geometry;
                }
                else if (value == EShape.Icosahedron)
                {
                    geometry3D = new IcosahedronMesh().Geometry;
                }
                else if (value == EShape.Octahedron)
                {
                    geometry3D = new OctahedronMesh().Geometry;
                }
                else if (value == EShape.Sphere)
                {
                    geometry3D = new SphereMesh().Geometry;
                }
                else if (value == EShape.Tetrahedron)
                {
                    geometry3D = new TetrahedronMesh().Geometry;
                }

                foreach (GeometryModel3D model in modelGroup.Children)
                {
                    model.Geometry = geometry3D;
                }
            }
        }

        double scale = 0.25;
        [CategoryAttribute("Appearance")]
        public double Scale
        {
            get { return scale; }
            set {
                scale = value;
                foreach (GeometryModel3D model in modelGroup.Children)
                {
                    ((Transform3DGroup)model.Transform).Children[0] = new ScaleTransform3D(value * 2, value * 2, value * 2);
                }
            }
        }


        public PositionSetElement(IPosition3DSet positionSet)
        {
            this.position3DSet = positionSet;
        }

        public void AddToScene(Visual3DCollection elementGroup)
        {
            //pointDrawer.ElementGroup = elementGroup;

            meshGeneratorBase = new Petzold.Media3D.OctahedronMesh();
            sphere = meshGeneratorBase.Geometry;
            //Color materialColor = new DiffuseMaterial(new SolidColorBrush(value));
            material = new DiffuseMaterial(new SolidColorBrush(Colors.AliceBlue));
           
            foreach (IPosition3D position in position3DSet)
            {
                //pointDrawer.Add(position.GetX(),
                //    position.GetY(),
                //    position.GetZ());

                GeometryModel3D geometryModel3D = new GeometryModel3D(sphere, material);
                Transform3DGroup transform3DGroup = new Transform3DGroup();
                transform3DGroup.Children.Add(new ScaleTransform3D(0.5, 0.5, 0.5));
                TranslateTransform3D translateTransform3D = new TranslateTransform3D(
                    position.GetX(), position.GetY(), position.GetZ());
                transform3DGroup.Children.Add(translateTransform3D);
                geometryModel3D.Transform = transform3DGroup;
                modelGroup.Children.Add(geometryModel3D);
            }

            var modelVisual3D = new ModelVisual3D();
            modelVisual3D.Content = modelGroup;
            elementGroup.Add(modelVisual3D);

            //var positionSetMesh = new MeshGeometry3D();
            //MeshGeometry3D sphereMesh = new Sphere().Geometry;

            //foreach (IPosition3D position in position3DSet)
            //{
            //    MeshGeometry3D mesh = WPF3DHelper.Translate(sphereMesh, new Vector3D(
            //        position.GetX(), position.GetY(), position.GetZ()));

            //    WPF3DHelper.CombineTo(positionSetMesh, mesh);
            //}

            //GeometryModel3D positionSetModel = new GeometryModel3D(positionSetMesh, material);

            //var modelVisual3D = new ModelVisual3D();
            //modelVisual3D.Content = positionSetModel;
            //elementGroup.Add(modelVisual3D);
        }
    }

    public class GridSetElement : Element
    {
        IPosition3DSet position3DSet = null;

        MeshGeneratorBase meshGeneratorBase = null;
        MeshGeometry3D shape = null;
        Material material = null;
        Model3DGroup modelGroup = new Model3DGroup();

        Color materialColor = Colors.AliceBlue;
        [CategoryAttribute("Appearance")]
        public Color MaterialColor
        {
            get { return materialColor; }
            set
            {
                materialColor = value;
                material = new DiffuseMaterial(new SolidColorBrush(value));
                foreach (GeometryModel3D model in modelGroup.Children)
                {
                    model.Material = material;
                }
            }
        }

        public enum EMaterialStyle { EmissiveMaterial, DiffuseMaterial, SpecularMaterial };

        EMaterialStyle materialStyle = EMaterialStyle.DiffuseMaterial;
        [CategoryAttribute("Appearance")]
        public EMaterialStyle MaterialStyle
        {
            get { return materialStyle; }
            set
            {
                materialStyle = value;

                if (value == EMaterialStyle.DiffuseMaterial)
                {
                    material = new DiffuseMaterial(new SolidColorBrush(materialColor));
                }
                else if (value == EMaterialStyle.EmissiveMaterial)
                {
                    material = new EmissiveMaterial(new SolidColorBrush(materialColor));
                }
                else if (value == EMaterialStyle.SpecularMaterial)
                {
                    MaterialGroup materialGroup = new MaterialGroup();
                    materialGroup.Children.Add(new DiffuseMaterial(new SolidColorBrush(materialColor)));
                    materialGroup.Children.Add(new SpecularMaterial(new SolidColorBrush(materialColor), 64));
                    material = materialGroup;
                }

                foreach (GeometryModel3D model in modelGroup.Children)
                {
                    model.Material = material;
                }
            }
        }

        //public enum EShape { Cube, Sphere, Dodecahedron, Icosahedron, Octahedron, Tetrahedron };
        //EShape shape = EShape.Tetrahedron;
        //[CategoryAttribute("Appearance")]
        //public EShape Shape
        //{
        //    get { return shape; }
        //    set
        //    {
        //        shape = value;

        //        Geometry3D geometry3D = null;

        //        if (value == EShape.Cube)
        //        {
        //            geometry3D = new CubeMesh().Geometry;
        //        }
        //        else if (value == EShape.Dodecahedron)
        //        {
        //            geometry3D = new DodecahedronMesh().Geometry;
        //        }
        //        else if (value == EShape.Icosahedron)
        //        {
        //            geometry3D = new IcosahedronMesh().Geometry;
        //        }
        //        else if (value == EShape.Octahedron)
        //        {
        //            geometry3D = new OctahedronMesh().Geometry;
        //        }
        //        else if (value == EShape.Sphere)
        //        {
        //            geometry3D = new SphereMesh().Geometry;
        //        }
        //        else if (value == EShape.Tetrahedron)
        //        {
        //            geometry3D = new TetrahedronMesh().Geometry;
        //        }

        //        foreach (GeometryModel3D model in modelGroup.Children)
        //        {
        //            model.Geometry = geometry3D;
        //        }
        //    }
        //}

        double scale = 1;
        [CategoryAttribute("Appearance")]
        public double Scale
        {
            get { return scale; }
            set
            {
                scale = value;
                foreach (GeometryModel3D model in modelGroup.Children)
                {
                    Transform3DGroup transform3DGroup = (Transform3DGroup)model.Transform;
                    //transform3DGroup.Children[0] = new ScaleTransform3D(value, value, value);
                    //transform3DGroup.Transform.Children[1] = new TranslateTransform3D( value * 2, value * 2, value * 2); 
                    transform3DGroup.Children[2] = new ScaleTransform3D(value, value, value);
                }
            }
        }


        public GridSetElement(IPosition3DSet positionSet)
        {
            this.position3DSet = positionSet;
        }

        public void AddToScene(Visual3DCollection elementGroup)
        {
            //pointDrawer.ElementGroup = elementGroup;

            meshGeneratorBase = new Petzold.Media3D.CubeMesh();
            shape = meshGeneratorBase.Geometry;
            //Color materialColor = new DiffuseMaterial(new SolidColorBrush(value));
            material = new DiffuseMaterial(new SolidColorBrush(Colors.AliceBlue));

            foreach (IPosition3D position in position3DSet)
            {
                //pointDrawer.Add(position.GetX(),
                //    position.GetY(),
                //    position.GetZ());

                GeometryModel3D geometryModel3D = new GeometryModel3D(shape, material);
                Transform3DGroup transform3DGroup = new Transform3DGroup();
                transform3DGroup.Children.Add(new ScaleTransform3D(0.5, 0.5, 0.5));
                transform3DGroup.Children.Add(new TranslateTransform3D(
                    position.GetX(), position.GetY(), position.GetZ()));
                transform3DGroup.Children.Add(new ScaleTransform3D(1,1,1));
                geometryModel3D.Transform = transform3DGroup;
                modelGroup.Children.Add(geometryModel3D);
            }

            var modelVisual3D = new ModelVisual3D();
            modelVisual3D.Content = modelGroup;
            elementGroup.Add(modelVisual3D);

            //var positionSetMesh = new MeshGeometry3D();
            //MeshGeometry3D sphereMesh = new Sphere().Geometry;

            //foreach (IPosition3D position in position3DSet)
            //{
            //    MeshGeometry3D mesh = WPF3DHelper.Translate(sphereMesh, new Vector3D(
            //        position.GetX(), position.GetY(), position.GetZ()));

            //    WPF3DHelper.CombineTo(positionSetMesh, mesh);
            //}

            //GeometryModel3D positionSetModel = new GeometryModel3D(positionSetMesh, material);

            //var modelVisual3D = new ModelVisual3D();
            //modelVisual3D.Content = positionSetModel;
            //elementGroup.Add(modelVisual3D);
        }
    }

    public class PositionSetElement2 : Element
    {
        IPosition3DSet position3DSet = null;

        MeshGeneratorBase meshGeneratorBase = null;
        MeshGeometry3D sphere = null;
        Material material = null;
        Model3DGroup modelGroup = new Model3DGroup();

        //PointDrawer pointDrawer = new PointDrawer();
        //[CategoryAttribute("Drawer")]
        //public PointDrawer PointDrawer
        //{
        //    get { return pointDrawer; }
        //    set { pointDrawer = value; }
        //}

        Color materialColor = Colors.AliceBlue;
        public Color MaterialColor
        {
            get { return materialColor; }
            set
            {
                materialColor = value;
                material = new DiffuseMaterial(new SolidColorBrush(value));

                foreach (GeometryModel3D model in modelGroup.Children)
                {
                    model.Material = material;
                }
            }
        }

        public PositionSetElement2(IPosition3DSet positionSet)
        {
            this.position3DSet = positionSet;
        }

        public void AddToScene(Visual3DCollection elementGroup)
        {
            //pointDrawer.ElementGroup = elementGroup;            

            meshGeneratorBase = new Petzold.Media3D.SphereMesh();
            sphere = meshGeneratorBase.Geometry;
            //Color materialColor = new DiffuseMaterial(new SolidColorBrush(value));
            material = new DiffuseMaterial(new SolidColorBrush(Colors.AliceBlue));

            //foreach (IPosition3D position in position3DSet)
            //{
            //    //pointDrawer.Add(position.GetX(),
            //    //    position.GetY(),
            //    //    position.GetZ());

            //    GeometryModel3D geometryModel3D = new GeometryModel3D(sphere, material);
            //    Transform3DGroup transform3DGroup = new Transform3DGroup();
            //    transform3DGroup.Children.Add(new ScaleTransform3D(0.5, 0.5, 0.5));
            //    TranslateTransform3D translateTransform3D = new TranslateTransform3D(
            //        position.GetX(), position.GetY(), position.GetZ());
            //    transform3DGroup.Children.Add(translateTransform3D);
            //    geometryModel3D.Transform = transform3DGroup;
            //    modelGroup.Children.Add(geometryModel3D);
            //}

            //var modelVisual3D = new ModelVisual3D();
            //modelVisual3D.Content = modelGroup;
            //elementGroup.Add(modelVisual3D);

            var positionSetMesh = new MeshGeometry3D();
            MeshGeometry3D sphereMesh = new Sphere().Geometry;

            foreach (IPosition3D position in position3DSet)
            {
                MeshGeometry3D mesh = WPF3DHelper.Translate(sphereMesh, new Vector3D(
                    position.GetX(), position.GetY(), position.GetZ()));

                WPF3DHelper.CombineTo(positionSetMesh, mesh);
            }

            GeometryModel3D positionSetModel = new GeometryModel3D(positionSetMesh, material);

            var modelVisual3D = new ModelVisual3D();
            modelVisual3D.Content = positionSetModel;
            elementGroup.Add(modelVisual3D);
        }
    }
}
