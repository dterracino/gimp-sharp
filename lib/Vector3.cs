// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2018 Maurits Rijk
//
// Vector3.cs
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.
//

using System.Runtime.InteropServices;

namespace Gimp
{
  public class Vector3
  {
    GimpVector3 _vector;

    public double Length => gimp_vector3_length(ref _vector);

    public Vector3(double x = 0.0, double y = 0.0, double z = 0.0)
    {
      _vector = gimp_vector3_new(x, y, z);
    }

    Vector3(Vector3 vector) : this(vector.X, vector.Y, vector.Z)
    {
    }

    Vector3(GimpVector3 vector)
    {
      _vector = vector;
    }

    public void Set(double x, double y, double z)
    {
      gimp_vector3_set(ref _vector, x, y, z);
    }

    public double X
    {
      get => _vector.x;
      set {_vector.x = value;}
    }

    public double Y
    {
      get => _vector.y;
      set {_vector.y = value;}
    }

    public double Z
    {
      get => _vector.z;
      set {_vector.z = value;}
    }

    public override bool Equals(object o)
    {
      var vector = o as Vector3;
      return vector?.X == X && vector?.Y == Y && vector?.Z == Z;
    }

    public override int GetHashCode() =>
      X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode();

    public Vector3 Mul(double factor)
    {
      gimp_vector3_mul(ref _vector, factor);
      return this;
    }

    public void Normalize()
    {
      gimp_vector3_normalize(ref _vector);
    }

    public Vector3 Neg()
    {
      gimp_vector3_neg(ref _vector);
      return this;
    }

    public Vector3 Add(Vector3 vector)
    {
      gimp_vector3_add(ref _vector, ref _vector, ref vector._vector);
      return this;
    }

    public Vector3 Sub(Vector3 vector)
    {
      gimp_vector3_sub(ref _vector, ref _vector, ref vector._vector);
      return this;
    }

    public static Vector3 operator + (Vector3 v1, Vector3 v2) =>
      (new Vector3(v1)).Add(v2);

    public static Vector3 operator - (Vector3 v1, Vector3 v2) =>
      (new Vector3(v1)).Sub(v2);

    public static Vector3 operator - (Vector3 vector) =>
      (new Vector3(vector)).Neg();

    public static Vector3 operator * (Vector3 vector, double factor) =>
      (new Vector3(vector)).Mul(factor);

    public double InnerProduct(Vector3 vector) =>
      gimp_vector3_inner_product(ref _vector, ref vector._vector);

    public double CrossProduct(Vector3 vector) =>
      gimp_vector3_cross_product(ref _vector, ref vector._vector);

    public void Rotate(double alpha, double beta, double gamma) =>
      gimp_vector3_rotate(ref _vector, alpha, beta, gamma);

    public override string ToString() => $"({X}, {Y}, {Z})";

    [DllImport("libgimpmath-2.0-0.dll")]
    static extern GimpVector3 gimp_vector3_new(double x,
					       double y,
					       double z);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern void gimp_vector3_set(ref GimpVector3 vector,
					double x,
					double y,
					double z);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern double gimp_vector3_length(ref GimpVector3 vector);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern void gimp_vector3_mul(ref GimpVector3 vector,
					double factor);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern void gimp_vector3_normalize(ref GimpVector3 vector);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern void gimp_vector3_neg(ref GimpVector3 vector);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern void gimp_vector3_add(ref GimpVector3 result,
					ref GimpVector3 vector1,
					ref GimpVector3 vector2);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern void gimp_vector3_sub(ref GimpVector3 result,
					ref GimpVector3 vector1,
					ref GimpVector3 vector2);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern double gimp_vector3_inner_product(ref GimpVector3 vector1,
						    ref GimpVector3 vector2);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern double gimp_vector3_cross_product(ref GimpVector3 vector1,
						    ref GimpVector3 vector2);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern void gimp_vector3_rotate(ref GimpVector3 vector,
					   double alpha,
					   double beta,
					   double gamma);
  }

  [StructLayout(LayoutKind.Sequential)]
  internal struct GimpVector3
  {
    public double x, y, z;
  }
}
