// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
//
// DuplicateDocumentEvent.cs
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
  public class DuplicateDocumentEvent : DuplicateEvent
  {
    [Parameter("Nm")]
    string _name;

    readonly string _which;

    public DuplicateDocumentEvent(DuplicateEvent srcEvent, string which) : 
      base(srcEvent) 
    {
      _which = which;
      Parameters.Fill(this);
    }

    public override string EventForDisplay =>
      base.EventForDisplay + " " + Abbreviations.Get(_which) + " document";

    protected override IEnumerable ListParameters()
    {
      if (_name != null)
	{
	  yield return "Name: \"" + _name + "\"";
	}
    }
    
    override public bool Execute()
    {
      ActiveImage = new Image(ActiveImage);
      new Display(ActiveImage);

      // Fix me: fill in name into image.

      return true;
    }
  }
}
