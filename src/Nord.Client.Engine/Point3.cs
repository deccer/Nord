using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Nord.Client.Engine
{
    [DebuggerDisplay("{" + nameof(DebugDisplayString) + ",nq}")]
    public struct Point3 : IEquatable<Point3>, IEquatableByRef<Point3>
    {
        public static readonly Point3 Zero = new Point3();

        public static readonly Point3 NaN = new Point3(float.NaN, float.NaN, float.NaN);

        public float X;

        public float Y;

        public float Z;

        public Point3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static bool operator ==(Point3 first, Point3 second) => first.Equals(ref second);

        public bool Equals(Point3 point) => Equals(ref point);

        public bool Equals(ref Point3 point) =>
            // ReSharper disable CompareOfFloatsByEqualityOperator
            (point.X == X) && (point.Y == Y) && (point.Z == Z);
        // ReSharper restore CompareOfFloatsByEqualityOperator

        public override bool Equals(object obj) => obj is Point3 point3 && Equals(point3);

        public static bool operator !=(Point3 first, Point3 second) => !(first == second);

        public static Point3 operator +(Point3 point, Vector3 vector) => Add(point, vector);

        public static Point3 Add(Point3 point, Vector3 vector)
        {
            Point3 p;
            p.X = point.X + vector.X;
            p.Y = point.Y + vector.Y;
            p.Z = point.Z + vector.Z;
            return p;
        }

        public static Point3 operator -(Point3 point, Vector3 vector) => Subtract(point, vector);

        public static Point3 Subtract(Point3 point, Vector3 vector)
        {
            Point3 p;
            p.X = point.X - vector.X;
            p.Y = point.Y - vector.Y;
            p.Z = point.Z - vector.Z;
            return p;
        }

        public static Vector3 operator -(Point3 point1, Point3 point2) => Displacement(point1, point2);

        public static Vector3 Displacement(Point3 point2, Point3 point1)
        {
            Vector3 vector;
            vector.X = point2.X - point1.X;
            vector.Y = point2.Y - point1.Y;
            vector.Z = point2.Z - point1.Z;
            return vector;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                hash = hash * 23 + Z.GetHashCode();
                return hash;
            }
        }

        public static Point3 Minimum(Point3 first, Point3 second) =>
            new Point3(first.X < second.X ? first.X : second.X,
                first.Y < second.Y ? first.Y : second.Y,
                first.Z < second.Z ? first.Z : second.Z);

        public static Point3 Maximum(Point3 first, Point3 second) =>
            new Point3(first.X > second.X ? first.X : second.X,
                first.Y > second.Y ? first.Y : second.Y,
                first.Z > second.Z ? first.Z : second.Z);

        public static implicit operator Vector3(Point3 point) => new Vector3(point.X, point.Y, point.Z);

        public static implicit operator Point3(Vector3 vector) => new Point3(vector.X, vector.Y, vector.Z);

        public override string ToString() => $"({X}, {Y}, {Z})";

        internal string DebugDisplayString => ToString();
    }
}
