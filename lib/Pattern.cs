// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2007 Maurits Rijk
//
// Pattern.cs
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

using System;
using System.Runtime.InteropServices;

namespace Gimp
{
  public sealed class Pattern
  {
    readonly string _name;

    public Pattern(string name)
    {
      _name = name;
    }

    internal Pattern(string name, bool unused)
    {
      _name = name;
    }

    public string Name
    {
      get {return _name;}
    }

    public override bool Equals(object o)
    {
      if (o is Pattern)
	{
	  return (o as Pattern)._name == _name;
	}
      return false;
    }

    public override int GetHashCode()
    {
      return _name.GetHashCode();
    }

    public void GetInfo(out int width, out int height, out int bpp)
    {
      if (!gimp_pattern_get_info(_name, out width, out height, out bpp))
        {
	  throw new GimpSharpException();
        }
    }

    public void GetPixels(out int width, out int height, out int bpp,
			  out int numColorBytes)
    {
      IntPtr colorBytes;
      if (!gimp_pattern_get_pixels(_name, out width, out height,
				   out bpp, out numColorBytes,
				   out colorBytes))
        {
	  throw new GimpSharpException();
        }
      // Fix me: fill array with colors
    }

    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_pattern_get_info(string name,
                                             out int width, out int height, 
					     out int bpp);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_pattern_get_pixels(string name,
					       out int width, out int height, 
					       out int bpp, 
					       out int num_color_bytes,
					       out IntPtr color_bytes);
  }
}
