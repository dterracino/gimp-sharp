// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// TestBrush.cs
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

using NUnit.Framework;

namespace Gimp
{
  [TestFixture]
  public class TestBrush
  {
    [Test]
    public void New()
    {
      string brushName = "Gimp#Brush";
      int count = new BrushList(null).Count;
      Brush brush = new Brush(brushName);
      Assert.AreEqual(brushName, brush.Name);
      Assert.AreEqual(count + 1, new BrushList(null).Count);
      brush.Delete();
    }

    [Test]
    public void GetInfo()
    {
      string brushName = "Gimp#Brush";
      Brush brush = new Brush(brushName);
      int width, height, mask_bpp, color_bpp;
      brush.GetInfo(out width, out height, out mask_bpp, out color_bpp);
      Assert.AreEqual(brushName, brush.Name);
      // Assert.AreEqual(0, width);
      // Fix me: insert more Asserts here!
      brush.Delete();
    }
  }
}
