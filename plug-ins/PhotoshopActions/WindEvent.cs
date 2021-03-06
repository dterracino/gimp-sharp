// The PhotoshopActions plug-in
// Copyright (C) 2006-2018 Maurits Rijk
//
// WindEvent.cs
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

namespace Gimp.PhotoshopActions
{
  public class WindEvent : ActionEvent
  {
    [Parameter("WndM")]
    EnumParameter _mode;
    [Parameter("Drct")]
    EnumParameter _direction;

    public override bool IsExecutable => _mode.Value == "Wnd";

    override public bool Execute()
    {
      int direction = (_direction.Value == "Lft") ? 1 : 0;
      int algorithm = (_mode.Value == "Wnd") ? 0 : 1;
      RunProcedure("plug_in_wind", 40, direction, 30, algorithm, 0);
      return true;
    }
  }
}
