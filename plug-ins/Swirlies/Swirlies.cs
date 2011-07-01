// The Swirlies plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// Swirlies.cs
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
using System.Collections.Generic;
using System.Threading;

using Gtk;

namespace Gimp.Swirlies
{
  class Swirlies : PluginWithPreview
  {
    Random _random;
    int _width;
    int _height;
    List<Swirly> _swirlies = new List<Swirly>();

    ProgressBar _progress;

    Thread _renderThread;

    Variable<UInt32> _seed = new Variable<UInt32>
    ("seed", _("Value for random seed"), 0);
    Variable<bool> _randomSeed = new Variable<bool>
    ("random_seed", _("Use specified random seed"), false);
    Variable<int> _points = new Variable<int>("points", _("Fix me"), 3);

    static void Main(string[] args)
    {
      GimpMain<Swirlies>(args);
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      yield return new Procedure("plug_in_swirlies",
				 _("Generates 2D textures"),
				 _("Generates 2D textures"),
				 "Maurits Rijk",
				 "(C) Maurits Rijk",
				 "2006-2007",
				 _("Swirlies..."),
				 "RGB",
				 new ParamDefList(_points))
	{
	  MenuPath = "<Image>/Filters/Render",
	  IconFile = "Swirlies.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      var dialog = DialogNew(_("Swirlies"), _("swirlies"), IntPtr.Zero, 
			     0, null, _("swirlies"));
      // _preview.SetBounds(0, 0, 50, 50);

      _progress = new ProgressBar();
      Vbox.PackStart(_progress, false, false, 0);
      
      var table = new GimpTable(4, 3, false)
	{ColumnSpacing = 6, RowSpacing = 6};
      Vbox.PackStart(table, false, false, 0);

      var seed = new RandomSeed(_seed, _randomSeed);

      table.AttachAligned(0, 0, _("Random _Seed:"), 0.0, 0.5, seed, 2, true);

      new ScaleEntry(table, 0, 1, _("Po_ints:"), 
		     150, 3, _points, 1.0, 16.0, 1.0, 8.0, 0);

      _points.ValueChanged += delegate {InvalidatePreview();};

      return dialog;
    }

    override protected void UpdatePreview(AspectPreview preview)
    {
      return;
      if (_renderThread != null)
	{
	  _renderThread.Abort();
	  _renderThread.Join();
	}
      else 
	{
	  _renderThread = new Thread(new ThreadStart(MyUpdatePreview));
	  _renderThread.Start();
	}
    }

    // void UpdatePreview(object sender, EventArgs e)
    void MyUpdatePreview()
    {
      Initialize(_drawable);

      int width, height;
      Preview.GetSize(out width, out height);

      var buffer = new byte[width * height * 3];

      for (int y = 0; y < height; y++)
	{
	  int y_orig = _height * y / height;
	  for (int x = 0; x < width; x++)
	    {
	      long index = 3 * (y * width + x);
	      int x_orig = _width * x / width;

	      DoSwirlies(x_orig, y_orig).CopyTo(buffer, index);
	    }
	  Application.Invoke(delegate {
	    _progress.Update((double) y / height);
	  });
	}
      Preview.DrawBuffer(buffer, width * 3);
    }
    
    override protected void Reset()
    {
      Console.WriteLine("Reset!");
    }

    void Initialize(Drawable drawable)
    {
      _random = new Random((int) _seed.Value);
      Swirly.Random = _random;

      _width = drawable.Width;
      _height = drawable.Height;

      _swirlies.Clear();

      for (int i = 0; i < _points.Value; i++)
	_swirlies.Add(Swirly.CreateRandom());
    }

    override protected void Render(Drawable drawable)
    {
      Initialize(drawable);
      var iter = new RgnIterator(drawable, _("Swirlies"));
      iter.IterateDest(DoSwirlies);
    }

    Pixel DoSwirlies(int x, int y)
    {
      var rgb = new RGB(0.0, 0.0, 0.0);

      const double zoom = 0.5;
      const int terms = 5;

      _swirlies.ForEach(swirly => 
			swirly.CalculateOnePoint(terms, _width, _height, zoom, 
						 x, y, rgb));

      return new Pixel(FloatToIntPixel(RemapColorRange(rgb.R)),
		       FloatToIntPixel(RemapColorRange(rgb.G)),
		       FloatToIntPixel(RemapColorRange(rgb.B)));
    }
    
    double RemapColorRange(double val)
    {
      const double postGain = 0.35;
      const double preGain = 10000;

      val = Math.Abs(val);
      return Math.Tanh(postGain * Math.Log(1 + preGain * val));
    }

    int FloatToIntPixel(double val)
    {
      val *= 255;
      val += 1 - 2 * _random.NextDouble();
      val += 1 - 2 * _random.NextDouble();

      if (val < 0)
	{
	  return 0;
	}
      else if (val > 255)
	{
	  return 255;
	}
      else
	{
	  return (int) val;
	}
    }
  }
}
