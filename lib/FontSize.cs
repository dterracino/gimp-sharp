// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2018 Maurits Rijk
//
// FontSize.cs
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

namespace Gimp
{
  public class FontSize
  {
    public double Size {get; set;}
    public Unit Unit {get; set;}

    public FontSize(double size, Unit unit)
    {
      Size = size;
      Unit = unit;
    }

    public override bool Equals(object o)
    {
      var fontsize = o as FontSize;
      return fontsize?.Size == Size && fontsize?.Unit == Unit;
    }

    public override int GetHashCode() => (int) Size + Unit.GetHashCode();

    public static bool operator==(FontSize fontsize1, 
				  FontSize fontsize2) =>
      fontsize1.Equals(fontsize2);

    public static bool operator!=(FontSize fontsize1, 
				  FontSize fontsize2) =>
      !(fontsize1 == fontsize2);

    public override string ToString() => $"({Size} {Unit})";
  }
}
