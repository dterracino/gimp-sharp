// The Pointillize plug-in
// Copyright (C) 2006 Maurits Rijk
//
// Pointillize.cs
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

using Gtk;

namespace Gimp.Pointillize
{
  public class Pointillize : PluginWithPreview
  {
    [SaveAttribute("cell_size")]
    int _cellSize = 30;

    ColorCoordinateSet _coordinates;

    static void Main(string[] args)
    {
      new Pointillize(args);
    }

    public Pointillize(string[] args) : base(args)
    {
    }

    override protected ProcedureSet GetProcedureSet()
    {
      ProcedureSet set = new ProcedureSet();

      ParamDefList in_params = new ParamDefList();
      in_params.Add(new ParamDef("cell_size", 30, typeof(int),
				 "Cell size"));

      Procedure procedure = new Procedure("plug_in_pointillize",
					  "Create pointillist paintings",
					  "Create pointillist paintings",
					  "Maurits Rijk",
					  "(C) Maurits Rijk",
					  "2006",
					  "Pointillize...",
					  "RGB*, GRAY*",
					  in_params);
      procedure.MenuPath = "<Image>/Filters/Artistic";
      // procedure.IconFile = "Pointillize.png";

      set.Add(procedure);

      return set;
    }

    override protected bool CreateDialog()
    {
      Dialog dialog = DialogNew("Pointillize", "Pointillize", IntPtr.Zero, 0,
				Gimp.StandardHelpFunc, "Pointillize");

      GimpTable table = new GimpTable(1, 3, false);

      ScaleEntry entry = new ScaleEntry(table, 0, 1, "Cell _Size:", 150, 3,
					_cellSize, 3.0, 300.0, 1.0, 8.0, 0,
					true, 0, 0, null, null);
      entry.ValueChanged += delegate(object sender, EventArgs e)
	{
	  _cellSize = entry.ValueAsInt;
	  InvalidatePreview();
	};

      Vbox.PackStart(table, false, false, 0);
      
      dialog.ShowAll();
      return DialogRun();
    }

    override protected void UpdatePreview(AspectPreview preview)
    {
      // move generic code from ncp to base class
    }

    override protected void Render(Drawable drawable)
    {
      Initialize(drawable);
      RgnIterator iter = new RgnIterator(drawable, RunMode.Interactive);
      iter.Progress = new Progress("Pointillize");
      iter.IterateDest(DoPointillize);
			
      Display.DisplaysFlush();
    }

    void Initialize(Drawable drawable)
    {
      _coordinates = new ColorCoordinateSet(drawable, _cellSize);
    }

    byte[] DoPointillize(int x, int y)
    {
      return _coordinates.GetColor(x, y);
    }
  }
}