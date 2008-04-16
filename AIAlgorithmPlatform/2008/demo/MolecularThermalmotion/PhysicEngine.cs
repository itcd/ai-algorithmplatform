﻿using System;
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

        }

        private static Vector3D pr(Vector3D a, Vector3D b)
        {
            if (a.Length != 0)
            {
                a = a / a.Length;
                double ba = b.X * a.X + b.Y * a.Y + b.Z * a.Z;
                return ba * a;
            }
            return b;

        }

        public static void UpdateVelocityByCollide(Point3D position1, Point3D position2, ref Vector3D velocity1, ref Vector3D velocity2, double mass1, double mass2, double r1, double r2)
        {
            //Vector3D v1 = velocity1;
            //velocity1 = velocity2;
            //velocity2 = v1;
            //Debug.WriteLine(velocity1.Length.ToString() + "  " + velocity2.Length.ToString(), "Before:");
            Vector3D normalVector = new Vector3D(position1.X - position2.X, position1.Y - position2.Y, position1.Z - position2.Z);
            if (normalVector.Length <= (r1 + r2) && (normalVector.Length > 0.00000001 || normalVector.Length < -0.0000001))
            {
                Vector3D v1s = velocity1 - pr(normalVector, velocity1) + pr(normalVector, velocity2);
                Vector3D v2s = velocity2 - pr(normalVector, velocity2) + pr(normalVector, velocity1);
                velocity1 = v1s;
                velocity2 = v2s;
            }
            //Debug.WriteLine(velocity1.Length.ToString() + "  " + velocity2.Length.ToString(), "After:");
        }
    }   
}