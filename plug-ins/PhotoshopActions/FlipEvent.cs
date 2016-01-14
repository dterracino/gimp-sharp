// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
//
// FlipEvent.cs
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
using System.Collections;

namespace Gimp.PhotoshopActions
{
  public class FlipEvent : ActionEvent
  {
    [Parameter("Axis")]
    EnumParameter _axis;

    public override string EventForDisplay =>
      base.EventForDisplay + " current layer";

    protected override IEnumerable ListParameters()
    {
      yield return Format(_axis, "Axis");
    }

    override public bool Execute()
    {
#if false
      var image = ActiveImage;

      if (_axis.Value == "Hrzn")
	{
	  image.Flip(OrientationType.Horizontal);
	}
      else
	{
	  image.Flip(OrientationType.Vertical);
	}
#else	// We need to flip the drawable, not the whole image!


      var drawable = ActiveImage.ActiveDrawable;

      if (_axis.Value == "Hrzn")
	{
	  drawable.TransformFlipSimple(OrientationType.Horizontal, true,
				       0.0, false);
	}
      else
	{
	  drawable.TransformFlipSimple(OrientationType.Vertical, true,
				       0.0, false);
	}      
#endif
      return true;
    }
  }
}
