using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace MolecularThermalmotion
{
    public class PhysicEngine
    {
        public static void UpdatePositionByVelocity(ref Point3D position, ref Point3D oldPosition, ref Vector3D velocity, double t)
        {
            oldPosition = position;
            position.X += velocity.X * t;
            position.Y += velocity.Y * t;
            position.Z += velocity.Z * t;
        }

        public static void UpdateVelocityByForce(ref Vector3D velocity, ref Vector3D force, double mass, double t)
        {

        }

        public static void UpdateVelocityByCollide(Point3D position1, Point3D position2, ref Vector3D velocity1, ref Vector3D velocity2, double mass1, double mass2)
        {
            Vector3D v1 = velocity1;
            velocity1 = velocity2;
            velocity2 = v1;
        }
    }   
}
