using System;

using Gtk;

namespace Gimp.PicturePackage
{
  public class PicturePackage : Plugin
  {
    LayoutSet _layoutSet = new LayoutSet();
    Layout _layout;

    ProviderFactory _loader;

    DocumentFrame _df;
    Preview _preview;

    [SaveAttribute]
    bool _flatten = false;

    [SaveAttribute]
    int _resolution = 72;

    // [SaveAttribute]
    string _label;

    [SaveAttribute]
    int _position;

    [STAThread]
    static void Main(string[] args)
    {
      new PicturePackage(args);
    }

    public PicturePackage(string[] args) : base(args)
    {
    }

    override protected void Query()
    {
      InstallProcedure("plug_in_picture_package",
		       "Picture package",
		       "Picture package",
		       "Maurits Rijk",
		       "Maurits Rijk",
		       "2004",
		       "Picture Package...",
		       "RGB*, GRAY*",
		       null);

      MenuRegister("plug_in_picture_package", "<Image>/Filters/Render");
    }

    override protected bool CreateDialog()
    {
      gimp_ui_init("PicturePackage", true);

      _layoutSet.Load();
      _loader = new FrontImageProviderFactory(_image);

      Dialog dialog = DialogNew("Picture Package", "PicturePackage",
				IntPtr.Zero, 0, null, "PicturePackage");

      HBox hbox = new HBox(false, 12);
      hbox.BorderWidth = 12;
      dialog.VBox.PackStart(hbox, true, true, 0);

      VBox vbox = new VBox(false, 12);
      hbox.PackStart(vbox, false, false, 0);

      SourceFrame sf = new SourceFrame(this);
      vbox.PackStart(sf, false, false, 0);

      _df = new DocumentFrame(this, _layoutSet);
      vbox.PackStart(_df, false, false, 0);

      LabelFrame lf = new LabelFrame(this);
      vbox.PackStart(lf, false, false, 0);

      Frame frame = new Frame();
      hbox.PackStart(frame, true, true, 0);

      VBox fbox = new VBox();
      fbox.BorderWidth = 12;
      frame.Add(fbox);

      _preview = new Preview(this);
      _preview.WidthRequest = 400;
      _preview.HeightRequest = 500;

      fbox.Add(_preview);

      _layoutSet.Selected = _layoutSet[0];
      _layout = _layoutSet[0];
      _layoutSet.SelectEvent += new SelectHandler(SetLayout);

      dialog.ShowAll();
	
      return DialogRun();
    }

    void SetLayout(Layout layout)
    {
      _layout = layout;
      RedrawPreview();
    }

    bool RenderOne()
    {
      Console.WriteLine("RenderOne");
      bool val = _layout.Render(_loader, _preview.GetRenderer(_layout), false);
      if (!val) _preview.QueueDraw();
      return val;
    }
    public void RenderX()
    {
      GLib.Idle.Add(new GLib.IdleHandler(RenderOne));
    }

    public void Render()
    {
      _layout.Render(_loader, _preview.GetRenderer(_layout));
    }

    void RedrawPreview()
    {
      Render();
      _preview.QueueDraw();
    }

    override protected void DoSomething(Image image)
    {
      _flatten = _df.Flatten;

       PageSize size = _layout.GetPageSizeInPixels(_resolution);

      int width = (int) size.Width;
      int height = (int) size.Height;
      Image composed = new Image(width, height, ImageBaseType.RGB);

      _layout.Render(_loader, 
		     new ImageRenderer(_layout, composed, _resolution));

      if (_flatten)
	{
	composed.Flatten();
	}

      new Display(composed);
      Display.DisplaysFlush();
    }

    public ProviderFactory Loader
    {
      set 
	  {
	  _loader = value;
	  RedrawPreview();
	  }
    }

    public Image Image
    {
      get {return _image;}
    }

    public string Label
    {
      set
	  {
	  _label = value;
	  _preview.DrawLabel(_position, _label);
	  }
    }

    public int Position
    {
      set
	  {
	  _position = value;
	  _preview.DrawLabel(_position, _label);
	  }
    }

    public int Resolution
    {
      set {_resolution = value;}
    }
  }
  }
