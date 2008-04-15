using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace MolecularThermalmotion
{
    public class Molecule
    {
        public double radius;

        public Point3D position;

        public Point3D oldPosition;

        public Vector3D currentVelocity;

        double mass;

        GeometryModel3D moleculeGeometryModel;
        public GeometryModel3D MoleculeGeometryModel
        {
            get { return moleculeGeometryModel; }
            set { moleculeGeometryModel = value; }
        }
    }
}
