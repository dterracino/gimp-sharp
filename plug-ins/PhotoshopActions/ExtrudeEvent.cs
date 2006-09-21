// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// ExtrudeEvent.cs
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

using System.Collections;

namespace Gimp.PhotoshopActions
{
  public class ExtrudeEvent : ActionEvent
  {
    [Parameter("ExtS")]
    int _extrudeSize;
    [Parameter("ExtD")]
    int _extrudeDepth;
    [Parameter("ExtF")]
    bool _extrudeSolidFace;
    [Parameter("ExtM")]
    bool _extrudeMaskIncomplete;
    [Parameter("ExtT")]
    EnumParameter _extrudeType;
    [Parameter("ExtR")]
    EnumParameter _extrudeRandom;

    public override bool IsExecutable
    {
      get {return false;}
    }

    protected override IEnumerable ListParameters()
    {
      yield return "Extrude Size: " + _extrudeSize;
      yield return "Extrude Depth: " + _extrudeDepth;
      yield return "Extrude Solid Face: " + _extrudeSolidFace;
      yield return "Extrude Mask Incomplete: " + _extrudeMaskIncomplete;
      yield return "Extrude Type: " + Abbreviations.Get(_extrudeType.Value);
      yield return "Extrude Random: " + 
	Abbreviations.Get(_extrudeRandom.Value);
    }

    override public bool Execute()
    {
      return true;
    }
  }
}