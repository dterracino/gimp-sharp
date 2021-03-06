// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
//
// ParameterSet.cs
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
using System.Reflection;

namespace Gimp.PhotoshopActions
{
  public class ParameterSet
  {
    Dictionary<string, Parameter> _set = new Dictionary<string, Parameter>();
    List<Parameter> Parameters {get;} = new List<Parameter>();

    public int Count => _set.Count;

    public Parameter this[string name]
    {
      get 
	{
	  Parameter parameter;
#if false
	  return (_set.TryGetValue(name, out parameter)) ? parameter : null;
#else
	  _set.TryGetValue(name, out parameter);
	  if (parameter == null)
	    {
	      foreach (Parameter p in this)
		{
		  if (p is ObjcParameter)
		    {
		      Parameter found = (p as ObjcParameter).Parameters[name];
		      if (found != null)
			{
			  return found;
			}
		    }
		}
	      return null;
	    }
	  else
	    {
	      return parameter;
	    }
#endif
	}
    }

    public IEnumerator<Parameter> GetEnumerator() => Parameters.GetEnumerator();

    public void Parse(ActionParser parser, int numberOfItems)
    {
      for (int i = 0; i < numberOfItems; i++)
	{
	  var parameter = parser.ReadItem();
	  if (parameter != null)
	    {
	      _set[parameter.Name] = parameter;
	      Parameters.Add(parameter);
	    }
	}
    }

    public void Parse(ActionParser parser, Object obj, int numberOfItems)
    {
      Parse(parser, numberOfItems);
      Fill(obj);
    }

    public IEnumerable<string> ListParameters()
    {
      foreach (var child in Parameters)
	{
	  if (child.Name != "null")
	    {
	      foreach (var s in child.Format())
		{
		  yield return s;
		}
	    }
	}
    }

    public void Fill(Object obj)
    {
      var type = obj.GetType();

      // Console.WriteLine("Type: " + type);

      foreach (var field in type.GetFields(BindingFlags.Instance |
					   BindingFlags.NonPublic | 
					   BindingFlags.Public))
	{
	  // Console.WriteLine("Fill: " + field);

	  foreach (var attribute in field.GetCustomAttributes(true))
	    {
	      if (attribute is ParameterAttribute)
		{
		  var parameterAttribute = attribute as ParameterAttribute;

		  string name = parameterAttribute.Name;
		  if (_set.ContainsKey(name))
		    {
		      var parameter = _set[name];
		      parameter.Fill(obj, field);
		    }
		  else
		    {
		      // Console.WriteLine("ParameterSet::Fill " + name);
		    }
		}
	    }
	}
      // Console.WriteLine("Done");
    }
  }
}
