// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.42
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace MonoDevelop.SourceEditor.OptionPanels {
    
    
    public partial class GeneralOptionsPanel {
        
        private Gtk.VBox vbox1;
        
        private Gtk.CheckButton codeCompletioncheckbutton;
        
        private Gtk.CheckButton quickFinderCheckbutton;
        
        private Gtk.CheckButton foldingCheckbutton;
        
        private Gtk.Frame frame2;
        
        private Gtk.Alignment GtkAlignment1;
        
        private Gtk.VBox vbox3;
        
        private Gtk.RadioButton radiobutton1;
        
        private Gtk.HBox hbox1;
        
        private Gtk.RadioButton radiobutton2;
        
        private Gtk.FontButton fontselection;
        
        private Gtk.Label GtkLabel13;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget MonoDevelop.SourceEditor.OptionPanels.GeneralOptionsPanel
            Stetic.BinContainer.Attach(this);
            this.Name = "MonoDevelop.SourceEditor.OptionPanels.GeneralOptionsPanel";
            // Container child MonoDevelop.SourceEditor.OptionPanels.GeneralOptionsPanel.Gtk.Container+ContainerChild
            this.vbox1 = new Gtk.VBox();
            this.vbox1.Name = "vbox1";
            this.vbox1.Spacing = 6;
            // Container child vbox1.Gtk.Box+BoxChild
            this.codeCompletioncheckbutton = new Gtk.CheckButton();
            this.codeCompletioncheckbutton.CanFocus = true;
            this.codeCompletioncheckbutton.Name = "codeCompletioncheckbutton";
            this.codeCompletioncheckbutton.Label = Mono.Unix.Catalog.GetString("_Code completion");
            this.codeCompletioncheckbutton.DrawIndicator = true;
            this.codeCompletioncheckbutton.UseUnderline = true;
            this.vbox1.Add(this.codeCompletioncheckbutton);
            Gtk.Box.BoxChild w1 = ((Gtk.Box.BoxChild)(this.vbox1[this.codeCompletioncheckbutton]));
            w1.Position = 0;
            w1.Expand = false;
            w1.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this.quickFinderCheckbutton = new Gtk.CheckButton();
            this.quickFinderCheckbutton.CanFocus = true;
            this.quickFinderCheckbutton.Name = "quickFinderCheckbutton";
            this.quickFinderCheckbutton.Label = Mono.Unix.Catalog.GetString("C_lass & Method quick finder");
            this.quickFinderCheckbutton.DrawIndicator = true;
            this.quickFinderCheckbutton.UseUnderline = true;
            this.vbox1.Add(this.quickFinderCheckbutton);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.vbox1[this.quickFinderCheckbutton]));
            w2.Position = 1;
            w2.Expand = false;
            w2.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this.foldingCheckbutton = new Gtk.CheckButton();
            this.foldingCheckbutton.CanFocus = true;
            this.foldingCheckbutton.Name = "foldingCheckbutton";
            this.foldingCheckbutton.Label = Mono.Unix.Catalog.GetString("Code _folding");
            this.foldingCheckbutton.DrawIndicator = true;
            this.foldingCheckbutton.UseUnderline = true;
            this.vbox1.Add(this.foldingCheckbutton);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.vbox1[this.foldingCheckbutton]));
            w3.Position = 2;
            w3.Expand = false;
            w3.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this.frame2 = new Gtk.Frame();
            this.frame2.Name = "frame2";
            this.frame2.ShadowType = ((Gtk.ShadowType)(0));
            // Container child frame2.Gtk.Container+ContainerChild
            this.GtkAlignment1 = new Gtk.Alignment(0F, 0F, 1F, 1F);
            this.GtkAlignment1.Name = "GtkAlignment1";
            this.GtkAlignment1.LeftPadding = ((uint)(12));
            // Container child GtkAlignment1.Gtk.Container+ContainerChild
            this.vbox3 = new Gtk.VBox();
            this.vbox3.Name = "vbox3";
            this.vbox3.Spacing = 6;
            // Container child vbox3.Gtk.Box+BoxChild
            this.radiobutton1 = new Gtk.RadioButton(Mono.Unix.Catalog.GetString("_Default monospace"));
            this.radiobutton1.CanFocus = true;
            this.radiobutton1.Name = "radiobutton1";
            this.radiobutton1.Active = true;
            this.radiobutton1.DrawIndicator = true;
            this.radiobutton1.UseUnderline = true;
            this.radiobutton1.Group = new GLib.SList(System.IntPtr.Zero);
            this.vbox3.Add(this.radiobutton1);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(this.vbox3[this.radiobutton1]));
            w4.Position = 0;
            w4.Expand = false;
            w4.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.hbox1 = new Gtk.HBox();
            this.hbox1.Name = "hbox1";
            this.hbox1.Spacing = 12;
            // Container child hbox1.Gtk.Box+BoxChild
            this.radiobutton2 = new Gtk.RadioButton(Mono.Unix.Catalog.GetString("_Custom"));
            this.radiobutton2.CanFocus = true;
            this.radiobutton2.Name = "radiobutton2";
            this.radiobutton2.DrawIndicator = true;
            this.radiobutton2.UseUnderline = true;
            this.radiobutton2.Group = this.radiobutton1.Group;
            this.hbox1.Add(this.radiobutton2);
            Gtk.Box.BoxChild w5 = ((Gtk.Box.BoxChild)(this.hbox1[this.radiobutton2]));
            w5.Position = 0;
            w5.Expand = false;
            w5.Fill = false;
            // Container child hbox1.Gtk.Box+BoxChild
            this.fontselection = new Gtk.FontButton();
            this.fontselection.CanFocus = true;
            this.fontselection.Name = "fontselection";
            this.hbox1.Add(this.fontselection);
            Gtk.Box.BoxChild w6 = ((Gtk.Box.BoxChild)(this.hbox1[this.fontselection]));
            w6.Position = 1;
            this.vbox3.Add(this.hbox1);
            Gtk.Box.BoxChild w7 = ((Gtk.Box.BoxChild)(this.vbox3[this.hbox1]));
            w7.Position = 1;
            w7.Expand = false;
            w7.Fill = false;
            this.GtkAlignment1.Add(this.vbox3);
            this.frame2.Add(this.GtkAlignment1);
            this.GtkLabel13 = new Gtk.Label();
            this.GtkLabel13.Name = "GtkLabel13";
            this.GtkLabel13.LabelProp = Mono.Unix.Catalog.GetString("<b>Font</b>");
            this.GtkLabel13.UseMarkup = true;
            this.frame2.LabelWidget = this.GtkLabel13;
            this.vbox1.Add(this.frame2);
            Gtk.Box.BoxChild w10 = ((Gtk.Box.BoxChild)(this.vbox1[this.frame2]));
            w10.Position = 3;
            this.Add(this.vbox1);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Show();
        }
    }
}
