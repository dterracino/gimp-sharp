// The Pointillize plug-in
// Copyright (C) 2006 Maurits Rijk
//
// ColorCoordinateSet.cs
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
using System.Collections.Generic;

namespace Gimp.Pointillize
{
  public class ColorCoordinateSet : CoordinateList<int>
  {
    readonly int _cellSize;
    readonly int _width;
    readonly int _height;
    readonly int _matrixRows;
    readonly int _matrixColumns;

    readonly byte[] _backgroundColor;

    readonly List<ColorCoordinate>[,] _matrix;

    Random _random = new Random();

    public ColorCoordinateSet(Drawable drawable, int cellSize)
    {
      _cellSize = cellSize;

      _backgroundColor = Context.Background.Bytes;

      PixelFetcher pf = new PixelFetcher(drawable, false);

      _width = drawable.Width;
      _height = drawable.Height;
      
      int nrOfCells = (int) (2.5 * _width * _height / (_cellSize * _cellSize));

      _matrixColumns = (int) Math.Sqrt(nrOfCells * _width / 8.0 / _height);
      _matrixRows = _matrixColumns * _height / _width;

      _matrixColumns = Math.Max(_matrixColumns, 1);
      _matrixRows = Math.Max(_matrixRows, 1);
      
      _matrix = new List<ColorCoordinate>[_matrixRows, _matrixColumns];

      for (int i = 0; i < nrOfCells; i++)
	{
	  int x = _random.Next(0, _width - 1);
	  int y = _random.Next(0, _height - 1);
	  byte[] color = new byte[drawable.Bpp];
	  pf.GetPixel(x, y, color);

	  // Add some noise
	  AddNoise(ref color[0]);
	  AddNoise(ref color[1]);
	  AddNoise(ref color[2]);

	  ColorCoordinate coordinate = new ColorCoordinate(x, y, color);
	  Add(coordinate);

	  int row = y * _matrixRows / _height;
	  int col = x * _matrixColumns / _width;
	  
	  if (_matrix[row, col] == null)
	    {
	      _matrix[row, col] = new List<ColorCoordinate>();
	    }
	  _matrix[row, col].Add(coordinate);

	  int top = row * _height / _matrixRows;
	  int left = col * _width / _matrixColumns;
	  int bottom = (row + 1) * _height / _matrixRows;
	  int right = (col + 1) * _width / _matrixColumns;
	  
	  Intersects(left, top, col - 1, row - 1, coordinate);
	  Intersects(x, top, col, row - 1, coordinate);
	  Intersects(right, top, col + 1, row - 1, coordinate);
	  Intersects(left, y, col - 1, row, coordinate);
	  Intersects(right, y, col + 1, row, coordinate);
	  Intersects(left, bottom, col - 1, row + 1, coordinate);
	  Intersects(x, bottom, col, row + 1, coordinate);
	  Intersects(right, bottom, col + 1, row + 1, coordinate);
	}

      pf.Dispose();
    }

    void AddNoise(ref byte channel)
    {
      int x = channel + _random.Next(0, 10) - 5;
      channel = (byte) Math.Min(255, Math.Max(x, 0));
    }

    void Intersects(int x, int y, int col, int row, ColorCoordinate coordinate)
    {
      if (col < 0 || col >= _matrixColumns || row < 0 || row >= _matrixRows)
	{
	  return;
	}
      
      if (coordinate.Distance(x, y) < _cellSize * _cellSize / 4)
	{
	  if (_matrix[row, col] == null)
	    {
	      _matrix[row, col] = new List<ColorCoordinate>();
	    }
	  _matrix[row, col].Add(coordinate);
	}
    }

    public byte[] GetColor(int x, int y)
    {
      int distance = int.MaxValue;
      ColorCoordinate closest = null;

      int row = y * _matrixRows / _height;
      int col = x * _matrixColumns / _width;
      
      List<ColorCoordinate> list = _matrix[row, col];

      if (list == null)
	{
	  return _backgroundColor;
	}

      foreach (ColorCoordinate coordinate in list)
	{
	  int d = coordinate.Distance(x, y);
	  if (d < distance)
	    {
	      distance = d;
	      closest = coordinate;
	    }
	}

      return distance < _cellSize * _cellSize / 4
	? closest.Color : _backgroundColor;
    }
  }
}