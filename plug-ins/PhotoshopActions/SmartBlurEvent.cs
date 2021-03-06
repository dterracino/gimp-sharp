// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
//
// SmartBlurEvent.cs
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
  public class SmartBlurEvent : ActionEvent
  {
    [Parameter("Rds")]
    double _radius;
    [Parameter("Thsh")]
    double _threshold;
    [Parameter("SmBQ")]
    EnumParameter _quality;
    [Parameter("SmBM")]
    EnumParameter _mode;

    protected override IEnumerable ListParameters()
    {
      yield return String.Format("Radius: {0:F3}", _radius);
      yield return String.Format("Threshold: {0:F3}", _threshold);
      yield return "Quality: " + Abbreviations.Get(_quality.Value);
      yield return "Mode: " + Abbreviations.Get(_mode.Value);
    }

    override public bool Execute()
    {
      RunProcedure("plug_in_sel_gauss", _radius, (int) _threshold);

      return true;
    }
  }
}
