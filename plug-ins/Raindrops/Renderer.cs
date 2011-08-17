// The Raindrops plug-in
// Copyright (C) 2004-2011 Maurits Rijk, Massimo Perga
//
// Renderer.cs
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//

using System;

namespace Gimp.Raindrops
{
  public class Renderer
  {
    readonly VariableSet _variables;

    public Renderer(VariableSet variables)
    {
      _variables = variables;
    }

    public void Render(Image image, Drawable drawable, Progress progress)
    {
      var dimensions = image.Dimensions;

      Tile.CacheDefault(drawable);
      var pf = new PixelFetcher(drawable, false);

      var iter = new RgnIterator(drawable, RunMode.Interactive);
      iter.IterateSrcDest(src => src);

      int dropSize = _variables.GetValue<int>("drop_size");
      int fishEye = _variables.GetValue<int>("fish_eye");
      int number = _variables.GetValue<int>("number");

      var factory = new RaindropFactory(dropSize, fishEye, dimensions);
      for (int numBlurs = 0; numBlurs <= number; numBlurs++)
	{
	  var raindrop = factory.Create();
	  if (raindrop == null)
	    {
	      if (progress != null)
		progress.Update(1.0);
	      break;
	    }

	  raindrop.Render(factory.BoolMatrix, pf, drawable);

	  if (progress != null)
	    progress.Update((double) numBlurs / number);
	}

      pf.Dispose();

      drawable.Flush();
      drawable.Update();
    }
  }
}
