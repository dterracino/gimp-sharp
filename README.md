Gimp#
=====

- I have only tested my code with GIMP 2.8. Gimp# will NOT work with GIMP 2.0.x! 
  It might still work with GIMP 2.4 and 2.6.

- Gimp# is an API wrapper around GIMP, written in C#. However, it's
not just a wrapper. It also adds a thin layer which adds C# specific
features, like iterating through a collection.  An example of this is
an iteration through the guides of an image. In C# this looks like:

```csharp
   foreach (Guide guide in image.Guides)
   {
	// Do something
   }
```
	
or even:

```csharp
   image.Guides.ForEach(guide => // Do something);
```
		
In C this would have been a bit more verbose:

```csharp
   gint32 guide_ID = 0:
   while ((guide_ID = gimp_image_find_next_guide(image_ID, guide_ID)) != 0)
   {
	// Do something
   }
```

- Gimp# also offers a base plug-in class which does the difficult
stuff for you. Implementing a new plug-in is just a matter of
overriding a few virtual methods. See the samples directory for
examples of how to do this.

- Gimp# fills the niche between scripting languages (easy to write,
slow) and C (harder to write, fast). If you need a quick and dirty
plug-in where speed doesn't matter that much, write it in any of the
scripting languages that come with GIMP (Scheme, Perl,
etc.). Scripting languages are very well fitted for calling existing
functionality, shortening manual tasks. You probably don't want pixel
manipulation in Scheme. In C, on the other hand, it takes a lot more
time to create a plug-in for several reasons: building the GUI is time
consuming.  Secondly, pixel handling is not completely trivial. You
have to know how to traverse through the tiles of an image,
etc. Typically the actual algorithm is only a very small (5 - 20 %)
part of the total code. Gimp# is not as fast as C, but much faster
than a scripting language. Building a decent GUI is much easier than
in C.
