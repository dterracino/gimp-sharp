// The PicturePackage plug-in
// Copyright (C) 2004-2006 Maurits Rijk
//
// FileImageProviderFactory.cs
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

namespace Gimp.PicturePackage
{
  public class FileImageProviderFactory : ProviderFactory
  {
    ImageProvider _provider;

    public FileImageProviderFactory(string filename)
    {
      _provider = new FileImageProvider(filename);
    }

    public override ImageProvider Provide()
    {
      return _provider;
    }

    public override void Cleanup()
    {
//      Console.WriteLine("Cleanup: " + _provider);
      _provider.Release();
    }
  }
}
