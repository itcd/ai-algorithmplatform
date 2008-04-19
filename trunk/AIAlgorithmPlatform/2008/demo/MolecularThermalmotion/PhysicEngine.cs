using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using System.Diagnostics;

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
            velocity.X -= (force.X / mass) * t;
            velocity.Y -= (force.Y / mass) * t;
            velocity.Z -= (force.Z / mass) * t;
        }

        private static Vector3D pr(Vector3D a, Vector3D b)
        {
            if (a.Length != 0)
            {
                a.Normalize();
                double ba = b.X * a.X + b.Y * a.Y + b.Z * a.Z;
                return ba * a;
            }
            return b;

        }

        public static void UpdateVelocityByCollide(Point3D position1, Point3D position2, ref Vector3D velocity1, ref Vector3D velocity2, double mass1, double mass2, double r1, double r2)
        {
            //velocity1 = new Vector3D(0, 0, 0);
            //velocity2 = new Vector3D(0, 0, 0);

            //Vector3D v1 = velocity1;
            //velocity1 = velocity2;
            //velocity2 = v1;
            //Debug.WriteLine(velocity1.Length.ToString() + "  " + velocity2.Length.ToString(), "Before:");

            Vector3D normalVector = new Vector3D(position1.X - position2.X, position1.Y - position2.Y, position1.Z - position2.Z);

            Vector3D n0 = normalVector;
            n0.Normalize();

            double ba1 = Ba(ref velocity1, ref n0);
            double ba2 = Ba(ref velocity2, ref n0);

            Vector3D pr1 = ba1 * n0;
            Vector3D pr2 = ba2 * n0;

            //Vector3D n0 = normalVector, n1 = pr1, n2 = pr2;
            //n0.Normalize();
            //n1.Normalize();
            //n2.Normalize();

            if (((ba1 < 0 && ba2 > 0) ||
                (ba1 < 0 && pr1.Length > pr2.Length) ||
                (ba2 > 0 && pr2.Length > pr1.Length)))
            {
                Vector3D v1s = velocity1 - pr1 + pr2;
                Vector3D v2s = velocity2 - pr2 + pr1;
                velocity1 = v1s;
                velocity2 = v2s;
            }

            //Debug.WriteLine(velocity1.Length.ToString() + "  " + velocity2.Length.ToString(), "After:");
        }

        private static double Ba(ref Vector3D velocity1, ref Vector3D n0)
        {
            double ba = velocity1.X * n0.X + velocity1.Y * n0.Y + velocity1.Z * n0.Z;
            return ba;
        }
    }   
}
